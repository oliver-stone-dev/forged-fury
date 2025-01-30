using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace forged_fury;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _windowBackground;
    private Texture2D _levelBackgroundSprite;
    private Texture2D _playerSpriteSheet;
    private Texture2D _enemyAdvancedSheet;
    private Texture2D _enemyRangedSheet;
    private Texture2D _debugTexture;
    private Texture2D _shadowTexture;
    private Texture2D _healthTexture;
    private Texture2D _sparks;
    private Texture2D _startButton;
    private Texture2D _endButton;
    private Texture2D _title;
    private SoundEffect _sword1effect;
    private SoundEffect _sword2effect;
    private SoundEffect _sword3effect;
    private SoundEffect _sword4effect;
    private SoundEffect _sword5effect;
    private SoundEffect _sword6effect;
    private Song _menuMusic;
    private Song _gameMusic;
    private const int _levelWidth = 448;
    private const int _levelHeight = 288;
    private const float _spriteScale = 2f;
    private SpriteFont font;

    private MainMenu _mainMenu;
    private ScoreScreen _scoreScreen;
    private GameManager _gameManager;
    private RoundManager _roundManager;
    private EnemySpawner _enemySpawner;
    private HealthPickupSpawner _pickupSpawner;
    private ParticleEmitter _particleEmitter;
    private SoundPlayer _soundPlayer;
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
        _windowBackground = new Texture2D(GraphicsDevice, 1, 1);
        _debugTexture = new Texture2D(GraphicsDevice, 1, 1);
        _levelBackgroundSprite = Content.Load<Texture2D>("Level");
        _playerSpriteSheet = Content.Load<Texture2D>("PlayerSheet");
        _enemyAdvancedSheet = Content.Load<Texture2D>("EnemyAdvancedSheet");
        _enemyRangedSheet = Content.Load<Texture2D>("EnemyRangedSheet");
        _shadowTexture = Content.Load<Texture2D>("Shadow");
        _healthTexture = Content.Load<Texture2D>("Health");
        _sparks = Content.Load<Texture2D>("Sparks");
        _startButton = Content.Load<Texture2D>("StartButton");
        _endButton = Content.Load<Texture2D>("ExitButton");
        _title = Content.Load<Texture2D>("Title");
        font = Content.Load<SpriteFont>("FontMain");
        _menuMusic = Content.Load<Song>("menuTheme");
        _gameMusic = Content.Load<Song>("gameTheme");
        _sword1effect = Content.Load<SoundEffect>("Sword1");
        _sword2effect = Content.Load<SoundEffect>("Sword2");
        _sword3effect = Content.Load<SoundEffect>("Sword3");
        _sword4effect = Content.Load<SoundEffect>("Sword4");
        _sword5effect = Content.Load<SoundEffect>("Sword5");
        _sword6effect = Content.Load<SoundEffect>("Sword6");

        _windowBackground.SetData(new Color[] { Color.White });
        _debugTexture.SetData(new Color[] { Color.White });

        //Add content to asset manager
        AssetManager.Textures.Add(_levelBackgroundSprite, _levelBackgroundSprite.Name);
        AssetManager.Textures.Add(_windowBackground, "WindowBackground");
        AssetManager.Textures.Add(_playerSpriteSheet, _playerSpriteSheet.Name);
        AssetManager.Textures.Add(_enemyAdvancedSheet, _enemyAdvancedSheet.Name);
        AssetManager.Textures.Add(_enemyRangedSheet, _enemyRangedSheet.Name);
        AssetManager.Textures.Add(_shadowTexture, _shadowTexture.Name);
        AssetManager.Textures.Add(_healthTexture, _healthTexture.Name);
        AssetManager.Textures.Add(_sparks, _sparks.Name);
        AssetManager.Textures.Add(_startButton, _startButton.Name);
        AssetManager.Textures.Add(_endButton, _endButton.Name);
        AssetManager.Textures.Add(_title, _title.Name);
        AssetManager.Textures.Add(_debugTexture, "DebugTexture");
        AssetManager.SoundEffects.Add(_sword1effect, _sword1effect.Name);
        AssetManager.SoundEffects.Add(_sword2effect, _sword2effect.Name);
        AssetManager.SoundEffects.Add(_sword3effect, _sword3effect.Name);
        AssetManager.SoundEffects.Add(_sword4effect, _sword4effect.Name);
        AssetManager.SoundEffects.Add(_sword5effect, _sword5effect.Name);
        AssetManager.SoundEffects.Add(_sword6effect, _sword6effect.Name);

        ColliderManager.DebugTextue = _debugTexture;
        ColliderManager.DrawDebugBoxes = false;

        //Load Main Screen
        _mainMenu = new MainMenu(_graphics);
        MediaPlayer.Volume = 0.0f;
        MediaPlayer.Play(_menuMusic);
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
                MediaPlayer.Volume = 0.0f;
                MediaPlayer.Play(_gameMusic);
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
                _scoreScreen = new ScoreScreen(_graphics, font);
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
        var environment = new Environment(_graphics, font);
        _particleEmitter = new ParticleEmitter();
        _soundPlayer = new SoundPlayer();

        _soundPlayer.Volume = 0.1f;

        _soundPlayer.AddSound(_sword1effect);
        _soundPlayer.AddSound(_sword2effect);
        _soundPlayer.AddSound(_sword3effect);
        _soundPlayer.AddSound(_sword4effect);
        _soundPlayer.AddSound(_sword5effect);
        _soundPlayer.AddSound(_sword6effect);

        _player = new PlayerController(_particleEmitter, _soundPlayer);
        _player.Position = new Vector2((_graphics.PreferredBackBufferWidth / 2), _graphics.PreferredBackBufferHeight / 2);
        _player.Name = "Player";
        _player.Health = 100;

        _gameManager = new GameManager(_player);
        _enemySpawner = new EnemySpawner(_particleEmitter,_soundPlayer);
        _pickupSpawner = new HealthPickupSpawner();
        _roundManager = new RoundManager(_player, font, _enemySpawner, _pickupSpawner);
    }
}
