using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Environment : GameObject
{
    private readonly Sprite _background;
    private readonly Sprite _level;

    public Environment(Texture2D backgroundSprite, Texture2D levelSprite, GraphicsDeviceManager graphics) :base()
    {
        //Background sprite
        _background = new Sprite(backgroundSprite);
        _background.Position.X = graphics.PreferredBackBufferWidth / 2;
        _background.Position.Y = graphics.PreferredBackBufferHeight / 2;
        _background.Width = graphics.PreferredBackBufferWidth;
        _background.Height = graphics.PreferredBackBufferHeight;
        _background.Color = Color.Black;

        //Level Sprite
        _level = new Sprite(levelSprite);
        _level.Position.X = graphics.PreferredBackBufferWidth / 2;
        _level.Position.Y = graphics.PreferredBackBufferHeight / 2;
        _level.Scale = 2f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _background.Draw(spriteBatch);
        _level.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

}
