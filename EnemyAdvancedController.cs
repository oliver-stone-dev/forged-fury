using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace forged_fury;

public class EnemyAdvancedController : Character, IDamagable
{
    private readonly Character _playerToFollow;

    private const int _attackDelayMs = 200;
    private int _attackDelayTimer = 0;
    private bool _attackingCharging = false;

    private const int _attackColliderDelayMs = 200;
    private int _attackCollisionDelayTimer = 0;
    private bool _attackColliderFlag = false;

    private const int _attackCooldownMs = 1000;
    private bool _hasAttackedFlag = false;
    private int _attackCooldownTimer = 0;

    private Collider _attackCollider;

    private readonly ParticleEmitter _particleEmitter;
    private readonly SoundPlayer _soundPlayer;

    private float _defaultAttackDistance = 35f;
    private int _attackHeight = 30;
    private int _attackWidth = 20;
    private float _attackPositionOffsetAmount = 1.0f;

    private int _scoreReward = 1;

    private float _hitBackAmount = 1000f;

    public Vector2 TargetPosition { get; set; }

    public float MinAttackDistance { get; set; }

    public EnemyAdvancedController(Character playerToFollow, ParticleEmitter particleEmitter, SoundPlayer soundPlayer) : base()
    {
        _particleEmitter = particleEmitter;
        _soundPlayer = soundPlayer;

        var spriteAsset = AssetManager.Textures.Get("EnemyAdvancedSheet");
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

        MinAttackDistance = _defaultAttackDistance;
        _playerToFollow = playerToFollow;
        _attackCollider.OnCollisionAction = OnAttackCollision;

        _spawnAnimationEnabled = true;

        _animationController.AttackPeriod = 100;
    }

    public override void Update(GameTime gameTime)
    {
        SetAttackColliderPosition();
        FollowTarget();
        HandleAttackCharge(gameTime);
        ResetAttack(gameTime);
        CheckHealth();
        base.Update(gameTime);
    }

    private void SetAttackColliderPosition()
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

    private void FollowTarget()
    {
        if (_hasSpawned == false) return;
        if (_isDying) return;
        if (_playerToFollow.Health <= 0) return;

        TargetPosition = _playerToFollow.Position;

        if (TargetPosition.X >= Position.X) _characterDirection = Direction.Right;
        else if (TargetPosition.X < Position.X) _characterDirection = Direction.Left;

        var distance = Vector2.Distance(Position, TargetPosition);

        if (distance <= MinAttackDistance)
        {
            ChargeAttack();
        }
        else
        {
            var dif = Vector2.Subtract(Position, TargetPosition);
            var dir = Vector2.Normalize(dif);
            var v = dir * MoveSpeed;
            Velocity = Vector2.Subtract(Velocity, v);
            _attackingCharging = false;
            StopAttack();
        }
    }

    private void ChargeAttack()
    {
        if (_attackingCharging) return;

        _attackingCharging = true;
        _attackDelayTimer = _attackDelayMs;
    }

    private void HandleAttackCharge(GameTime gameTime)
    {
        if (_attackingCharging == false) return;

        _attackDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;
        if (_attackDelayTimer <= 0)
        {
            Attack();
            _attackingCharging = false;
            _attackDelayTimer = _attackDelayMs;
        }
    }

    private void Attack()
    {
        if (_hasAttackedFlag) return;

        _attackFlag = true;
        _hasAttackedFlag = true;
        _attackCooldownTimer = _attackCooldownMs;
        _attackCollisionDelayTimer = _attackColliderDelayMs;
    }

    private void ResetAttack(GameTime gameTime)
    {
        if (_hasAttackedFlag == false) return;
        
        _attackCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;
        _attackCollisionDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_attackCollisionDelayTimer <= 0)
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
        _attackCollisionDelayTimer = _attackColliderDelayMs;
        _attackColliderFlag = false;
        _attackCollider.Enabled = false;
        _attackingCharging = false;
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
                StopAttack();
                _characterCollider.IgnoreName = "Player";
                DeathFlag = true;
                _attackCollider.Enabled = false;
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
