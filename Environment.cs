using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace forged_fury;

public class Environment : GameObject
{
    private readonly SpriteFont _font;
    private readonly Sprite _background;
    private readonly Sprite _level;

    private Collider _topWallCollider;
    private Collider _bottomWallCollider;
    private Collider _leftWallCollider;
    private Collider _rightWallCollider;

    public Environment(Texture2D backgroundSprite, Texture2D levelSprite, GraphicsDeviceManager graphics, SpriteFont font) :base()
    {
        _font = font;

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

        //Load wall colliders
        _topWallCollider = new Collider(this);
        _bottomWallCollider = new Collider(this);
        _leftWallCollider = new Collider(this);
        _rightWallCollider = new Collider(this);

        _topWallCollider.Width = 500 * 2;
        _topWallCollider.Height = 32;
        _topWallCollider.Position.X = graphics.PreferredBackBufferWidth / 2;
        _topWallCollider.Position.Y = (graphics.PreferredBackBufferHeight / 2) - 270;

        _bottomWallCollider.Width = 500 * 2;
        _bottomWallCollider.Height = 32;
        _bottomWallCollider.Position.X = graphics.PreferredBackBufferWidth / 2;
        _bottomWallCollider.Position.Y = (graphics.PreferredBackBufferHeight / 2) + 240;

        _leftWallCollider.Width = 32;
        _leftWallCollider.Height = 400 * 2;
        _leftWallCollider.Position.X = (graphics.PreferredBackBufferWidth / 2) - 435;
        _leftWallCollider.Position.Y = graphics.PreferredBackBufferHeight / 2;

        _rightWallCollider.Width = 32;
        _rightWallCollider.Height = 400 * 2;
        _rightWallCollider.Position.X = (graphics.PreferredBackBufferWidth / 2) + 435;
        _rightWallCollider.Position.Y = graphics.PreferredBackBufferHeight / 2;

        _topWallCollider.Enabled = true;
        _bottomWallCollider.Enabled = true;
        _leftWallCollider.Enabled = true;
        _rightWallCollider.Enabled = true;
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
