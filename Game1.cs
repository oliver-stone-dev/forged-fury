using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace forged_fury;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _windowBackground;
    private Texture2D _levelBackgroundSprite;
    private Texture2D _playerSpriteSheet;
    private const int _levelWidth = 448;
    private const int _levelHeight = 288;
    private const float _spriteScale = 2f;

    private List<Sprite> _spritesToDraw = new();
    private List<Character> _playersToDrawer = new();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _levelBackgroundSprite = Content.Load<Texture2D>("Level");
        _windowBackground = new Texture2D(GraphicsDevice, 1, 1);
        _windowBackground.SetData(new Color[] { Color.White });
        _playerSpriteSheet = Content.Load<Texture2D>("PlayerSheet");

        //Background sprite
        var background = new Sprite(_windowBackground);
        background.Position.X = _graphics.PreferredBackBufferWidth / 2;
        background.Position.Y = _graphics.PreferredBackBufferHeight / 2;
        background.Width = _graphics.PreferredBackBufferWidth;
        background.Height = _graphics.PreferredBackBufferHeight;
        background.Color = Color.Black;

        //Level Sprite
        var level = new Sprite(_levelBackgroundSprite);
        level.Position.X = _graphics.PreferredBackBufferWidth / 2;
        level.Position.Y = _graphics.PreferredBackBufferHeight / 2;
        level.Scale = _spriteScale;

        _spritesToDraw.Add(background);
        _spritesToDraw.Add(level);

        //Player Sprite
        var player = new PlayerController(_playerSpriteSheet);
        player.Position.X = _graphics.PreferredBackBufferWidth / 2;
        player.Position.Y = _graphics.PreferredBackBufferHeight / 2;

        _playersToDrawer.Add(player);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _playersToDrawer.ForEach(s => s.Update(gameTime));

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spritesToDraw.ForEach(s => s.Draw(_spriteBatch));
        _playersToDrawer.ForEach(p => p.Draw(_spriteBatch));
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
