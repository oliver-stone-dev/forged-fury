using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

    private readonly int _roundLengthMs = 60000;
    private readonly int _waveLengthMs = 10000;

    private bool _roundStarted = false;

    private int _timeUntilNextWave = 0;

    public int CurrentRound { get; set; }

    public int TimeRemainingMs { get; set; }

    public bool RoundFinishedFlag { get; set; }

    public RoundManager(PlayerController player, SpriteFont font, EnemySpawner spawner)
    {
        _font = font;
        _player = player;
        _spawner = spawner;
        CurrentRound = 0;
        TimeRemainingMs = _roundLengthMs;
    }

    public void NextRound()
    {
        CurrentRound += 1;
        TimeRemainingMs = _roundLengthMs;
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
            TimeRemainingMs -= (gameTime.ElapsedGameTime.Milliseconds);
            _timeUntilNextWave -= (gameTime.ElapsedGameTime.Milliseconds);

            if (TimeRemainingMs <= 0)
            {
                _roundStarted = false;
                RoundFinishedFlag = true;
            }

            if (_timeUntilNextWave <= 0)
            {
                SpawnWaveOfEnemies(CurrentRound);
                _timeUntilNextWave = _waveLengthMs;
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_font, "Round: ", new Vector2(200, 15), Color.White);
        spriteBatch.DrawString(_font, $"{CurrentRound}", new Vector2(310, 15), Color.White);
        spriteBatch.DrawString(_font, "Time Remaining: ", new Vector2(370, 15), Color.White);
        spriteBatch.DrawString(_font, $"{TimeRemainingMs / 1000}", new Vector2(630, 15), Color.White);
        spriteBatch.DrawString(_font, "Player Health: ", new Vector2(600, 15), Color.White);
        spriteBatch.DrawString(_font, $"{_player.Health}", new Vector2(800, 15), Color.White);
        spriteBatch.DrawString(_font, "Player Score: ", new Vector2(900, 15), Color.White);
        spriteBatch.DrawString(_font, $"{_player.Score}", new Vector2(1100, 15), Color.White);
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
        SpawnEnemies(toSpawn); 
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _spawner.SpawnEnemy();
        }
    }
}
