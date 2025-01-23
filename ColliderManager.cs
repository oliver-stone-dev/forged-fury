using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;


public static class ColliderManager
{
    private static List<Collider> _colliders = new();
    public static IEnumerable<Collider> Colliders => _colliders;

    public static Vector3 CollisionArea { get; set; }

    public static Texture2D DebugTextue;

    public static void Update(GameTime gameTime)
    {
        CheckForCollisions();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach(var collider in _colliders)
        {
            collider.DebugSprite.Draw(spriteBatch);
        }
    }

    public static void Add(Collider collider)
    {
        if (DebugTextue != null)
        {
            collider.DebugSprite = new Sprite(DebugTextue);
            collider.DebugSprite.Color = new Color(Color.Green, 0.55f);
        }

        _colliders.Add(collider);
    }

    public static void Remove(Collider collider)
    {
        _colliders.Remove(collider);
    }

    public static void RemoveParentColliders(GameObject gameObject)
    {
        _colliders.RemoveAll(c => c.Parent == gameObject);
    }

    private static void CheckForCollisions()
    {
        foreach(var collider in _colliders.ToList())
        {
            collider.Update(); //debug

            if (collider.Enabled == false) continue;

            if (CheckForCollision(collider, out Collider hit))
            {
                collider.OnCollision(hit);
            }
        }
    }

    private static bool CheckForCollision(Collider collider, out Collider hit)
    {
        Vector2 l1 = GetTopLeftPoint(collider);
        Vector2 r1 = GetBottomRightPoint(collider);

        foreach (var colliderToCheck in _colliders)
        {
            if (colliderToCheck == collider) continue;
            if (colliderToCheck.Enabled == false) continue;
            if (collider.Parent == colliderToCheck.Parent) continue;

            var l2 = GetTopLeftPoint(colliderToCheck);
            var r2 = GetBottomRightPoint(colliderToCheck);

            if (CheckIfPointsOverlap(l1,r1,l2,r2))
            {
                hit = colliderToCheck;
                return true;
            }
        }

        hit = null;
        return false;
    }

    private static Vector2 GetTopLeftPoint(Collider collider)
    {
        Vector2 point = Vector2.Zero;
        point.X = (float)Math.Round(collider.Position.X - (collider.Width / 2));
        point.Y = (float)Math.Round(collider.Position.Y - (collider.Height / 2));
        return point;
    }

    private static Vector2 GetBottomRightPoint(Collider collider)
    {
        Vector2 point = Vector2.Zero;
        point.X = (float)Math.Round(collider.Position.X + (collider.Width / 2));
        point.Y = (float)Math.Round(collider.Position.Y + (collider.Height / 2));
        return point;
    }

    private static bool CheckIfPointsOverlap(Vector2 Point1Left,
                              Vector2 Point1Right,
                              Vector2 Point2Left,
                              Vector2 Point2Right)
    {
        if ((Point1Left.X > Point2Right.X) || (Point2Left.X > Point1Right.X)) return false;

        if ((Point1Left.Y > Point2Right.Y) || (Point2Left.Y > Point1Right.Y)) return false;

        return true;
    }
}
