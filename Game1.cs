﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

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
    private Texture2D _shadowTexture;
    private Texture2D _healthTexture;
    private Texture2D _sparks;
    private const int _levelWidth = 448;
    private const int _levelHeight = 288;
    private const float _spriteScale = 2f;
    private SpriteFont font;

    private MainMenu _mainMenu;
    private ScoreScreen _scoreScreen;
    private GameManager _gameManager;
    private RoundManager _roundManager;
    private EnemySpawner _enemySpawner;
    private PickupSpawner _pickupSpawner;
    private ParticleEmitter _particleEmitter;
    private PlayerController _player;

    private GameStates _gameState = GameStates.Menu;

    private enum GameStates
    {
        Menu,
        Start,
        LoadNextRound,
        PlayRound,
        End,
        Score
    }

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
        _shadowTexture = Content.Load<Texture2D>("Shadow");
        _healthTexture = Content.Load<Texture2D>("Health");
        _sparks = Content.Load<Texture2D>("Sparks");

        _debugTexture = new Texture2D(GraphicsDevice, 1, 1);
        _debugTexture.SetData(new Color[] { Color.White });
        ColliderManager.DebugTextue = _debugTexture;
        ColliderManager.DrawDebugBoxes = false;

        font = Content.Load<SpriteFont>("FontMain");

        //Load Main Screen
        _mainMenu = new MainMenu(_windowBackground, _graphics, font);
    }


    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (_gameState)
        {
            case (GameStates.Menu):
                if (Menu())   _gameState = GameStates.Start;
                break;
            case (GameStates.Start):
                InitGame();
                _gameState = GameStates.LoadNextRound;
                break;
            case (GameStates.LoadNextRound):
                _roundManager.NextRound();
                _roundManager.StartRound();
                _gameState = GameStates.PlayRound;
                break;
            case (GameStates.PlayRound):
                if (_roundManager.RoundFinishedFlag) 
                    _gameState = GameStates.LoadNextRound;
                if (_gameManager.GameEndFlag) 
                    _gameState = GameStates.End;
                break;
            case (GameStates.End):
                GameObjectManager.Clear();
                _scoreScreen = new ScoreScreen(_windowBackground, _graphics, font);
                _scoreScreen.Rounds = _roundManager.CurrentRound;
                _scoreScreen.Score = _player.Score;
                _gameState = GameStates.Score;
                break;
            case (GameStates.Score):
                if (_scoreScreen.EndFlag)
                {
                    Exit();
                }
                break;
        }

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

    private bool Menu()
    {
        if (_mainMenu.StartFlag)
        {
            _mainMenu.StartFlag = false;
            _mainMenu.Destroy();
            return true;
        }
        else if (_mainMenu.EndFlag)
        {
            _mainMenu.EndFlag = false;
            Exit();
        }

        return false;
    }
    
    private void InitGame()
    {
        var environment = new Environment(_windowBackground, _levelBackgroundSprite, _graphics, font);
        _particleEmitter = new ParticleEmitter(_sparks);

        _player = new PlayerController(_playerSpriteSheet, _shadowTexture,_particleEmitter);
        _player.Position = new Vector2((_graphics.PreferredBackBufferWidth / 2), _graphics.PreferredBackBufferHeight / 2);
        _player.Name = "Player";
        _player.Health = 100;

        _gameManager = new GameManager(_player);
        _enemySpawner = new EnemySpawner(_enemyAdvancedSheet,_shadowTexture);
        _pickupSpawner = new PickupSpawner(_healthTexture);
        _roundManager = new RoundManager(_player, font, _enemySpawner, _pickupSpawner);
    }
}
