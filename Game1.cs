using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;

namespace forged_fury
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _windowBackground;
        private Texture2D _levelBackground;
        private const int _backgroundWidth = 448;
        private const int _backgroundHeight = 288;
        private const float _spriteScaleAmount = 1.4f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _levelBackground = Content.Load<Texture2D>("Level");
            _windowBackground = new Texture2D(GraphicsDevice, 1, 1);
            _windowBackground.SetData(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_windowBackground, 
                              new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), 
                              Color.Black);

            int width = Convert.ToInt32(_backgroundWidth * _spriteScaleAmount);
            int height = Convert.ToInt32(_backgroundHeight * _spriteScaleAmount);

            _spriteBatch.Draw(_levelBackground,new Rectangle(_graphics.PreferredBackBufferWidth/2-(width / 2), 
                                                             _graphics.PreferredBackBufferHeight/2 - (height / 2),
                                                             width,
                                                             height),
                                                             Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
