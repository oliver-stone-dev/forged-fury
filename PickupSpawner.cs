using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class PickupSpawner : SpawnArea
{
    private readonly Texture2D _pickupTexture;

    public PickupSpawner(Texture2D pickupTexture)
    {
        _pickupTexture = pickupTexture;
    }

    public void SpawnPickup(PlayerController player)
    {
        var spawnPoint = GetRandomSpawnPoint(player.Position);

        var healthPack = new HealthPickup(_pickupTexture);
        healthPack.Position = spawnPoint;
        healthPack.Enable();
    }
}
