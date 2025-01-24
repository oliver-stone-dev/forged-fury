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

public class PlayerController : Character
{
    private bool _attackButtonPressed = false;
    private bool _attacking = false;

    private Collider _attackCollider;

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
    }

    public override void Update(GameTime gameTime)
    {
        SetColliderAttackPosition();
        SetVelocity(gameTime);
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
                _attackCollider.Enabled = true;
                _attackFlag = true;
                _attackButtonPressed = false;
                _attacking = true;
            }
        }
    }

    private void OnAttackCollision(Collider collider)
    {
        if (_attacking)
        {
            var hit = collider.Parent;

            if (hit.Name == "Enemy")
            {
                var damageable = (IDamagable)hit;
                if (damageable != null)
                {
                    damageable.ApplyDamage(10);
                }
            }

            _attackCollider.Enabled = false;
        }

        _attacking = false;
        //Debug.WriteLine($"Attack collision with {collider.Parent.Name}");
    }
}
