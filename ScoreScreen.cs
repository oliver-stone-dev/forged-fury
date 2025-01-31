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
    private readonly Environment _environment;

    private Rectangle _startButton;
    private Rectangle _exitButton;

    private int _buttonWidth = 160;
    private int _buttonHeight = 64;

    public int Rounds { get; set; }
    public int Score { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool EndFlag { get; set; }

    public ScoreScreen(GraphicsDeviceManager graphics, SpriteFont font,Environment environment)
    {
        _font = font;
        _environment = environment;

        var backgroundAsset = AssetManager.Textures.Get("WindowBackground");
        var backgroundSprite = backgroundAsset!.AssetObject;
        if (backgroundSprite == null) return;

        _background = new Sprite(backgroundSprite);
        _background.Position.X = graphics.PreferredBackBufferWidth / 2;
        _background.Position.Y = graphics.PreferredBackBufferHeight / 2;
        _background.Width = graphics.PreferredBackBufferWidth;
        _background.Height = graphics.PreferredBackBufferHeight;
        _background.Color = Color.Black;

        Width = graphics.PreferredBackBufferWidth;
        Height = graphics.PreferredBackBufferHeight;

        _exitButton = new Rectangle((Width / 2) - _buttonWidth / 2, Height / 2 + Convert.ToInt32((_buttonHeight * 1.5f)), _buttonWidth, _buttonHeight);
        
        var endBtnAsset = AssetManager.Textures.Get("ExitButton");
        var endSprite = endBtnAsset!.AssetObject;
        if (endSprite == null) return;

        _exitButtonSprite = new Sprite(endSprite);
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
        var levelPosition = _environment.GetLevelPosition();
        var levelSize = _environment.GetLevelSize();
        var textArea = new Rectangle(
            Convert.ToInt32(levelPosition.X - (levelSize.X / 2)),
            Convert.ToInt32(levelPosition.Y - (levelSize.Y / 2)),
            Convert.ToInt32(levelSize.X),
            Convert.ToInt32(levelSize.Y / 2));

        var deathTextSize = _font.MeasureString("YOU DIED!");
        var roundsTextSize = _font.MeasureString($"Rounds Complete: {Rounds}");
        var scoreTextSize = _font.MeasureString($"Score: {Score}");

        _background.Draw(spriteBatch);
        spriteBatch.DrawString(_font, $"YOU DIED!", new Vector2((Width / 2) - deathTextSize.X / 2, textArea.Top), Color.White);
        spriteBatch.DrawString(_font, $"Rounds Complete: {Rounds}", new Vector2((Width / 2) - roundsTextSize.X / 2, textArea.Center.Y), Color.White);
        spriteBatch.DrawString(_font, $"Score: {Score}", new Vector2((Width / 2) - scoreTextSize.X / 2, textArea.Bottom), Color.White);
        _exitButtonSprite.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
}
