using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Asset<T>
{
    public T AssetObject { get; init; }
    public string Name { get; init; }

    public Asset(T assetObject, string name)
    {
        AssetObject = assetObject;
        Name = name;
    }
}
