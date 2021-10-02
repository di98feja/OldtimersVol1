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
        private Vector2 _rocketTargetPos;
        private Vector2 _rocketCurrentPos;
        private const int _rocketYDrift = 10;
        private int _rocketYPos;

        private Vector2 _textScrollerPos;
        private SpriteFont font;
        private const string _scrollText = "Oldtimers presents stuff at n0LanX, for times that used to be";

        public Demo()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600; 
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            font = Content.Load<SpriteFont>("2P");
        }

        protected override void Initialize()
       {
            //new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _textScrollerPos = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight-30);
            _rocketYPos = _graphics.PreferredBackBufferHeight - 100;
            _rocketCurrentPos = new Vector2(-100, _rocketYPos);
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

            // Background
            _bgPos.X = _bgPos.X - 1;
            if (_bgPos.X < -_graphics.PreferredBackBufferWidth+1)
            {
                _bgPos.X = 0;
            }

            // Scroller
            _textScrollerPos.X = _textScrollerPos.X - 2;
            if (_textScrollerPos.X < -font.MeasureString(_scrollText).X + 1)
            {
                _textScrollerPos.X = _graphics.PreferredBackBufferWidth;
            }

            // Rocket
            if (Vector2.Distance(_rocketCurrentPos, _rocketTargetPos) > 2)
            {
                _rocketTargetPos.Y = _rocketYPos;
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
            _spriteBatch.DrawString(font, _scrollText, _textScrollerPos, Color.White, 0, new Vector2(0,0), 1.0f, SpriteEffects.None, 0.5f);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
