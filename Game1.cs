using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace forged_fury;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _windowBackground;
    private Texture2D _levelBackgroundSprite;
    private Texture2D _playerSpriteSheet;
    private Texture2D _enemyAdvancedSheet;
    private Texture2D _debugTexture;
    private const int _levelWidth = 448;
    private const int _levelHeight = 288;
    private const float _spriteScale = 2f;

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
        _enemyAdvancedSheet = Content.Load<Texture2D>("EnemyAdvancedSheet");

        _debugTexture = new Texture2D(GraphicsDevice, 1, 1);
        _debugTexture.SetData(new Color[] { Color.White });
        ColliderManager.DebugTextue = _debugTexture;
        ColliderManager.DrawDebugBoxes = true;

        //Environment
        var environment = new Environment(_windowBackground, _levelBackgroundSprite, _graphics);

        //Player Sprite
        var player = new PlayerController(_playerSpriteSheet);
        player.Position.X = _graphics.PreferredBackBufferWidth / 2;
        player.Position.Y = _graphics.PreferredBackBufferHeight / 2;
        player.Name = "Player";

        var enemy = new EnemyController(_enemyAdvancedSheet, player);
        enemy.Position.X = (_graphics.PreferredBackBufferWidth / 2) - 150;
        enemy.Position.Y = (_graphics.PreferredBackBufferHeight / 2) - 100;
        enemy.MoveSpeed = 30f;
        enemy.Name = "Enemy";

        var enemy2 = new EnemyController(_enemyAdvancedSheet, player);
        enemy2.Position.X = (_graphics.PreferredBackBufferWidth / 2) - 150;
        enemy2.Position.Y = (_graphics.PreferredBackBufferHeight / 2) + 0;
        enemy2.MoveSpeed = 30f;
        enemy2.Name = "Enemy2";

        var enemy3 = new EnemyController(_enemyAdvancedSheet, player);
        enemy3.Position.X = (_graphics.PreferredBackBufferWidth / 2) - 150;
        enemy3.Position.Y = (_graphics.PreferredBackBufferHeight / 2) + 100;
        enemy3.MoveSpeed = 30f;
        enemy3.Name = "Enemy3";
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        ColliderManager.Update(gameTime);
        GameObjectManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        GameObjectManager.Draw(_spriteBatch);
        ColliderManager.Draw(_spriteBatch);//draw collider debugs
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
