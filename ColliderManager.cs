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

    public static bool DrawDebugBoxes { get; set; }

    public static void Update(GameTime gameTime)
    {
        CheckForCollisions();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        if (DrawDebugBoxes == false) return;

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
            collider.TopCollision = false;
            collider.BottomCollision = false;
            collider.LeftCollision = false;
            collider.RightCollision = false;

            collider.Update(); //debug

            if (collider.Enabled == false) continue;

            HandleCollisions(collider);
        }
    }

    private static void HandleCollisions(Collider collider)
    {
        Vector2 l1 = GetTopLeftPoint(collider);
        Vector2 r1 = GetBottomRightPoint(collider);

        foreach (var colliderToCheck in _colliders.ToList())
        {
            if (colliderToCheck == collider) continue;
            if (colliderToCheck.Enabled == false) continue;
            if (collider.Parent == colliderToCheck.Parent) continue;

            var l2 = GetTopLeftPoint(colliderToCheck);
            var r2 = GetBottomRightPoint(colliderToCheck);

            if (CheckIfPointsOverlap(l1, r1, l2, r2))
            {
                if (collider.EnablePhysicsCollions && colliderToCheck.EnablePhysicsCollions)
                {
                    SetCollisionSides(collider, colliderToCheck);
                }
                collider.OnCollision(colliderToCheck);
            }
        }
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


    public static void SetCollisionSides(Collider collider1, Collider collider2)
    {
        var rect1 = new Rectangle((int)collider1.Position.X - (collider1.Width / 2), (int)collider1.Position.Y - (collider1.Height / 2), collider1.Width, collider1.Height);
        var rect2 = new Rectangle((int)collider2.Position.X - (collider2.Width / 2), (int)collider2.Position.Y - (collider2.Height / 2), collider2.Width, collider2.Height);

        var xDepth = Math.Min(rect2.Right - rect1.Left, rect1.Right - rect2.Left);
        var yDepth = Math.Min(rect2.Bottom - rect1.Top, rect1.Bottom - rect2.Top);

        if (xDepth < yDepth)
        {
            if (rect1.Right > rect2.Left && rect1.Right < rect2.Right)
            {
                collider1.RightCollision = true;
            }
            if (rect1.Left < rect2.Right && rect1.Left > rect2.Left)
            {
                collider1.LeftCollision = true;
            }
        }
        else if (xDepth > yDepth)
        {
            if (rect1.Bottom > rect2.Top && rect1.Bottom < rect2.Bottom)
            {
                collider1.BottomCollision = true;
            }
            if (rect1.Top < rect2.Bottom && rect1.Top > rect2.Top)
            {
                collider1.TopCollision = true;
            }
        }
    }
}
