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
    private readonly Texture2D _spriteSheet;
    private readonly Texture2D _shadowTexture;
    private readonly ParticleEmitter _particleEmitter;
    private readonly SoundPlayer _soundPlayer;

    public EnemySpawner (Texture2D spriteSheet,Texture2D shadow, ParticleEmitter particleEmitter, SoundPlayer soundPlayer)
    {
        _particleEmitter = particleEmitter;
        _soundPlayer = soundPlayer;
        _spriteSheet = spriteSheet;
        _shadowTexture = shadow;
    }

    public void SpawnEnemy(int health, float moveSpeed, PlayerController player)
    {
        var spawnPoint = GetRandomSpawnPoint(player.Position);

        var enemy = new EnemyController(_spriteSheet, _shadowTexture, player, _particleEmitter,_soundPlayer);
        enemy.Position = spawnPoint;
        enemy.MoveSpeed = moveSpeed;
        enemy.Name = "Enemy";
        enemy.Health = health;
    }
}
