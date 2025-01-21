using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class AnimatedSprite : Sprite
{
    private int _currentFrame;
    private int _timestamp;

    public static readonly int DefaultFramePeriod = 1000;

    public int FrameWidth { get; set; }
    public int FrameHeight { get; set; }
    public int AnimationRow { get; set; }
    public int MaxFrame { get; set; }
    public int Period { get; set; }

    public AnimatedSprite(Texture2D texture) : base(texture)
    {
        _timestamp = 0;
        Period = DefaultFramePeriod;
    }

    public void Update(GameTime gameTime)
    {
        _timestamp += gameTime.ElapsedGameTime.Milliseconds;
        if (_timestamp >= Period)
        {
            _currentFrame++;
            _timestamp = 0;
        }

        if (_currentFrame >= MaxFrame) _currentFrame = 0;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int scaledWidth = Convert.ToInt32(Width * Scale);
        int scaledHeight = Convert.ToInt32(Height * Scale);

        spriteBatch.Draw(
            _texture2D,
            new Rectangle(X - (scaledWidth / 2), Y - (scaledHeight / 2), scaledWidth, scaledHeight),
            new Rectangle(FrameWidth * _currentFrame, FrameHeight * AnimationRow, FrameWidth, FrameHeight),
            Color,
            0f,
            Vector2.Zero,
            SpriteEffects.None,
            0f
            );
    }
}
