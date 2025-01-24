using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Character : GameObject
{
    public enum Direction
    {
        Left,
        Right
    }

    private readonly float _defaultMoveSpeed = 40f;
    private readonly float _maxSpeed = 100f;
    private readonly float _stoppedVelocity = 10f;

    private AnimationController _animationController;
    private readonly AnimatedSprite _animatedSprite;
    protected Collider _characterCollider;

    protected bool _attackFlag = false;

    protected Direction _characterDirection = Direction.Right;

    public Vector2 Position;
    public Vector2 Velocity;
    private Texture2D texture2D;

    public float Scale { get; set; }
    public float MoveSpeed { get; set; }

    public float Friction { get; set; }


    public Character(Texture2D texture2D) : base()
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

        _characterCollider = new Collider(this);
        _characterCollider.Position = Position;
        _characterCollider.Height = 90;
        _characterCollider.Width = 50;
        _characterCollider.Name = "solid";

        Friction = 0.90f;

        _characterCollider.OnCollisionAction = OnCharacterCollision;
    }

    public override void Update(GameTime gameTime)
    {
        SetDirection();
        SetSpritePosition();
        SetAnimatorState();
        ClampVelocity();
        ApplyFriction();
        Move(gameTime);
        _animationController.Update(gameTime);
        _animatedSprite.Update(gameTime);
        _characterCollider.Position = Position;
        _characterCollider.Enabled = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _animatedSprite.Draw(spriteBatch);
    }

    private void ClampVelocity()
    {
        Vector2.Clamp(Velocity, new Vector2(-_maxSpeed, -_maxSpeed), new Vector2(_maxSpeed, _maxSpeed));
    }

    private void Move(GameTime gameTime)
    {
        if (_characterCollider.TopCollision && Velocity.Y < 0) Velocity.Y = 0;
        if (_characterCollider.BottomCollision && Velocity.Y > 0) Velocity.Y = 0;
        if (_characterCollider.LeftCollision && Velocity.X < 0) Velocity.X = 0;
        if (_characterCollider.RightCollision && Velocity.X > 0) Velocity.X = 0;

        Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void ApplyFriction()
    {
       Velocity *= Friction;
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
        if (_attackFlag)
        {
            _attackFlag = false;
            if (_characterDirection == Direction.Right)
            {
                 _animationController.SetNextState(AnimationController.AnimationStates.AttackRight);
            }
            else
            {
                _animationController.SetNextState(AnimationController.AnimationStates.AttackLeft);
            }
        }
        else if (Math.Abs(Velocity.X) <= _stoppedVelocity && Math.Abs(Velocity.Y) <= _stoppedVelocity)
        {
            if (_characterDirection == Direction.Right)
            {
                _animationController.SetNextState(AnimationController.AnimationStates.IdleRight);
            }
            else
            {
                _animationController.SetNextState(AnimationController.AnimationStates.IdleLeft);
            }
        }
        else if (Velocity.X > 0)
        {
            _animationController.SetNextState(AnimationController.AnimationStates.RunRight);
        }
        else if (Velocity.X < 0)
        {
            _animationController.SetNextState(AnimationController.AnimationStates.RunLeft);
        }
        else if (Velocity.Y < 0)
        {
            _animationController.SetNextState(AnimationController.AnimationStates.RunRight);
        }
        else if (Velocity.Y > 0)
        {
            _animationController.SetNextState(AnimationController.AnimationStates.RunLeft);
        }
    }


    private void OnCharacterCollision(Collider collider)
    {
        if (collider.Name == "solid")
        {
            Debug.WriteLine(_characterCollider.TopCollision);
            // Velocity = Vector2.Zero;
        }
    }
}
