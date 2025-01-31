using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class PlayerController : Character, IDamagable, IHealable, IScoreTracker
{
    public int Score { get; set; }

    private bool _attackButtonPressed = false;
    private bool _attacking = false;

    private readonly ParticleEmitter _particleEmitter;
    private readonly SoundPlayer _soundPlayer;

    private Collider _attackCollider;

    private int _attackColliderDelayMs = 50;
    private int _attackDelayTimer = 0;
    private bool _attackColliderFlag = false;

    private int _attackCooldownMs = 100;
    private bool _hasAttackedFlag = false;
    private int _attackCooldownTimer = 0;

    private int _attackHeight = 40;
    private int _attackWidth = 20;
    private float _attackPositionOffsetAmount = 1.0f;

    private bool _hasDashed = false;
    private bool _dashButtonPressed = false;
    private int _dashCoolDownMs = 2500;
    private int _dashCooldownTimer = 0;
    private float _dashAmount = 8f;

    private int _invulnerableMs = 500;
    private int _invulnerableTimer = 0;
    private bool invulnerable = false;

    private bool _collisionTop = false;
    private bool _collisionBottom = false;
    private bool _collisionLeft = false;
    private bool _collisionRight = false;

    private Vector2 _movement = Vector2.Zero;

    public PlayerController(ParticleEmitter particleEmitter, SoundPlayer soundPlayer) : base()
    {
        _particleEmitter = particleEmitter;
        _soundPlayer = soundPlayer;

        var spriteAsset = AssetManager.Textures.Get("PlayerSheet");
        var spriteSheet = spriteAsset!.AssetObject;
        if (spriteSheet == null) return;
        _animatedSprite = new AnimatedSprite(spriteSheet);
        _animatedSprite.Position = this.Position;
        _animatedSprite.FrameHeight = 64;
        _animatedSprite.FrameWidth = 64;
        _animatedSprite.Width = 64;
        _animatedSprite.Height = 64;
        _animatedSprite.Scale = Scale;

        _animationController = new(_animatedSprite);
        _animationController.AttackFrames = 3;

        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = _attackHeight;
        _attackCollider.Width = _attackWidth;

        _attackCollider.OnCollisionAction = OnAttackCollision;

        _attacking = false;

        HasAltAttack = true;

        _animationController.AttackPeriod = 100;
    }

    public override void Update(GameTime gameTime)
    {
        SetColliderAttackPosition();
        GetInputs();
        SetVelocity(gameTime);
        CheckHealth();
        ResetAttack(gameTime);
        ResetDash(gameTime);
        ResetInvulnerablilty(gameTime);
        base.Update(gameTime);
    }

    private void SetColliderAttackPosition()
    {
        if (_characterDirection == Character.Direction.Right)
        {
            var pos = Position;
            pos.X += Width * _attackPositionOffsetAmount;
            _attackCollider.Position = pos;
        }
        else
        {
            var pos = Position;
            pos.X -= Width * _attackPositionOffsetAmount;
            _attackCollider.Position = pos;
        }
    }

    private void GetInputs()
    {
        if (_hasSpawned == false) return;
        if (_isDying) return;
        var state = Keyboard.GetState();

        _movement = Vector2.Zero;
        if (state.IsKeyDown(Keys.W))
        {
            _movement.Y = -1;
        }
        if (state.IsKeyDown(Keys.S))
        {
            _movement.Y = 1;
        }
        if (state.IsKeyDown(Keys.A))
        {
            _characterDirection = Direction.Left;
            _movement.X = -1;
        }
        if (state.IsKeyDown(Keys.D))
        {
            _characterDirection = Direction.Right;
            _movement.X = 1;
        }
        if (state.IsKeyDown(Keys.Space))
        {
            _attackButtonPressed = true;
        }
        if (state.IsKeyUp(Keys.Space))
        {
            if (_attackButtonPressed)
            {
                Attack();
                _attackButtonPressed = false;
            }
        }
        if (state.IsKeyDown(Keys.LeftShift))
        {
            if (_hasDashed == false)
            {
                _dashButtonPressed = true;
            }
        }
        if (state.IsKeyUp(Keys.LeftShift))
        {
            if (_dashButtonPressed && _hasDashed == false)
            {
                Dash();
                _dashButtonPressed = false;
            }
        }
    }

    private void SetVelocity(GameTime gameTime)
    {
        if (_movement == Vector2.Zero) return;

        var dir = -_movement;
        var v = dir * MoveSpeed;
        var norm = Vector2.Normalize(v);
        Velocity = Vector2.Subtract(Velocity, v);
    }

    private void Attack()
    {
        if (_hasAttackedFlag) return;

        _attackFlag = true;
        _hasAttackedFlag = true;
        _attackCooldownTimer = _attackCooldownMs;
        _attackDelayTimer = _attackColliderDelayMs;
    }

    private void Dash()
    {
        Velocity *= _dashAmount;
        _hasDashed = true;
        _dashCooldownTimer = _dashCoolDownMs;
        invulnerable = true;
        _invulnerableTimer = _invulnerableMs;
    }

    private void ResetDash(GameTime gameTime)
    {
        if (_hasDashed == false) return;
        
        _dashCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_dashCooldownTimer <= 0)
        {
            _dashCooldownTimer = _dashCoolDownMs;
            _hasDashed = false;
        }
    }

    private void ResetInvulnerablilty(GameTime gameTime)
    {
        if (invulnerable == false) return;

        _invulnerableTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_invulnerableTimer <= 0)
        {
            invulnerable = false;
        }
    }

    private void ResetAttack(GameTime gameTime)
    {
        if (_hasAttackedFlag == false) return;

        _attackCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;
        _attackDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_attackDelayTimer <= 0)
        {
            if (_attackCollider.Enabled == false)
            {
                _attackColliderFlag = true;
                _attackCollider.Enabled = true;
            }
        }

        if (_attackCooldownTimer <= 0)
        {
            _hasAttackedFlag = false;
            _attackFlag = false;
            _attackCooldownTimer = 0;
            _attackColliderFlag = false;
            _attackCollider.Enabled = false;
        }
    }

    private void OnAttackCollision(Collider collider)
    {
        if (_attackColliderFlag)
        {
            var hit = collider.Parent;

            if (hit.Name == "Enemy")
            {
                var damageable = (IDamagable)hit;
                if (damageable != null)
                {
                    var rand = new Random();
                    damageable.ApplyDamage(rand.Next(5,15));
                }
                _particleEmitter.Emit(collider.Position);
                _soundPlayer.PlayRandomSound();
                _attackColliderFlag = false;
            }
        }
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            if (_isDying == false)
            {
                DeathFlag = true;
            }
        }
    }

    public void ApplyDamage(int amount)
    {
        if (invulnerable) return;

        Health -= amount;
        if (Health <= 0) Health = 0;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void Heal(int amount)
    {
        Health += amount;
    }
}
