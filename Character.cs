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
using static forged_fury.AnimationController;

namespace forged_fury;

public class Character
{
    private enum Direction
    {
        Left,
        Right
    }

    private readonly float _defaultMoveSpeed = 100f;

    private AnimationController _animationController;
    private readonly AnimatedSprite _animatedSprite;
    protected Collider _characterCollider;

    protected bool _attackFlag = false;

    private Direction _characterDirection = Direction.Right;

    public Vector2 Position;
    public Vector2 Velocity;
    private Texture2D texture2D;

    public float Scale { get; set; }
    public float MoveSpeed { get; set; }


    public Character(Texture2D texture2D)
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

        _animationController = new(_animatedSprite);

        _characterCollider = new Collider();
        _characterCollider.Position = Position;
        _characterCollider.Height = _animatedSprite.Height;
        _characterCollider.Width = _animatedSprite.Width;
    }

    public virtual void Update(GameTime gameTime)
    {
        SetDirection();
        SetSpritePosition();
        SetAnimatorState();
        Move(gameTime);
        _animationController.Update(gameTime);
        _animatedSprite.Update(gameTime);
        _characterCollider.Position = Position;
        _characterCollider.Height = _animatedSprite.Height;
        _characterCollider.Width = _animatedSprite.Width;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animatedSprite.Draw(spriteBatch);
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

    //TO DO - Refactor how animator class works
    private void SetAnimatorState()
    {
        if (_attackFlag && Velocity.X > 0)
        {
            _attackFlag = false;
            _animationController.SetNextState(AnimationController.AnimationStates.AttackRight);
        }
        else if (_attackFlag && Velocity.X < 0)
        {
            _attackFlag = false;
            _animationController.SetNextState(AnimationStates.AttackLeft);
        }
        else if (_attackFlag)
        {
            _attackFlag = false;
            if (_characterDirection == Direction.Right)
            {
                 _animationController.SetNextState(AnimationStates.AttackRight);
            }
            else
            {
                _animationController.SetNextState(AnimationStates.AttackLeft);
            }
        }
        else if (Math.Abs(Velocity.X) <= 0.0001f && Math.Abs(Velocity.Y) <= 0.0001f)
        {
            if (_characterDirection == Direction.Right)
            {
                _animationController.SetNextState(AnimationStates.IdleRight);
            }
            else
            {
                _animationController.SetNextState(AnimationStates.IdleLeft);
            }
        }
        else if (Velocity.X > 0)
        {
            _animationController.SetNextState(AnimationStates.RunRight);
        }
        else if (Velocity.X < 0)
        {
            _animationController.SetNextState(AnimationStates.RunLeft);
        }
        else if (Velocity.Y < 0)
        {
            _animationController.SetNextState(AnimationStates.RunRight);
        }
        else if (Velocity.Y > 0)
        {
            _animationController.SetNextState(AnimationStates.RunLeft);
        }
    }
}
