﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace forged_fury;

public class RoundManager : GameObject
{
    private readonly PlayerController _player;
    private readonly SpriteFont _font;
    private readonly EnemySpawner _spawner;

    private bool _roundStarted = false;
    private const int _waveLengthMs = 10000;
    private int _timeUntilNextWave = 0;

    private const int _enemyStartHealth = 10;
    private const int _enemiesRemainingDefault = 10;

    private int _enemiesAlive = 0;

    private const float _spawnFactor = 0.25f;
    private const float _healthFactor = 0.05f;

    public int CurrentRound { get; set; }

    public int EnemiesRemaining { get; set; }

    public bool RoundFinishedFlag { get; set; }

    public RoundManager(PlayerController player, SpriteFont font, EnemySpawner spawner)
    {
        _font = font;
        _player = player;
        _spawner = spawner;
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
    }

    public override void Update(GameTime gameTime)
    {
        if (_roundStarted)
        {
            _timeUntilNextWave -= (gameTime.ElapsedGameTime.Milliseconds);

            if (_timeUntilNextWave <= 0)
            {
                SpawnWaveOfEnemies(CurrentRound);
                _timeUntilNextWave = _waveLengthMs;
            }

            if (EnemiesRemaining <= 0 && EnemiesAlive() == false)
            {
                EnemiesRemaining = _enemiesRemainingDefault + (int)(CurrentRound * 1.5f);
                RoundFinishedFlag = true;
            }
            else if (EnemiesRemaining > 0 && EnemiesAlive() == false)
            {
                SpawnWaveOfEnemies(CurrentRound);
            }
        
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_font, $"Round: {CurrentRound} ", new Vector2(170, 15), Color.White);
        spriteBatch.DrawString(_font, $"Enemies Remaining: {EnemiesRemaining}", new Vector2(360, 15), Color.White);
        spriteBatch.DrawString(_font, $"Health: {_player.Health} ", new Vector2(700, 15), Color.White);
        spriteBatch.DrawString(_font, $"Score: {_player.Score} ", new Vector2(960, 15), Color.White);
        base.Draw(spriteBatch);
    }

    private void SpawnWaveOfEnemies(int round)
    {
        int max = 1;
        int toSpawn = 0;

        if (round <= 5)
        {
            max = 2;
        }
        else if (round <= 10)
        {
            max = 3;
        }
        else if (round <= 20)
        {
            max = 4;
        }
        else if (round <= 30)
        {
            max = 5;
        }
        else
        {
            max = 6;
        }

        var rand = new Random();
        toSpawn = rand.Next(1,max + 1);

        if (toSpawn > EnemiesRemaining)
        {
            toSpawn = EnemiesRemaining;
        }

        EnemiesRemaining -= toSpawn;

        SpawnEnemies(toSpawn); 
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _spawner.SpawnEnemy(GetEnemieHealth(CurrentRound), 20f);
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
}
