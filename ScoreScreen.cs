using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace forged_fury;

public class ScoreScreen : GameObject
{
    private readonly SpriteFont _font;
    private readonly Sprite _background;
    private readonly Sprite _exitButtonSprite;

    public int Rounds { get; set; }
    public int Score { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public bool EndFlag { get; set; }

    private Rectangle _startButton;
    private Rectangle _exitButton;


    private int _buttonWidth = 160;
    private int _buttonHeight = 64;



    public ScoreScreen(Texture2D backgroundSprite,Texture2D exitButtonSprite, GraphicsDeviceManager graphics, SpriteFont font)
    {
        _font = font;
        _background = new Sprite(backgroundSprite);
        _background.Position.X = graphics.PreferredBackBufferWidth / 2;
        _background.Position.Y = graphics.PreferredBackBufferHeight / 2;
        _background.Width = graphics.PreferredBackBufferWidth;
        _background.Height = graphics.PreferredBackBufferHeight;
        _background.Color = Color.Black;

        Width = graphics.PreferredBackBufferWidth;
        Height = graphics.PreferredBackBufferHeight;

        _exitButton = new Rectangle((Width / 2) - _buttonWidth / 2, 450, _buttonWidth, _buttonHeight);

        _exitButtonSprite = new Sprite(exitButtonSprite);
        _exitButtonSprite.Position.X = _exitButton.X + _buttonWidth / 2; ;
        _exitButtonSprite.Position.Y = _exitButton.Y + _buttonHeight / 2; ;
        _exitButtonSprite.Width = _exitButton.Width;
        _exitButtonSprite.Height = _exitButton.Height;
    }

    public override void Update(GameTime gameTime)
    {
        var state = Mouse.GetState();

        if (state.LeftButton == ButtonState.Pressed)
        {
            var point = state.Position;
            
            if (_exitButton.Contains(point))
            {
                EndFlag = true;
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _background.Draw(spriteBatch);
        spriteBatch.DrawString(_font, $"YOU DIED!", new Vector2((Width / 2) - 70, 150), Color.White);
        spriteBatch.DrawString(_font, $"Rounds Complete: {Rounds}", new Vector2((Width / 2) - 120, 250), Color.White);
        spriteBatch.DrawString(_font, $"Score: {Score}", new Vector2((Width / 2) - 50, 350), Color.White);
        _exitButtonSprite.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
}
