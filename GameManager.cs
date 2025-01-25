using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class GameManager : GameObject
{
    private readonly PlayerController _player;

    public bool GameEndFlag { get; set; }

    public GameManager(PlayerController player)
    {
        _player = player;
    }
    public override void Update(GameTime gameTime)
    {
        if (_player.Health <= 0)
        {
            GameEndFlag = true;
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
   
}
