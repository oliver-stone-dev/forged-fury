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

    public Action OnCollisionEnterAction;

    public Action OnCollisionExitAction;

    public bool HasCollided { get; set; }

    private bool CollisionEnterFlag = false;

    public Collider()
    {
        HasCollided = false;
        ColliderManager.Add(this);
    }

    public void Update()
    {
        if (HasCollided)
        {
            if (CollisionEnterFlag == false)
            {
                CollisionEnterFlag = true;
                OnCollisionEnter();
            }
        }
        else
        {
            if (CollisionEnterFlag)
            {
                CollisionEnterFlag = false;
                OnCollisionExit();
            }
        }
    }

    private void OnCollisionEnter()
    {
       OnCollisionEnterAction?.Invoke();
    }

    private void OnCollisionExit()
    {
        OnCollisionExitAction?.Invoke();
    }

    public void Dispose()
    {
        ColliderManager.Remove(this);
    }

}
