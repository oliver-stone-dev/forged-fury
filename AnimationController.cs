﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace forged_fury;

// TO DO: This whole class needs to be reworked

public class AnimationController
{
    public enum AnimationStates
    {

        IdleRight,
        IdleLeft,
        RunRight,
        RunLeft,
        AttackRight,
        AttackLeft,
        AttackRightAlt,
        AttackLeftAlt,
        DeathRight,
        DeathLeft,
        Spawn
    }

    private readonly AnimatedSprite _animatedSprite;
    private AnimationStates _currentState;
    private AnimationStates _nextState;
    private bool _waitToFinish = false;

    public int AttackFrames { get; set; }

    public int AttackPeriod { get; set; }

    public AnimationController(AnimatedSprite animatedSprite)
    {
        _animatedSprite = animatedSprite;
        _currentState = AnimationStates.IdleRight;
        _nextState = AnimationStates.IdleRight;
        SetAnimatedSpriteAnimation(_currentState);
        _animatedSprite.Start();
    }

    public void Update(GameTime gameTime)
    {
        HandleAnimationState();
    }

    public void SetNextState(AnimationStates state)
    {
        _nextState = state;
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
                _animatedSprite.MaxFrame = AttackFrames;
                _animatedSprite.Period = AttackPeriod;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.AttackLeft):
                _animatedSprite.AnimationRow = 5;
                _animatedSprite.MaxFrame = AttackFrames;
                _animatedSprite.Period = AttackPeriod;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.AttackRightAlt):
                _animatedSprite.AnimationRow = 6;
                _animatedSprite.MaxFrame = 4;
                _animatedSprite.Period = 75;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.AttackLeftAlt):
                _animatedSprite.AnimationRow = 7;
                _animatedSprite.MaxFrame = 4;
                _animatedSprite.Period = 75;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.DeathRight):
                _animatedSprite.AnimationRow = 8;
                _animatedSprite.MaxFrame = 5;
                _animatedSprite.Period = 200;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.DeathLeft):
                _animatedSprite.AnimationRow = 9;
                _animatedSprite.MaxFrame = 5;
                _animatedSprite.Period = 200;
                _animatedSprite.Loop = false;
                _waitToFinish = true;
                _animatedSprite.StartFrame = 1;
                break;
            case (AnimationStates.Spawn):
                _animatedSprite.AnimationRow = 10;
                _animatedSprite.MaxFrame = 8;
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
        switch (_currentState)
        {
            case (AnimationStates.IdleRight):
                if (_nextState != _currentState)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.IdleLeft):
                if (_nextState != _currentState)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.RunRight):
                if (_nextState != _currentState)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.RunLeft):
                if (_nextState != _currentState)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.AttackRight):
                if (_nextState != _currentState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.AttackLeft):
                if (_nextState != _currentState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.AttackRightAlt):
                if (_nextState != _currentState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.AttackLeftAlt):
                if (_nextState != _currentState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.DeathRight):
                if (_nextState != _currentState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.DeathLeft):
                if (_nextState != _currentState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            case (AnimationStates.Spawn):
                if (_nextState != _currentState && _animatedSprite.IsRunning() == false)
                {
                    _animatedSprite.Stop();
                    _currentState = _nextState;
                    SetAnimatedSpriteAnimation(_currentState);
                    _animatedSprite.Start();
                }
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        _animatedSprite.Stop();
    }

    public bool IsRunning()
    {
        return _animatedSprite.IsRunning();
    }
}
