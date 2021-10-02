using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OldtimersVol1
{
    public class Demo : Game
    {
        private Texture2D _backgroundTexture;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _bgPos;
        private SpriteFont font;
        private const string _scrollText = "Oldtimers presents stuff at n0LanX, for times that used to be";

        public Demo()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600; 
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            font = this.Content.Load<SpriteFont>("Fonts/myFont");
        }

        protected override void Initialize()
       {
            //new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTexture = Content.Load<Texture2D>("starsBackground");
        } 

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _bgPos.X = _bgPos.X - 1;
            if (_bgPos.X < -_graphics.PreferredBackBufferWidth+1)
            {
                _bgPos.X = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            // Background
            _spriteBatch.Draw(_backgroundTexture, _bgPos, Color.White);
            _spriteBatch.Draw(_backgroundTexture, new Vector2(_bgPos.X + 800, _bgPos.Y), Color.White);

            // Text scroller
            Vector2 textMiddlePoint = font.MeasureString(_scrollText) / 2;
            // Places text in center of the screen
            Vector2 position = new Vector2(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2);
            _spriteBatch.DrawString(font, _scrollText, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
