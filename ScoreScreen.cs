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

    public int Rounds { get; set; }
    public int Score { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public bool EndFlag { get; set; }

    private Rectangle _startButton;
    private Rectangle _exitButton;

    private int _buttonWidth = 40;
    private int _buttonHeight = 40;



    public ScoreScreen(Texture2D backgroundSprite, GraphicsDeviceManager graphics, SpriteFont font)
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

        _exitButton = new Rectangle((Width / 2) - 36, 550, Width, Height);
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
        spriteBatch.DrawString(_font, $"Rounds Complete: {Rounds}", new Vector2(Width / 2, 270), Color.White);
        spriteBatch.DrawString(_font, $"Score: {Score}", new Vector2(Width / 2, 400), Color.White);
        spriteBatch.DrawString(_font, "EXIT", new Vector2(_exitButton.X, _exitButton.Y), Color.White);
        base.Draw(spriteBatch);
    }
}
