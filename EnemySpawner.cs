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

    public void SpawnEnemy(int health, float moveSpeed, PlayerController player)
    {
        var spawnPoint = GetRandomSpawnPoint(player.Position);

        var enemy = new EnemyController(player, _particleEmitter,_soundPlayer);
        enemy.Position = spawnPoint;
        enemy.MoveSpeed = moveSpeed;
        enemy.Name = "Enemy";
        enemy.Health = health;
    }
}
