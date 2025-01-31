using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace forged_fury;

public class Collider : IDisposable
{
    public Vector2 Position;
    public int Width { get; set; }
    public int Height { get; set; }

    public Action<Collider> OnCollisionAction;
    public GameObject Parent { get; set; }

    public Sprite DebugSprite;

    public string? Name { get; set; }

    public string? IgnoreName { get; set; }

    public bool Enabled { get; set; }

    public bool EnablePhysicsCollions { get; set; }

    public bool TopCollision { get; set; }
    public bool BottomCollision { get; set; }
    public bool LeftCollision { get; set; }
    public bool RightCollision { get; set; }

    public Collider(GameObject parent)
    {
        Position = Vector2.Zero;
        Name = "collider";
        Enabled = false;
        Parent = parent;
        ColliderManager.Add(this);
        EnablePhysicsCollions = true;
        IgnoreName = String.Empty;
    }

    public void Update()
    {
        DebugSprite.Position = Position;
        DebugSprite.Width = Width;
        DebugSprite.Height = Height;
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
