using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace forged_fury;

public class EnemyController : Character, IDamagable
{
    private readonly Character _playerToFollow;

    private int _attackColliderDelayMs = 350;
    private int _attackDelayTimer = 0;
    private bool _attackColliderFlag = false;

    private int _attackCooldownMs = 1000;
    private bool _hasAttackedFlag = false;
    private int _attackCooldownTimer = 0;

    private Collider _attackCollider;

    private readonly ParticleEmitter _particleEmitter;
    private readonly SoundPlayer _soundPlayer;

    private float _defaultAttackDistance = 70f;
    private int _attackHeight = 50;
    private int _attackWidth = 30;
    private int _attackPosition = 40;

    private int _scoreReward = 1;

    private float _hitBackAmount = 1500f;

    public Vector2 TargetPosition { get; set; }

    public float MinAttackDistance { get; set; }

    public EnemyController(Character playerToFollow, ParticleEmitter particleEmitter, SoundPlayer soundPlayer) : base()
    {
        _particleEmitter = particleEmitter;
        _soundPlayer = soundPlayer;

        var spriteAsset = AssetManager.Textures.Get("EnemyAdvancedSheet");
        var spriteSheet = spriteAsset!.AssetObject;
        if (spriteSheet == null) return;
        if (spriteSheet == null) return;

        _animatedSprite = new AnimatedSprite(spriteSheet);
        _animatedSprite.Position = this.Position;
        _animatedSprite.FrameHeight = 64;
        _animatedSprite.FrameWidth = 64;
        _animatedSprite.Width = 64;
        _animatedSprite.Height = 64;
        _animatedSprite.Scale = Scale;

        _animationController = new(_animatedSprite);

        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = _attackHeight;
        _attackCollider.Width = _attackWidth;

        MinAttackDistance = _defaultAttackDistance;
        _playerToFollow = playerToFollow;
        _attackCollider.OnCollisionAction = OnAttackCollision;

        _spawnAnimationEnabled = true;
    }

    public override void Update(GameTime gameTime)
    {
        SetAttackColliderPosition();
        FollowTarget();
        ResetAttack(gameTime);
        CheckHealth();
        base.Update(gameTime);
    }

    private void SetAttackColliderPosition()
    {
        if (_characterDirection == Character.Direction.Right)
        {
            var pos = Position;
            pos.X += _attackPosition;
            _attackCollider.Position = pos;
        }
        else
        {
            var pos = Position;
            pos.X -= _attackPosition;
            _attackCollider.Position = pos;
        }
    }

    private void FollowTarget()
    {
        if (_hasSpawned == false) return;
        if (_isDying) return;
        if (_playerToFollow.Health <= 0) return;

        TargetPosition = _playerToFollow.Position;

        if (TargetPosition.X > Position.X) _characterDirection = Direction.Right;
        else if (TargetPosition.X < Position.X) _characterDirection = Direction.Left;

        var distance = Vector2.Distance(Position, TargetPosition);

        if (distance <= MinAttackDistance)
        {
            Attack();
        }
        else
        {
            var dif = Vector2.Subtract(Position, TargetPosition);
            var dir = Vector2.Normalize(dif);
            var v = dir * MoveSpeed;
            Velocity = Vector2.Subtract(Velocity, v);
        }
    }

    private void Attack()
    {
        if (_hasAttackedFlag) return;

        _attackFlag = true;
        _hasAttackedFlag = true;
        _attackCooldownTimer = _attackCooldownMs;
        _attackDelayTimer = _attackColliderDelayMs;
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
            StopAttack();
        }
    }

    private void StopAttack()
    {
        _hasAttackedFlag = false;
        _attackFlag = false;
        _attackCooldownTimer = _attackCooldownMs;
        _attackDelayTimer = _attackColliderDelayMs;
        _attackColliderFlag = false;
        _attackCollider.Enabled = false;
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            if (_isDying == false)
            {
                var scoreTracker = (IScoreTracker)_playerToFollow;
                if (scoreTracker != null)
                {
                    scoreTracker.AddScore(_scoreReward);
                }
                DeathFlag = true;
            }
        }
    }

    private void OnAttackCollision(Collider collider)
    {
        if (_attackColliderFlag)
        {
            var hit = collider.Parent;

            if (hit.Name == "Player")
            {
                var damageable = (IDamagable)hit;
                if (damageable != null)
                {
                    var rand = new Random();
                    damageable.ApplyDamage(rand.Next(2, 10));
                }
                _particleEmitter.Emit(collider.Position);
                _soundPlayer.PlayRandomSound();
                _attackColliderFlag = false;
            }
        }
    }

    public void ApplyDamage(int amount)
    {
        Health -= amount;
        StopAttack();
        HitBack();
    }

    public void HitBack()
    {
        var dif = Vector2.Subtract(Position, TargetPosition);
        var dir = Vector2.Normalize(dif);
        var v = dir * _hitBackAmount;
        Velocity = -Vector2.Subtract(Velocity, v);
    }
}
