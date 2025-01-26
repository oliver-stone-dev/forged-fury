﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class PlayerController : Character, IDamagable, IScoreTracker
{
    public int Score { get; set; }

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

    private bool _hasDashed = false;
    private bool _dashButtonPressed = false;
    private int _dashCoolDownMs = 2000;
    private int _dashCooldownTimer = 0;
    private float _dashAmount = 4f;

    private bool _collisionTop = false;
    private bool _collisionBottom = false;
    private bool _collisionLeft = false;
    private bool _collisionRight = false;

    private Vector2 _movement = Vector2.Zero;

    public PlayerController(Texture2D texture2D,Texture2D shadow) : base(texture2D, shadow)
    {
        _attackCollider = new(this);
        _attackCollider.Enabled = false;
        _attackCollider.Height = _attackHeight;
        _attackCollider.Width = _attackWidth;

        _attackCollider.OnCollisionAction = OnAttackCollision;

        _attacking = false;

        HasAltAttack = true;
    }

    public override void Update(GameTime gameTime)
    {
        SetColliderAttackPosition();
        GetInputs();
        SetVelocity(gameTime);
        CheckHealth();
        ResetAttack(gameTime);
        ResetDash(gameTime);
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

    private void GetInputs()
    {
        var state = Keyboard.GetState();

        _movement = Vector2.Zero;
        if (state.IsKeyDown(Keys.W))
        {
            _movement.Y = -1;
        }
        if (state.IsKeyDown(Keys.S))
        {
            _movement.Y = 1;
        }
        if (state.IsKeyDown(Keys.A))
        {
            _movement.X = -1;
        }
        if (state.IsKeyDown(Keys.D))
        {
            _movement.X = 1;
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
        if (state.IsKeyDown(Keys.LeftShift))
        {
            if (_hasDashed == false)
            {
                _dashButtonPressed = true;
            }
        }
        if (state.IsKeyUp(Keys.LeftShift))
        {
            if (_dashButtonPressed && _hasDashed == false)
            {
                Dash();
                _dashButtonPressed = false;
            }
        }
    }

    private void SetVelocity(GameTime gameTime)
    {
        if (_movement == Vector2.Zero) return;

        var dir = -_movement;
        var v = dir * MoveSpeed;
        var norm = Vector2.Normalize(v);
        Velocity = Vector2.Subtract(Velocity, v);
    }

    private void Attack()
    {
        if (_hasAttackedFlag) return;

        _attackFlag = true;
        _hasAttackedFlag = true;
        _attackCooldownTimer = _attackCooldownMs;
        _attackDelayTimer = _attackColliderDelayMs;
    }

    private void Dash()
    {
        Velocity *= _dashAmount;
        _hasDashed = true;
        _dashCooldownTimer = _dashCoolDownMs;
    }

    private void ResetDash(GameTime gameTime)
    {
        if (_hasDashed == false) return;
        
        _dashCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_dashCooldownTimer <= 0)
        {
            _dashCooldownTimer = _dashCoolDownMs;
            _hasDashed = false;
        }
    }

    private void ResetAttack(GameTime gameTime)
    {
        if (_hasAttackedFlag == false) return;

        _attackCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;
        _attackDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_attackDelayTimer <= 0)
        {
            if (_attackCollider.Enabled == false)
            {
                _attackColliderFlag = true;
                _attackCollider.Enabled = true;
            }
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

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            if (_isDying == false)
            {
                DeathFlag = true;
            }
        }
    }

    public void ApplyDamage(double amount)
    {
        Health -= amount;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }
}
