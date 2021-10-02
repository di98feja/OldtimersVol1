using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace OldtimersVol1
{
    public enum States
    {
        LogoFadein,
        RocketStage1,
        RocketStage2,
        RocketStage3
    }

    public class Demo : Game
    {
        private Texture2D _backgroundTexture;
        private Texture2D _rocketTexture;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _bgPos;
        private Vector2 _rocketStartPos;
        private Vector2 _rocketTargetPos;
        private Vector2 _rocketCurrentPos;
        private float _rocketEase;
        private readonly Vector2 _rocketSize = new Vector2(168, 100);
        private int _rocketCurrentFrame = 0;
        private float _rocketFrameTimer = 0f;
        private const int _rocketYDrift = 5;
        private const int _rocketStayDelay = 10000;
        private int _rocketStayTimer;
        private int _rocketYPos;
        private System.Random _random;

        private Vector2 _textScrollerPos;
        private SpriteFont font;
        private Song song;
        private float _rocketXLerp = 0.01f;
        private States _state = States.RocketStage1;
        private const string _scrollText = "Oldtimers presents stuff at n0LanX, for times that used to be";

        public Demo()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            //font = Content.Load<SpriteFont>("2P");
        }

        protected override void Initialize()
        {
            //new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _textScrollerPos = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight - 30);
            _rocketYPos = _graphics.PreferredBackBufferHeight - 200;
            _rocketStartPos = new Vector2(-100, _rocketYPos);
            _rocketCurrentPos = _rocketStartPos;
            this.song = Content.Load<Song>("technogeek");
            MediaPlayer.Play(song);

            _rocketTargetPos = new Vector2(_graphics.PreferredBackBufferWidth / 2 - _rocketSize.X / 2, _rocketYPos);
            _random = new System.Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTexture = Content.Load<Texture2D>("starsBackground");
            _rocketTexture = Content.Load<Texture2D>("rocket");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Background
            _bgPos.X = _bgPos.X - 1;
            if (_bgPos.X < -_graphics.PreferredBackBufferWidth + 1)
            {
                _bgPos.X = 0;
            }

            // Scroller
            //_textScrollerPos.X = _textScrollerPos.X - 2;
            //if (_textScrollerPos.X < -font.MeasureString(_scrollText).X + 1)
            //{
            //    _textScrollerPos.X = _graphics.PreferredBackBufferWidth;
            //}

            // Rocket
            if (_rocketTargetPos.X - _rocketCurrentPos.X < 1f && _state == States.RocketStage1)
            {
                _rocketTargetPos.X = _graphics.PreferredBackBufferWidth;
                _rocketStartPos = _rocketCurrentPos;
                _state = States.RocketStage2;
                _rocketStayTimer = (int)(gameTime.TotalGameTime.TotalMilliseconds + _rocketStayDelay);
            }
            else if (gameTime.TotalGameTime.TotalMilliseconds > _rocketStayTimer && _state == States.RocketStage2)
            {
                _state = States.RocketStage3;
            }
            else if (_rocketTargetPos.X - _rocketCurrentPos.X < 1f && _state == States.RocketStage3)
            {
                _state = States.LogoFadein;
            }

            if (System.Math.Abs(_rocketCurrentPos.Y - _rocketTargetPos.Y) < 2)
            {
                _rocketTargetPos.Y = _random.Next(_rocketYPos - _rocketYDrift, _rocketYPos + _rocketYDrift);
            }
            else
            {
                if (_state == States.RocketStage1)
                {
                    _rocketEase += (float)(System.Math.PI/2) / 100;
                    _rocketCurrentPos.X = _rocketStartPos.X + (_rocketTargetPos.X - _rocketStartPos.X) * (float)System.Math.Sin(_rocketEase);
                    _rocketCurrentPos.Y = MathHelper.Lerp(_rocketStartPos.Y, _rocketTargetPos.Y, 0.1f);
                }
                else if (_state == States.RocketStage3)
                {
                    _rocketEase -= (float)(System.Math.PI / 2) / 100;
                    _rocketCurrentPos.X = _rocketTargetPos.X - (_rocketTargetPos.X - _rocketStartPos.X) * (float)System.Math.Sin(_rocketEase);
                    _rocketCurrentPos.Y = MathHelper.Lerp(_rocketStartPos.Y, _rocketTargetPos.Y, 0.1f);
                }
            }
            if (_rocketFrameTimer < gameTime.TotalGameTime.TotalMilliseconds)
            {
                _rocketFrameTimer = (float)(gameTime.TotalGameTime.TotalMilliseconds + 100.0);
                _rocketCurrentFrame = ++_rocketCurrentFrame % 3;
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

            // Rocket
            var sourceRect = new Rectangle((int)(_rocketSize.X * _rocketCurrentFrame), 0, (int)_rocketSize.X, (int)_rocketSize.Y);
            _spriteBatch.Draw(_rocketTexture, _rocketCurrentPos, sourceRect, Color.White);

            // Text scroller
            //_spriteBatch.DrawString(font, _scrollText, _textScrollerPos, Color.White, 0, new Vector2(0,0), 1.0f, SpriteEffects.None, 0.5f);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        enum EaseType { None, In, Out, InOut }

        private float Ease(float t, EaseType easeType)
        {
            if (easeType == EaseType.None)
                return t;
            else if (easeType == EaseType.In)
                return MathHelper.Lerp(0.0f, 1.0f, (float)(1.0 - System.Math.Cos(t * System.Math.PI * .5)));
            else if (easeType == EaseType.Out)
                return MathHelper.Lerp(0.0f, 1.0f, (float)System.Math.Sin(t * System.Math.PI * .5));
            else
                return MathHelper.SmoothStep(0.0f, 1.0f, t);
        }
    }
}
