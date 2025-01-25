using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class EnemySpawner
{
    private readonly Texture2D _spriteSheet;
    private readonly PlayerController _player;
    private readonly GraphicsDeviceManager _graphics;

    public EnemySpawner (Texture2D spriteSheet, PlayerController player)
    {
        _spriteSheet = spriteSheet;
        _player = player;
    }

    public void SpawnEnemy(int x, int y)
    {
        var enemy = new EnemyController(_spriteSheet, _player);
        enemy.Position.X = x;
        enemy.Position.Y = y;
        enemy.MoveSpeed = 30f;
        enemy.Name = "Enemy";
        enemy.Health = 20;
    }
}
