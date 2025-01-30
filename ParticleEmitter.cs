using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class ParticleEmitter
{
    private readonly Texture2D _particleSpriteSheet;

    public ParticleEmitter()
    {
        var particleAsset = AssetManager.Textures.Get("Sparks");
        var particleSprite = particleAsset!.AssetObject;
        if (particleSprite == null) return;

        _particleSpriteSheet = particleSprite;
    }

    public void Emit(Vector2 position)
    {
        if (_particleSpriteSheet == null) return;

        var sparkParticles = new Sparks(_particleSpriteSheet);
        sparkParticles.Position = position;
        sparkParticles.Start();
    }
}
