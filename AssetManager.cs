using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public static class AssetManager
{
    public static AssetContainer<Texture2D> Textures { get; } = new();
    public static AssetContainer<SoundEffect> SoundEffects { get; } = new();
}
