using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Collider : IDisposable
{
    public Vector2 Position { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Action<Collider> OnCollisionAction;

    public GameObject Parent { get; set; }

    public Collider(GameObject parent)
    {
        Parent = parent;
        ColliderManager.Add(this);
    }

    public void OnCollision(Collider collider)
    {
       OnCollisionAction?.Invoke(collider);
    }

    public void Dispose()
    {
        ColliderManager.Remove(this);
    }
}
