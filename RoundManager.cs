using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace forged_fury;

public class RoundManager : GameObject
{
    private readonly PlayerController _player;
    private readonly SpriteFont _font;
    private readonly EnemySpawner _enemySpawner;
    private readonly HealthPickupSpawner _pickupSpawner;

    private bool _roundStarted = false;
    private const int _waveLengthMs = 10000;
    private int _timeUntilNextWave = 0;

    private const int _healthPickupMs = 20000;
    private int _healthPickupTimer = 0;
    private const int _maxHealthPerRound = 5;
    private int _healthPickupCounter = 0;

    private const int _enemyStartHealth = 10;
    private const int _enemiesRemainingDefault = 5;

    private int _enemiesAlive = 0;

    private const float _spawnFactor = 0.25f;
    private const float _healthFactor = 0.05f;

    private const int _maxEnemiesPerWave = 10;

    private const float _rangedEnemyHealthFactor = 0.5f;

    public int CurrentRound { get; set; }

    public int EnemiesRemaining { get; set; }

    public bool RoundFinishedFlag { get; set; }

    public RoundManager(PlayerController player, SpriteFont font, EnemySpawner enemySpawner, HealthPickupSpawner pickupSpawner)
    {
        _font = font;
        _player = player;
        _enemySpawner = enemySpawner;
        _pickupSpawner = pickupSpawner;
        CurrentRound = 0;
    }

    public void NextRound()
    {
        CurrentRound += 1;
        EnemiesRemaining = GetEnemiesToKill(CurrentRound);
        _timeUntilNextWave = _waveLengthMs;
    }

    public void StartRound()
    {
        _roundStarted = true;
        RoundFinishedFlag = false;
        SpawnWaveOfEnemies(CurrentRound);
        _healthPickupTimer = _healthPickupMs;
        _healthPickupCounter = 0;
    }

    public override void Update(GameTime gameTime)
    {
        if (_roundStarted)
        {
            HandleEnemySpawning(gameTime);
            HandleItemSpawning(gameTime);

            if (EnemiesRemaining <= 0 && EnemiesAlive() == false)
            {
                EnemiesRemaining = _enemiesRemainingDefault + (int)(CurrentRound * 1.5f);
                RoundFinishedFlag = true;
                _roundStarted = false;
            }        
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_font, $"Round: {CurrentRound} ", new Vector2(200, 15), Color.White);
        spriteBatch.DrawString(_font, $"Enemies Remaining: {EnemiesRemaining}", new Vector2(720, 15), Color.White);
        spriteBatch.DrawString(_font, $"Health: {_player.Health} ", new Vector2(200, 660), Color.White);
        spriteBatch.DrawString(_font, $"Score: {_player.Score} ", new Vector2(950, 660), Color.White);
        base.Draw(spriteBatch);
    }

    private void HandleEnemySpawning(GameTime gameTime)
    {
        _timeUntilNextWave -= (gameTime.ElapsedGameTime.Milliseconds);

        if (_timeUntilNextWave <= 0)
        {
            SpawnWaveOfEnemies(CurrentRound);
            _timeUntilNextWave = _waveLengthMs;
        }

        if (EnemiesRemaining > 0 && EnemiesAlive() == false)
        {
            SpawnWaveOfEnemies(CurrentRound);
        }
    }
    
    private void HandleItemSpawning(GameTime gameTime)
    {
        if (_healthPickupCounter >= _maxHealthPerRound) return;
        if (HealthPickupAlive()) return;

        _healthPickupTimer -= (gameTime.ElapsedGameTime.Milliseconds);
        if (_healthPickupTimer <= 0)
        {
            SpawnHealthPickup();
            _healthPickupCounter++;
            _healthPickupTimer = _healthPickupMs;
        }
    }

    private void SpawnWaveOfEnemies(int round)
    {
        var rand = new Random();

        int max = CurrentRound + 1;
        if (max > _maxEnemiesPerWave) max = _maxEnemiesPerWave;

        int toSpawn = 0;

        toSpawn = rand.Next(1,max + 1);

        if (toSpawn > EnemiesRemaining)
        {
            toSpawn = EnemiesRemaining;
        }

        SpawnEnemies(toSpawn);
        EnemiesRemaining -= toSpawn;
    }

    private void SpawnEnemies(int count)
    {
        var chanceOfRanged = 0.25f;
        int rangedCount = 0;

        for (int i = 0; i < count; i++)
        {
            if (GetRandomBool(chanceOfRanged) && CurrentRound > 1 && rangedCount < 4) 
            {
                var health = Convert.ToInt32(GetEnemieHealth(CurrentRound) * _rangedEnemyHealthFactor);
                _enemySpawner.SpawnRangedEnemy(health, 10f, _player);
                rangedCount++;
            }
            else
            {
                _enemySpawner.SpawnAdvancedEnemy(GetEnemieHealth(CurrentRound), 20f, _player);
            }
        }
    }

    private int GetEnemiesToKill(int round)
    {
        return _enemiesRemainingDefault + Convert.ToInt32(Math.Round((round * round - 1) * _spawnFactor));
    }

    private int GetEnemieHealth(int round)
    {
        return _enemyStartHealth + Convert.ToInt32(Math.Round((round * round - 1) * _healthFactor));
    }

    private bool EnemiesAlive()
    {
        if (GameObjectManager.CountObjects("Enemy") > 0)
        {
            return true;
        }

        return false;
    }

    private bool HealthPickupAlive()
    {
        if (GameObjectManager.CountObjects("Health") > 0)
        {
            return true;
        }

        return false;
    }

    private void SpawnHealthPickup()
    {
        _pickupSpawner.SpawnPickup(_player);
    }

    private bool GetRandomBool(float probabilityTrue = 0.5f)
    {
        var rand = new Random();
        var randomFloat = (float)rand.NextDouble();

        if (randomFloat <= probabilityTrue) return true;

        return false;
    }
}
