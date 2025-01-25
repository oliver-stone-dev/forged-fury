using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class GameObject : IDisposable
{
    public string? Name { get; set; }

    public GameObject()
    {
        GameObjectManager.Add(this);
    }

    public virtual void Update(GameTime gameTime)
    {

    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {

    }

    public void Destroy()
    {
        GameObjectManager.Remove(this);
        ColliderManager.RemoveParentColliders(this);
    }

    public void Dispose()
    {
        GameObjectManager.Remove(this);
    }
}
