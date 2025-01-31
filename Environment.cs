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
    GraphicsDeviceManager _graphics;

    private readonly Sprite _background;
    private readonly Sprite _level;

    private Collider _topWallCollider;
    private Collider _bottomWallCollider;
    private Collider _leftWallCollider;
    private Collider _rightWallCollider;

    public Environment(GraphicsDeviceManager graphics) :base()
    {
        _graphics = graphics;

        //Background sprite
        var backgroundAsset = AssetManager.Textures.Get("WindowBackground");
        var backgroundSprite = backgroundAsset!.AssetObject;
        if (backgroundSprite == null) return;

        _background = new Sprite(backgroundSprite);
        _background.Position.X = graphics.PreferredBackBufferWidth / 2;
        _background.Position.Y = graphics.PreferredBackBufferHeight / 2;
        _background.Width = graphics.PreferredBackBufferWidth;
        _background.Height = graphics.PreferredBackBufferHeight;
        _background.Color = Color.Black;

        //Level Sprite
        var levelAsset = AssetManager.Textures.Get("Level");
        var levelSprite = levelAsset!.AssetObject;
        if (levelSprite == null) return;

        _level = new Sprite(levelSprite);
        _level.Position.X = graphics.PreferredBackBufferWidth / 2;
        _level.Position.Y = graphics.PreferredBackBufferHeight / 2;
        _level.Scale = 1f;

        //Load wall colliders
        _topWallCollider = new Collider(this);
        _bottomWallCollider = new Collider(this);
        _leftWallCollider = new Collider(this);
        _rightWallCollider = new Collider(this);

        _topWallCollider.Width = _level.Width;
        _topWallCollider.Height = 16;
        _topWallCollider.Position.X = _level.Position.X;
        _topWallCollider.Position.Y = graphics.PreferredBackBufferHeight / 2 - (_level.Height / 2) + (_topWallCollider.Height / 2);

        _bottomWallCollider.Width = _level.Width;
        _bottomWallCollider.Height = 16;
        _bottomWallCollider.Position.X = _level.Position.X;
        _bottomWallCollider.Position.Y = graphics.PreferredBackBufferHeight / 2 + (_level.Height / 2) - ((_topWallCollider.Height) * 1.75f);

        _leftWallCollider.Width = 16;
        _leftWallCollider.Height = _level.Height;
        _leftWallCollider.Position.X = graphics.PreferredBackBufferWidth / 2 - (_level.Width / 2) + (_leftWallCollider.Width / 2);
        _leftWallCollider.Position.Y = _level.Position.Y;

        _rightWallCollider.Width = 16;
        _rightWallCollider.Height = _level.Height;
        _rightWallCollider.Position.X = graphics.PreferredBackBufferWidth / 2 + (_level.Width / 2) - (_rightWallCollider.Width / 2);
        _rightWallCollider.Position.Y = _level.Position.Y;

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

    public Rectangle GetPlayableArea()
    {
        var x = Convert.ToInt32(_graphics.PreferredBackBufferWidth / 2 - (_level.Width / 2) + 32);
        var y = Convert.ToInt32(_graphics.PreferredBackBufferHeight / 2 - (_level.Height / 2) + 48);

        var area = new Rectangle(x, y, _level.Width - 64, _level.Height - 128);

        return area;
    }

    public Vector2 GetLevelPosition()
    {
        return _level.Position;
    }

    public Vector2 GetLevelSize()
    {
        var size = new Vector2(_level.Width, _level.Height);
        return size;
    }

    public Vector2 GetWindowSize()
    {
        var size = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        return size;
    }
}
