using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class EnemySpawner
{
    private readonly Texture2D _spriteSheet;
    private readonly PlayerController _player;
    private readonly GraphicsDeviceManager _graphics;

    private Rectangle _spawnAreaLeft;
    private Rectangle _spawnAreaRight;

    private int _spawnAreaWidth = 150;
    private int _spawnAreaHeight = 300;

    public EnemySpawner (Texture2D spriteSheet, PlayerController player)
    {
        _spriteSheet = spriteSheet;
        _player = player;

        _spawnAreaLeft = new Rectangle(400 - (_spawnAreaWidth / 2), 333 - (_spawnAreaHeight / 2), _spawnAreaWidth, _spawnAreaHeight);
        _spawnAreaRight = new Rectangle(885 - (_spawnAreaWidth / 2), 333 - (_spawnAreaHeight / 2), _spawnAreaWidth, _spawnAreaHeight);
    }

    public void SpawnEnemy()
    {
        var spawnPoint = GetRandomSpawnPoint();

        var enemy = new EnemyController(_spriteSheet, _player);
        enemy.Position.X = spawnPoint.X;
        enemy.Position.Y = spawnPoint.Y;
        enemy.MoveSpeed = 30f;
        enemy.Name = "Enemy";
        enemy.Health = 20;
    }

    private bool PointInsideArea(Vector2 point, Rectangle area)
    {
        if (area.Contains(point))
        {
            return true;
        }

        return false;
    }

    private Vector2 GetRandomPointWithinArea(Rectangle area)
    {
        var rand = new Random();
        var point = Vector2.Zero;

        point.X = rand.Next(area.Left, area.Right);
        point.Y = rand.Next(area.Top, area.Bottom);

        return point;
    }

    private Vector2 GetRandomSpawnPoint()
    {
        var spawnPoint = Vector2.Zero;
        var rand = new Random();

        var playerLeft = PointInsideArea(_player.Position, _spawnAreaLeft);
        var playerRight = PointInsideArea(_player.Position, _spawnAreaRight);

        if (playerLeft)
        {
            spawnPoint = GetRandomPointWithinArea(_spawnAreaRight);
        }
        else if (playerRight)
        {
            spawnPoint = GetRandomPointWithinArea(_spawnAreaLeft);
        }
        else
        {
            var randomSide = rand.Next(0, 2);
            if (randomSide == 0)
            {
                spawnPoint = GetRandomPointWithinArea(_spawnAreaRight);
            }
            else
            {
                spawnPoint = GetRandomPointWithinArea(_spawnAreaLeft);
            }
        }

        return spawnPoint;
    }
}
