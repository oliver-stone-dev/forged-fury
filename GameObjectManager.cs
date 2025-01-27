using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public static class GameObjectManager
{
    private static List<GameObject> _gameObjects = new();

    public static void Update(GameTime gameTime)
    {
        _gameObjects.ToList().ForEach(g => g.Update(gameTime));
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        _gameObjects.ToList().ForEach(g => g.Draw(spriteBatch));
    }

    public static void Add(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }

    public static void Remove(GameObject gameObject)
    {
        _gameObjects.Remove(gameObject);
    }

    public static void Clear()
    {
        _gameObjects.Clear();
    }

    public static int CountObjects(string name)
    {
        return _gameObjects.Count(o => o.Name == name);
    }
}
