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

    private bool _playerDeathFlag = false;
    private int _gameEndDelayMs = 2000;
    private int _gameEndDelayTimer = 0;

    public bool GameEndFlag { get; set; }

    public GameManager(PlayerController player)
    {
        _player = player;
        _gameEndDelayTimer = _gameEndDelayMs;
    }
    public override void Update(GameTime gameTime)
    {
        if (_player.Health <= 0)
        {
            _playerDeathFlag = true;
        }

        if (_playerDeathFlag)
        {
            _gameEndDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;
            
            if (_gameEndDelayTimer <= 0)
            {
                GameEndFlag = true;
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
   
}
