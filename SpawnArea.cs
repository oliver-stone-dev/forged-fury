using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class SpawnArea
{
    private Rectangle _spawnAreaLeft;
    private Rectangle _spawnAreaRight;

    private int _spawnAreaWidth = 128;
    private int _spawnAreaHeight = 175;
    public SpawnArea(Rectangle area)
    {
        _spawnAreaLeft = new Rectangle(area.Left, area.Top, _spawnAreaWidth, _spawnAreaHeight);
        _spawnAreaRight = new Rectangle(area.Right - _spawnAreaWidth, area.Top, _spawnAreaWidth, _spawnAreaHeight);
    }

    private bool PointInsideArea(Vector2 point, Rectangle area)
    {
        if (area.Contains(point))
        {
            return true;
        }

        return false;
    }

    private Vector2 GetRandomPointWithinArea(Rectangle area)
    {
        var rand = new Random();
        var point = Vector2.Zero;

        point.X = rand.Next(area.Left, area.Right);
        point.Y = rand.Next(area.Top, area.Bottom);

        return point;
    }

    public Vector2 GetRandomSpawnPoint(Vector2 positionToAvoid)
    {
        var spawnPoint = Vector2.Zero;
        var rand = new Random();

        var playerLeft = PointInsideArea(positionToAvoid, _spawnAreaLeft);
        var playerRight = PointInsideArea(positionToAvoid, _spawnAreaRight);

        if (playerLeft)
        {
            spawnPoint = GetRandomPointWithinArea(_spawnAreaRight);
        }
        else if (playerRight)
        {
            spawnPoint = GetRandomPointWithinArea(_spawnAreaLeft);
        }
        else
        {
            var randomSide = rand.Next(0, 2);
            if (randomSide == 0)
            {
                spawnPoint = GetRandomPointWithinArea(_spawnAreaRight);
            }
            else
            {
                spawnPoint = GetRandomPointWithinArea(_spawnAreaLeft);
            }
        }

        return spawnPoint;
    }
}
