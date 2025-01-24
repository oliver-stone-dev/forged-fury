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

public class PlayerController : Character, IDamagable
{
    private bool _attackButtonPressed = false;
    private bool _attacking = false;

    private Collider _attackCollider;

    private int _attackColliderDelayMs = 50;
    private int _attackDelayTimer = 0;
    private bool _attackColliderFlag = false;

    private int _attackCooldownMs = 100;
    private bool _hasAttackedFlag = false;
    private int _attackCooldownTimer = 0;

    private int _attackHeight = 50;
    private int _attackWidth = 30;
    private int _attackPosition = 55;

    private bool _collisionTop = false;
    private bool _collisionBottom = false;
    private bool _collisionLeft = false;
    private bool _collisionRight = false;


    public PlayerController(Texture2D texture2D) : base(texture2D)
    {
        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = _attackHeight;
        _attackCollider.Width = _attackWidth;

        _attackCollider.OnCollisionAction = OnAttackCollision;

        _attacking = false;
    }

    public override void Update(GameTime gameTime)
    {
        SetColliderAttackPosition();
        SetVelocity(gameTime);
        ResetAttack(gameTime);
        base.Update(gameTime);
    }

    private void SetColliderAttackPosition()
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

    private void SetVelocity(GameTime gameTime)
    {
        var state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.W))
        {
            Velocity.Y -= MoveSpeed;
        }
        if (state.IsKeyDown(Keys.S))
        {
            Velocity.Y += MoveSpeed;
        }
        if (state.IsKeyDown(Keys.A))
        {
            Velocity.X -= MoveSpeed;
        }
        if (state.IsKeyDown(Keys.D))
        {
            Velocity.X += MoveSpeed;
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
            _attackColliderFlag = true;
            _attackCollider.Enabled = true;
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
                    damageable.ApplyDamage(10);
                }
                _attackColliderFlag = false;
            }
        }
    }

    public void ApplyDamage(double amount)
    {
        Debug.WriteLine(Health);
        Health -= amount;
    }
}
