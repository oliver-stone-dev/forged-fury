using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class EnemySpawner : SpawnArea
{
    private readonly ParticleEmitter _particleEmitter;
    private readonly SoundPlayer _soundPlayer;

    public EnemySpawner (ParticleEmitter particleEmitter, SoundPlayer soundPlayer)
    {
        _particleEmitter = particleEmitter;
        _soundPlayer = soundPlayer;
    }

    public void SpawnAdvancedEnemy(int health, float moveSpeed, PlayerController player)
    {
        var spawnPoint = GetRandomSpawnPoint(player.Position);

        var enemy = new EnemyAdvancedController(player, _particleEmitter,_soundPlayer);
        enemy.Position = spawnPoint;
        enemy.MoveSpeed = moveSpeed;
        enemy.Name = "Enemy";
        enemy.Health = health;
    }

    public void SpawnRangedEnemy(int health, float moveSpeed, PlayerController player)
    {
        var spawnPoint = GetRandomSpawnPoint(player.Position);

        var enemy = new EnemyRangedController(player, _particleEmitter, _soundPlayer);
        enemy.Position = spawnPoint;
        enemy.MoveSpeed = moveSpeed;
        enemy.Name = "Enemy";
        enemy.Health = health;
    }
}
