using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class PlayerCharacter
{
    private enum AnimationStates
    {

        Idle,
        RunRight,
        RunLeft,
        AttackRight,
        AttackLeft
    }

    private readonly float _defaultMoveSpeed = 140f;
    private readonly AnimatedSprite _animatedSprite;
    private AnimationStates _animationState;
    private bool _waitToFinish = false;

    private bool _attackFlag = false;
    private bool _attackButtonPressed = false;

    public Vector2 Position;
    public Vector2 Velocity;

    public float Scale { get; set; }
    public float MoveSpeed { get; set; }


    public PlayerCharacter(Texture2D texture2D)
    {
        MoveSpeed = _defaultMoveSpeed;
        Position = Vector2.Zero;
        Velocity = Vector2.Zero;
        Scale = 2f;

        _animationState = AnimationStates.Idle;

        _animatedSprite = new AnimatedSprite(texture2D);
        _animatedSprite.Position = this.Position;
        _animatedSprite.FrameHeight = 64;
        _animatedSprite.FrameWidth = 64;
        _animatedSprite.Width = 64;
        _animatedSprite.Height = 64;
        _animatedSprite.Scale = Scale;
        _animatedSprite.MaxFrame = 4;
        _animatedSprite.Period = 250;

        _animatedSprite.Start();
    }

    public void Update(GameTime gameTime)
    {
        SetVelocity();
        Move(gameTime);
        SetSpritePosition();
        HandleAnimationState();
        _animatedSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animatedSprite.Draw(spriteBatch);
    }

    private void SetVelocity()
    {
        var state = Keyboard.GetState();

        Velocity = Vector2.Zero;

        if (state.IsKeyDown(Keys.W)) //UP
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

    private void Move(GameTime gameTime)
    {
        Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void SetSpritePosition()
    {
        _animatedSprite.Position = this.Position;
    }

    private AnimationStates GetNextAnimationState()
    {
        if (Math.Abs(Velocity.X) <= 0.0001f && Math.Abs(Velocity.Y) <= 0.0001f)
        {
            return AnimationStates.Idle;
        }
        else if (Velocity.X > 0)
        {
            return AnimationStates.RunRight;
        }
        else if (Velocity.X < 0)
        {
            return AnimationStates.RunLeft;
        }
        else if (Velocity.Y < 0)
        {
            return AnimationStates.RunRight;
        }
        else if (Velocity.Y > 0)
        {
            return AnimationStates.RunLeft;
        }

        return AnimationStates.Idle;
    }

    private void HandleAnimationState()
    {
        var state = GetNextAnimationState();
        if (state == _animationState) return;

        _animatedSprite.Stop();

        _animationState = state;
        switch (_animationState)
        {
            case (AnimationStates.Idle):
                _animatedSprite.AnimationRow = 0;
                _animatedSprite.MaxFrame = 4;
                _animatedSprite.Period = 250;
                _animatedSprite.Loop = false;
                _waitToFinish = false;
                break;
            case (AnimationStates.RunRight):
                _animatedSprite.AnimationRow = 1;
                _animatedSprite.MaxFrame = 6;
                _animatedSprite.Period = 150;
                _animatedSprite.Loop = true;
                _waitToFinish = false;
                break;
            case (AnimationStates.RunLeft):
                _animatedSprite.AnimationRow = 2;
                _animatedSprite.MaxFrame = 6;
                _animatedSprite.Period = 150;
                _animatedSprite.Loop = true;
                _waitToFinish = false;
                break;
            case (AnimationStates.AttackRight):
                _animatedSprite.AnimationRow = 3;
                _animatedSprite.MaxFrame = 3;
                _animatedSprite.Period = 100;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.AttackLeft):
                _animatedSprite.AnimationRow = 4;
                _animatedSprite.MaxFrame = 3;
                _animatedSprite.Period = 100;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            default:
                break;
        }

        _animatedSprite.Start();
    }
}
