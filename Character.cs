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

        IdleRight,
        IdleLeft,
        RunRight,
        RunLeft,
        AttackRight,
        AttackLeft
    }

    private enum Direction
    {
        Left,
        Right
    }

    private readonly float _defaultMoveSpeed = 100f;
    private readonly AnimatedSprite _animatedSprite;
    private AnimationStates _animationState;
    private bool _waitToFinish = false;

    private bool _attackFlag = false;
    private bool _attackButtonPressed = false;

    private Direction _characterDirection = Direction.Right;

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

        _animatedSprite = new AnimatedSprite(texture2D);
        _animatedSprite.Position = this.Position;
        _animatedSprite.FrameHeight = 64;
        _animatedSprite.FrameWidth = 64;
        _animatedSprite.Width = 64;
        _animatedSprite.Height = 64;
        _animatedSprite.Scale = Scale;

        _animationState = AnimationStates.IdleRight;
        SetAnimatedSpriteAnimation(_animationState);
        _animatedSprite.Start();
    }

    public void Update(GameTime gameTime)
    {
        SetVelocity();
        SetDirection();
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

    private void Move(GameTime gameTime)
    {
        Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void SetSpritePosition()
    {
        _animatedSprite.Position = this.Position;
    }

    private void SetDirection()
    {
        if (Velocity.X > 0)
        {
            _characterDirection = Direction.Right;
        }
        else if (Velocity.X < 0)
        {
            _characterDirection = Direction.Left;
        }
        else if (Velocity.Y < 0)
        {
            _characterDirection = Direction.Right;
        }
        else if (Velocity.Y > 0)
        {
            _characterDirection = Direction.Left;
        }
    }

    private AnimationStates GetNextAnimationState()
    {
        if (_attackFlag && Velocity.X > 0)
        {
            _attackFlag = false;
            return AnimationStates.AttackRight;
        }
        else if (_attackFlag && Velocity.X < 0)
        {
            _attackFlag = false;
            return AnimationStates.AttackLeft;
        }
        else if (_attackFlag)
        {
            _attackFlag = false;
            if (_characterDirection == Direction.Right)
            {
                return AnimationStates.AttackRight;
            }
            else
            {
                return AnimationStates.AttackLeft;
            }
        }
        else if (Math.Abs(Velocity.X) <= 0.0001f && Math.Abs(Velocity.Y) <= 0.0001f)
        {
            if (_characterDirection == Direction.Right)
            {
                return AnimationStates.IdleRight;
            }
            else
            {
                return AnimationStates.IdleLeft;
            }
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

        return AnimationStates.IdleRight;
    }

    private void SetAnimatedSpriteAnimation(AnimationStates state)
    {
        switch (state)
        {
            case (AnimationStates.IdleRight):
                _animatedSprite.AnimationRow = 0;
                _animatedSprite.MaxFrame = 4;
                _animatedSprite.Period = 250;
                _animatedSprite.Loop = true;
                _waitToFinish = false;
                break;
            case (AnimationStates.IdleLeft):
                _animatedSprite.AnimationRow = 1;
                _animatedSprite.MaxFrame = 4;
                _animatedSprite.Period = 250;
                _animatedSprite.Loop = true;
                _waitToFinish = false;
                break;
            case (AnimationStates.RunRight):
                _animatedSprite.AnimationRow = 2;
                _animatedSprite.MaxFrame = 6;
                _animatedSprite.Period = 100;
                _animatedSprite.Loop = true;
                _waitToFinish = false;
                break;
            case (AnimationStates.RunLeft):
                _animatedSprite.AnimationRow = 3;
                _animatedSprite.MaxFrame = 6;
                _animatedSprite.Period = 100;
                _animatedSprite.Loop = true;
                _waitToFinish = false;
                break;
            case (AnimationStates.AttackRight):
                _animatedSprite.AnimationRow = 4;
                _animatedSprite.MaxFrame = 3;
                _animatedSprite.Period = 100;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.AttackLeft):
                _animatedSprite.AnimationRow = 5;
                _animatedSprite.MaxFrame = 3;
                _animatedSprite.Period = 100;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            default:
                break;
        }
    }

    private void HandleAnimationState()
    {
        var state = GetNextAnimationState();

        switch (_animationState)
        {
            case (AnimationStates.IdleRight):
                if (state != _animationState)
                {
                    _animatedSprite.Stop();
                    _animationState = state;
                    SetAnimatedSpriteAnimation(_animationState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.IdleLeft):
                if (state != _animationState)
                {
                    _animatedSprite.Stop();
                    _animationState = state;
                    SetAnimatedSpriteAnimation(_animationState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.RunRight):
                if (state != _animationState)
                {
                    _animatedSprite.Stop();
                    _animationState = state;
                    SetAnimatedSpriteAnimation(_animationState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.RunLeft):
                if (state != _animationState)
                {
                    _animatedSprite.Stop();
                    _animationState = state;
                    SetAnimatedSpriteAnimation(_animationState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.AttackRight):
                if (state != _animationState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _animationState = state;
                    SetAnimatedSpriteAnimation(_animationState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.AttackLeft):
                if (state != _animationState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _animationState = state;
                    SetAnimatedSpriteAnimation(_animationState);
                    _animatedSprite.Start();
                }
                break;
            default:
                break;
        }
    }
}
