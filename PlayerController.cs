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


    public PlayerController(Texture2D texture2D) : base(texture2D)
    {
        _characterCollider.OnCollisionEnterAction = OnCollision;
    }

    public override void Update(GameTime gameTime)
    {
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

    private void OnCollision()
    {
        Debug.WriteLine("Collision!");
    }
}
