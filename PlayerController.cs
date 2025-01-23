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

    private Collider _attackCollider;

    public PlayerController(Texture2D texture2D) : base(texture2D)
    {
        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = 10;
        _attackCollider.Width = 50;

        _characterCollider.OnCollisionAction = OnCharacterCollision;
    }

    public override void Update(GameTime gameTime)
    {
        _attackCollider.Position = Position;



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
                _attackFlag = true;
                _attackButtonPressed = false;
            }
        }
    }

    private void OnCharacterCollision(Collider collider)
    {
        Debug.WriteLine($"Character collision with {collider.Parent.Name}");
    }

    private void OnAttackCollidion(Collider collider)
    {
        Debug.WriteLine($"Attack collision with {collider.Parent.Name}");
    }
}
