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
    private double _startingHealth = 10f;

    private readonly Character _playerToFollow;

    private int _attackCooldownMs = 1000;
    private bool _hasAttackedFlag = false;
    private bool _attacking = false;
    private int _attackCooldownTimer = 0;

    private Collider _attackCollider;

    private float _defaultAttackDistance = 60f;
    private int _attackHeight = 40;
    private int _attackWidth = 20;
    private int _attackPosition = 40;

    public double Health { get; set; }

    public Vector2 TargetPosition { get; set; }

    public float MinAttackDistance { get; set; }

    public EnemyController(Texture2D texture2D, Character playerToFollow) : base(texture2D)
    {
        Health = _startingHealth;

        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = _attackHeight;
        _attackCollider.Width = _attackWidth;

        MinAttackDistance = _defaultAttackDistance;
        _playerToFollow = playerToFollow;
        _characterCollider.OnCollisionAction = OnCharacterCollision;
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
        TargetPosition = _playerToFollow.Position;

        var distance = Vector2.Distance(Position, TargetPosition);

        if (distance <= MinAttackDistance)
        {
            Velocity = Vector2.Zero;
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

        _attacking = true;
        _attackFlag = true;
        _hasAttackedFlag = true;
        _attackCooldownTimer = _attackCooldownMs;
        _attackCollider.Enabled = true;
    }

    private void ResetAttack(GameTime gameTime)
    {
        if (_hasAttackedFlag == false) return;
        
        _attackCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;
        if (_attackCooldownTimer <= 0)
        {
            _hasAttackedFlag = false;
            _attackFlag = false;
            _attackCooldownTimer = 0;
        }
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            Destroy();
        }
    }

    private void OnCharacterCollision(Collider collider)
    {
        if (collider.Name == "solid")
        {
            //var sides = ColliderManager.GetCollisionSides(_characterCollider, collider);
           // Velocity = Vector2.Zero;
        }
    }

    private void OnAttackCollision(Collider collider)
    {
        if (_attacking)
        {
            _attackCollider.Enabled = false;
        }

        _attacking = false;

        //Debug.WriteLine($"Attack collision with {collider.Parent.Name}");
    }

    public void ApplyDamage(double amount)
    {
        Health -= amount;
    }
}
