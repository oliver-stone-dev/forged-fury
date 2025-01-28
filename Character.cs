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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace forged_fury;

public class Character : GameObject
{
    public enum Direction
    {
        Left,
        Right
    }

    private int _health = 0;
    private int _startingHealth = 10;
    private int _maxHealth = 100;

    private readonly float _defaultMoveSpeed = 40f;
    private readonly float _maxSpeed = 100f;
    private readonly float _stoppedVelocity = 10f;

    protected bool _isDying = false;
    private int _deathDelayMs = 800;
    private int _deathDelayTimer = 0;

    private AnimationController _animationController;
    private readonly AnimatedSprite _animatedSprite;
    private readonly Sprite _shadow;
    protected Collider _characterCollider;

    protected bool _attackFlag = false;

    protected Direction _characterDirection = Direction.Right;

    public Vector2 Velocity;
    private Texture2D texture2D;

    private int _shadowYOffset = 18;

    private float _aliveFriction = 0.8f;
    private float _deadFriction = 0.7f;

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            if (_animatedSprite != null) _animatedSprite.Position = value;
            if (_characterCollider != null) _characterCollider.Position = value;
        }
    }

    public int Health 
    {
        get => _health;
        set => _health = value >= _maxHealth ? _maxHealth : value;
    }

    public float Scale { get; set; }
    public float MoveSpeed { get; set; }
    public float Friction { get; set; }
    public bool DeathFlag { get; set; }

    public bool HasAltAttack { get; set; }

    public Character(Texture2D texture2D, Texture2D shadow) : base()
    {
        Health = _startingHealth;

        MoveSpeed = _defaultMoveSpeed;
        _position = Vector2.Zero;
        Velocity = Vector2.Zero;
        Scale = 2f;

        _animatedSprite = new AnimatedSprite(texture2D);
        _animatedSprite.Position = this.Position;
        _animatedSprite.FrameHeight = 64;
        _animatedSprite.FrameWidth = 64;
        _animatedSprite.Width = 64;
        _animatedSprite.Height = 64;
        _animatedSprite.Scale = Scale;

        _shadow = new Sprite(shadow);
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
        _characterCollider.Enabled = true;

        Friction = _aliveFriction;
    }

    public override void Update(GameTime gameTime)
    {
        SetDirection();
        SetAnimatorState();
        ClampVelocity();
        ApplyFriction();
        SetShadowPosition();
        Move(gameTime);
        OnDeath(gameTime);
        _animationController.Update(gameTime);
        _animatedSprite.Update(gameTime);
        _characterCollider.Position = Position;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _shadow.Draw(spriteBatch);
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

        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
/*
        Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;*/
    }

    private void ApplyFriction()
    {
       Velocity *= Friction;
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
        if (_isDying == true) return;

        if (DeathFlag)
        {
            Friction = _deadFriction;
            DeathFlag = false;
            _deathDelayTimer = _deathDelayMs;
            _isDying = true;
            if (_characterDirection == Direction.Right)
            {
                _animationController.SetNextState(AnimationController.AnimationStates.DeathRight);
            }
            else
            {
                _animationController.SetNextState(AnimationController.AnimationStates.DeathLeft);
            }
        }
        else if (_attackFlag)
        {
            var rand = new Random();
            _attackFlag = false;
            if (_characterDirection == Direction.Right)
            {
                if ( (rand.Next(0, 2) == 0) && HasAltAttack)
                {
                    _animationController.SetNextState(AnimationController.AnimationStates.AttackRightAlt);
                }
                else
                {
                    _animationController.SetNextState(AnimationController.AnimationStates.AttackRight);
                }
            }
            else
            {
                if ((rand.Next(0, 2) == 0) && HasAltAttack)
                {
                    _animationController.SetNextState(AnimationController.AnimationStates.AttackLeftAlt);
                }
                else
                {
                    _animationController.SetNextState(AnimationController.AnimationStates.AttackLeft);
                }
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

    private void SetShadowPosition()
    {
        var pos = Position;
        pos.Y += _shadowYOffset;
        _shadow.Position = pos;
    }

    protected void ResetAnimation()
    {
        _animationController.Reset();
    }

    private void OnDeath(GameTime gameTime)
    {
        if (_isDying)
        {
            _characterCollider.Enabled = false;

            _deathDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (_deathDelayTimer <= 0)
            {
                Destroy();
            }
        }
    }
}
