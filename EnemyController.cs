using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace forged_fury;

public class EnemyController : Character
{
    private readonly Character _playerToFollow;

    private int _attackCooldownMs = 1000;
    private bool _hasAttackedFlag = false;
    private bool _attacking = false;
    private int _attackCooldownTimer = 0;

    private Collider _attackCollider;

    public static readonly float _defaultAttackDistance = 50f;

    public Vector2 TargetPosition { get; set; }

    public float MinAttackDistance { get; set; }

    public EnemyController(Texture2D texture2D, Character playerToFollow) : base(texture2D)
    {
        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = 50;
        _attackCollider.Width = 10;

        MinAttackDistance = _defaultAttackDistance;
        _playerToFollow = playerToFollow;
        _characterCollider.OnCollisionAction = OnCharacterCollision;
        _attackCollider.OnCollisionAction = OnAttackCollision;
    }

    public override void Update(GameTime gameTime)
    {
        if (_characterDirection == Character.Direction.Right)
        {
            var pos = Position;
            pos.X += 40;
            _attackCollider.Position = pos;
        }
        else
        {
            var pos = Position;
            pos.X -= 40;
            _attackCollider.Position = pos;
        }

        FollowTarget();
        ResetAttack(gameTime);
        base.Update(gameTime);
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

            Velocity = -(dir * MoveSpeed);
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

    private void OnCharacterCollision(Collider collider)
    {
        //collider.Parent.Destroy();
        //Debug.WriteLine($"Character collision with {collider.Parent.Name}");
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
}
