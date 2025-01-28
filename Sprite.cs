using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Sprite
{
    protected readonly Texture2D _texture2D;

    public Vector2 Position;

    public int Width { get; set; }
    public int Height { get; set; }
    public double Scale { get; set; }
    public Color Color { get; set; }

    public bool Enabled { get; set; }

    public Sprite(Texture2D texture) : base()
    {
        _texture2D = texture;
        Position = Vector2.Zero;
        Width = _texture2D.Width;
        Height = _texture2D.Height;
        Scale = 1f;
        Color = Color.White;
        Enabled = true;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (Enabled == false) return;

        int scaledWidth = Convert.ToInt32(Width * Scale);
        int scaledHeight = Convert.ToInt32(Height * Scale);

        int x = Convert.ToInt32(Position.X);
        int y = Convert.ToInt32(Position.Y);

        spriteBatch.Draw(_texture2D,
                          new Rectangle(x - (scaledWidth / 2), y - (scaledHeight / 2), scaledWidth, scaledHeight),
                          Color);
    }
}
