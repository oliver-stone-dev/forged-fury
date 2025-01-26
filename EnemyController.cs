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

    private int _attackColliderDelayMs = 250;
    private int _attackDelayTimer = 0;
    private bool _attackColliderFlag = false;

    private int _attackCooldownMs = 1000;
    private bool _hasAttackedFlag = false;
    private int _attackCooldownTimer = 0;

    private Collider _attackCollider;

    private float _defaultAttackDistance = 60f;
    private int _attackHeight = 40;
    private int _attackWidth = 20;
    private int _attackPosition = 40;

    private int _scoreReward = 1;

    private float _hitBackAmount = 2000f;

    public Vector2 TargetPosition { get; set; }

    public float MinAttackDistance { get; set; }

    public EnemyController(Texture2D texture2D, Character playerToFollow) : base(texture2D)
    {
        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = _attackHeight;
        _attackCollider.Width = _attackWidth;

        MinAttackDistance = _defaultAttackDistance;
        _playerToFollow = playerToFollow;
        _attackCollider.OnCollisionAction = OnAttackCollision;
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
        if (_playerToFollow.Health <= 0) return;

        TargetPosition = _playerToFollow.Position;

        var distance = Vector2.Distance(Position, TargetPosition);

        if (distance <= MinAttackDistance)
        {
            //Velocity = Vector2.Zero;
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
            _hasAttackedFlag = false;
            _attackFlag = false;
            _attackCooldownTimer = 0;
            _attackColliderFlag = false;
            _attackCollider.Enabled = false;
        }
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            var scoreTracker = (IScoreTracker)_playerToFollow;
            if (scoreTracker != null)
            {
                scoreTracker.AddScore(_scoreReward);
            }
            Destroy();
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
                    damageable.ApplyDamage(10);
                }
                _attackColliderFlag = false;
            }
        }
    }

    public void ApplyDamage(double amount)
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
}
