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
    private readonly Texture2D _spriteSheet;

    public ParticleEmitter(Texture2D spriteSheet)
    {
        _spriteSheet = spriteSheet;
    }

    public void Emit(Vector2 position)
    {
        var sparkParticles = new Sparks(_spriteSheet);
        sparkParticles.Position = position;
        sparkParticles.Start();
    }
}
