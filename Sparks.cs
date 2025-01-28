using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Sparks : GameObject
{
    private readonly AnimatedSprite _animatedSprite;
    private bool _hasStarted = false;

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            if (_animatedSprite != null) _animatedSprite.Position = value;
        }
    }

    public Sparks(Texture2D spriteSheet)
    {
        _position = Vector2.Zero;

        _animatedSprite = new AnimatedSprite(spriteSheet);
        _animatedSprite.Loop = false;
        _animatedSprite.StartFrame = 0;
        _animatedSprite.MaxFrame = 4;
        _animatedSprite.AnimationRow = 0;
        _animatedSprite.Period = 50;
        _animatedSprite.FrameHeight = 32;
        _animatedSprite.FrameWidth = 16;
        _animatedSprite.Height = 32;
        _animatedSprite.Width = 16;
        _animatedSprite.Scale = 3f;

        _animatedSprite.Position = _position;
    }

    public void Start()
    {
        var rand = new Random();
        var row = rand.Next(0, 2);
        _animatedSprite.AnimationRow = row;

        _animatedSprite.Start();
        _hasStarted = true;
    }

    public override void Update(GameTime gameTime)
    {
        HandleParticles();
        _animatedSprite.Update(gameTime);
        base.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        _animatedSprite.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    private void HandleParticles()
    {
        if (_hasStarted)
        {
            if (_animatedSprite.IsRunning() == false)
            {
                Destroy();
            }
        }
    }
}
