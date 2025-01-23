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

    public PlayerController(Texture2D texture2D) : base(texture2D)
    {
        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = 50;
        _attackCollider.Width = 10;


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

        SetVelocity();
        base.Update(gameTime);
    }

    private void SetVelocity()
    {
        var state = Keyboard.GetState();

        Velocity = Vector2.Zero;

        if (state.IsKeyDown(Keys.W))
        {
            Velocity.Y = -MoveSpeed;
        }
        if (state.IsKeyDown(Keys.S))
        {
            Velocity.Y = MoveSpeed;
        }
        if (state.IsKeyDown(Keys.A))
        {
            Velocity.X = -MoveSpeed;
        }
        if (state.IsKeyDown(Keys.D))
        {
            Velocity.X = MoveSpeed;
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

    private void OnCharacterCollision(Collider collider)
    {
        //collider.Parent.Destroy();
        //Debug.WriteLine($"Character collision with {collider.Parent.Name}");
    }

    private void OnAttackCollision(Collider collider)
    {
        if (_attacking)
        {
            collider.Parent.Destroy();
            _attackCollider.Enabled = false;
        }

        _attacking = false;
        //Debug.WriteLine($"Attack collision with {collider.Parent.Name}");
    }
}
