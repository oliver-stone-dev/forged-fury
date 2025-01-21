using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Sprite
{
    protected readonly Texture2D _texture2D;
    
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Scale { get; set; }
    public Color Color { get; set; }

    public Sprite(Texture2D texture)
    {
        _texture2D = texture;
        X = 0;
        Y = 0;
        Width = _texture2D.Width;
        Height = _texture2D.Height;
        Scale = 1f;
        Color = Color.White;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        int scaledWidth = Convert.ToInt32(Width * Scale);
        int scaledHeight = Convert.ToInt32(Height * Scale);

        spriteBatch.Draw(_texture2D,
                          new Rectangle(X - (scaledWidth / 2), Y - (scaledHeight / 2), scaledWidth, scaledHeight),
                          Color);
    }
}
