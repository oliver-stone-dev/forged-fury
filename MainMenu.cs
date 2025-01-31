using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace forged_fury;

public class MainMenu : GameObject
{
    private readonly Sprite _background;
    private readonly Sprite _startButtonSprite;
    private readonly Sprite _exitButtonSprite;
    private readonly Sprite _titleSprite;

    public int MenuWidth { get; set; }
    public int MenuHeight { get; set; }
    public bool StartFlag { get; set; }
    public bool EndFlag { get; set; }

    private Rectangle _startButton;
    private Rectangle _exitButton;

    private int _buttonWidth = 160;
    private int _buttonHeight = 64;

    public MainMenu(GraphicsDeviceManager graphics)
    {
        var backgroundAsset = AssetManager.Textures.Get("WindowBackground");
        var backgroundSprite = backgroundAsset!.AssetObject;
        if (backgroundSprite == null) return;

        _background = new Sprite(backgroundSprite);
        _background.Position.X = graphics.PreferredBackBufferWidth / 2;
        _background.Position.Y = graphics.PreferredBackBufferHeight / 2;
        _background.Width = graphics.PreferredBackBufferWidth;
        _background.Height = graphics.PreferredBackBufferHeight;
        _background.Color = Color.Black;

        MenuWidth = graphics.PreferredBackBufferWidth;
        MenuHeight = graphics.PreferredBackBufferHeight;

        _startButton = new Rectangle((MenuWidth / 2) - _buttonWidth / 2, MenuHeight / 2, _buttonWidth, _buttonHeight);
        _exitButton = new Rectangle((MenuWidth / 2) - _buttonWidth / 2, MenuHeight / 2 + Convert.ToInt32((_buttonHeight * 1.5f)), _buttonWidth, _buttonHeight);

        var startBtnAsset = AssetManager.Textures.Get("StartButton");
        var startSprite = startBtnAsset!.AssetObject;
        if (startSprite == null) return;

        _startButtonSprite = new Sprite(startSprite);
        _startButtonSprite.Position.X = _startButton.X + _buttonWidth / 2;
        _startButtonSprite.Position.Y = _startButton.Y + _buttonHeight / 2;
        _startButtonSprite.Width = _startButton.Width;
        _startButtonSprite.Height = _startButton.Height;

        var endBtnAsset = AssetManager.Textures.Get("ExitButton");
        var endSprite = endBtnAsset!.AssetObject;
        if (endSprite == null) return;

        _exitButtonSprite = new Sprite(endSprite);
        _exitButtonSprite.Position.X = _exitButton.X + _buttonWidth / 2; ;
        _exitButtonSprite.Position.Y = _exitButton.Y + _buttonHeight / 2; ;
        _exitButtonSprite.Width = _exitButton.Width;
        _exitButtonSprite.Height = _exitButton.Height;

        var titleAsset = AssetManager.Textures.Get("Title");
        var titleSprite = titleAsset!.AssetObject;
        if (titleSprite == null) return;

        _titleSprite = new Sprite(titleSprite);
        _titleSprite.Position.X = graphics.PreferredBackBufferWidth / 2;
        _titleSprite.Position.Y = graphics.PreferredBackBufferHeight / 2 - (MenuHeight / 2) + titleSprite.Height;
        _titleSprite.Width = titleSprite.Width;
        _titleSprite.Height = titleSprite.Height;
        _titleSprite.Scale = 2f;
    }

    public override void Update(GameTime gameTime)
    {
        var state = Mouse.GetState();

        if (state.LeftButton == ButtonState.Pressed)
        {
            var point = state.Position;
            
            if (_startButton.Contains(point))
            {
                StartFlag = true;
            }

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
        _startButtonSprite.Draw(spriteBatch);
        _exitButtonSprite.Draw(spriteBatch);
        _titleSprite.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
}
