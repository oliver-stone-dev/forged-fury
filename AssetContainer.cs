using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;
public class AssetContainer<T>
{
    private List<Asset<T>> _assets;

    public AssetContainer()
    {
        _assets = new();
    }

    public void Add(T asset, string name)
    {
        var newAsset = new Asset<T>(asset, name);
        _assets.Add(newAsset);
    }

    public Asset<T> Get(string name)
    {
        return _assets.Where(a => a.Name == name).FirstOrDefault();
    }
}
