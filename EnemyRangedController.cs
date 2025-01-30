using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace forged_fury;

public class EnemyRangedController : Character, IDamagable
{
    private readonly Character _playerToFollow;

    private float _defaultAttackDistance = 400f;

    private const int _arrowLaunchDelayMs = 500;
    private int _arrowLaunchTimer = 0;
    private bool _arrowLaunchFlag = false;

    private const int _attackCooldownMs = 2500;
    private bool _hasAttackedFlag = false;
    private int _attackCooldownTimer = 0;

    private readonly ParticleEmitter _particleEmitter;
    private readonly SoundPlayer _soundPlayer;

    private int _scoreReward = 2;

    private float _hitBackAmount = 1000f;

    private float _arrowSpeed = 500f;

    public Vector2 TargetPosition { get; set; }

    public float MinAttackDistance { get; set; }

    public EnemyRangedController(Character playerToFollow, ParticleEmitter particleEmitter, SoundPlayer soundPlayer) : base()
    {
        _particleEmitter = particleEmitter;
        _soundPlayer = soundPlayer;

        var spriteAsset = AssetManager.Textures.Get("EnemyRangedSheet");
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
        _animationController.AttackFrames = 4;

        _playerToFollow = playerToFollow;

        _spawnAnimationEnabled = true;

        MinAttackDistance = _defaultAttackDistance;

        _animationController.AttackPeriod = 200;
    }

    public override void Update(GameTime gameTime)
    {
        FollowTarget();
        ResetAttack(gameTime);
        HandleArrowSpawning(gameTime);
        CheckHealth();
        base.Update(gameTime);
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
        _arrowLaunchFlag = true;
        _attackCooldownTimer = _attackCooldownMs;
        _arrowLaunchTimer = _arrowLaunchDelayMs;
    }

    private void ResetAttack(GameTime gameTime)
    {
        if (_hasAttackedFlag == false) return;
        
        _attackCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;

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

    public void ApplyDamage(int amount)
    {
        Health -= amount;
        HitBack();
    }

    public void HitBack()
    {
        var dif = Vector2.Subtract(Position, TargetPosition);
        var dir = Vector2.Normalize(dif);
        var v = dir * _hitBackAmount;
        Velocity = -Vector2.Subtract(Velocity, v);
    }

    public void HandleArrowSpawning(GameTime gameTime)
    {
        if (_arrowLaunchFlag)
        {
            _arrowLaunchTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (_arrowLaunchTimer <= 0)
            {
                SpawnArrow(_playerToFollow.Position);
                _arrowLaunchFlag = false;
            }
        }
    }

    public void SpawnArrow(Vector2 targetPosition)
    {
        var arrow = new Arrow();

        var dif = Vector2.Subtract(Position, TargetPosition);
        var dir = Vector2.Normalize(dif);
        var v = dir * _arrowSpeed;
        arrow.Velocity = Vector2.Subtract(Velocity, v);

        arrow.Position = Position;
    }
}
