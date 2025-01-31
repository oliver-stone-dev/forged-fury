using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class HealthPickupSpawner
{
    private readonly Texture2D _pickupTexture;
    private readonly SpawnArea _spawnArea;

    public HealthPickupSpawner(Environment environment)
    {
        var healthAsset = AssetManager.Textures.Get("Health");
        var healthSprite = healthAsset!.AssetObject;
        if (healthSprite == null) return;

        _pickupTexture = healthSprite;

        _spawnArea = new(environment.GetPlayableArea());
    }

    public void SpawnPickup(PlayerController player)
    {
        if (_pickupTexture == null) return;

        var spawnPoint = _spawnArea.GetRandomSpawnPoint(player.Position);

        var healthPack = new HealthPickup(_pickupTexture);
        healthPack.Position = spawnPoint;
        healthPack.Enable();
    }
}
