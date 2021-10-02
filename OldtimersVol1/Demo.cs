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
        private Texture2D _cometTexture;
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
        private Vector2 _tipsScrollerPos;
        private Vector2 _cometPos;
        private SpriteFont font;
        private Song song;
        private float _rocketXLerp = 0.01f;
        private int _state = 0;
        private bool comets;
        private double cometsTime;
        private const string _scrollText = "Oldtimers presents stuff at n0LanX, from times that used to be";
        private string _scrollTips = (@"Ladies and gentlemen of the class of '97
                                            Wear sunscreen
                                            If I could offer you only one tip for the future, sunscreen would be it
                                            A long-term benefits of sunscreen have been proved by scientists
                                            Whereas the rest of my advice has no basis more reliable
                                            Than my own meandering experience, I will dispense this advice now
                                            Enjoy the power and beauty of your youth, oh, never mind
                                            You will not understand the power and beauty of your youth
                                            Until they've faded, but trust me, in 20 years, you'll look back
                                            At photos of yourself and recall in a way you can't grasp now
                                            How much possibility lay before you and how fabulous you really looked
                                            You are not as fat as you imagine
                                            Don't worry about the future
                                            Or worry, but know that worrying
                                            Is as effective as trying to solve an algebra equation by chewing Bubble gum
                                            The real troubles in your life
                                            Are apt to be things that never crossed your worried mind
                                            The kind that blindsides you at 4 p.m. on some idle Tuesday
                                            Do one thing every day that scares you
                                            Saying, don't be reckless with other people's hearts
                                            Don't put up with people who are reckless with yours
                                            Floss
                                            Don't waste your time on jealousy
                                            Sometimes you're ahead, sometimes you're behind
                                            The race is long and in the end, it's only with yourself
                                            Remember compliments you receive, forget the insults
                                            If you succeed in doing this, tell me how
                                            Keep your old love letters, throw away your old bank statements
                                            Stretch
                                            Don't feel guilty if you don't know what you want to do with your life
                                            The most interesting people I know
                                            Didn't know at 22 what they wanted to do with their lives
                                            Some of the most interesting 40-year-olds I know still don't
                                            Get plenty of calcium
                                            Be kind to your knees
                                            You'll miss them when they're gone
                                            Maybe you'll marry, maybe you won't
                                            Maybe you'll have children, maybe you won't
                                            Maybe you'll divorce at 40, maybe you'll dance the 'Funky Chicken'
                                            On your 75th wedding anniversary
                                            Whatever you do, don't congratulate yourself too much
                                            Or berate yourself either
                                            Your choices are half chance, so are everybody else's
                                            Enjoy your body, use it every way you can
                                            Don't be afraid of it or what other people think of it
                                            It's the greatest instrument you'll ever own
                                            Dance, even if you have nowhere to do it but your own living room
                                            Read the directions even if you don't follow them
                                            Do not read beauty magazines, they will only make you feel ugly
                                            Get to know your parents, you never know when they'll be gone for good
                                            Be nice to your siblings, they're your best link to your past
                                            And the people most likely to stick with you in the future
                                            Understand that friends come and go
                                            But a precious few, who should hold on
                                            Work hard to bridge the gaps in geography and lifestyle
                                            For as the older you get
                                            The more you need the people you knew when you were young
                                            Live in New York City once but leave before it makes you hard
                                            Live in northern California once but leave before it makes you soft
                                            Travel
                                            Accept certain inalienable truths
                                            Prices will rise, politicians will philander, you too, will get old
                                            And when you do, you'll fantasize that when you were young
                                            Prices were reasonable, politicians were noble
                                            And children respected their elders
                                            Respect your elders
                                            Don't expect anyone else to support you
                                            Maybe you have a trust fund, maybe you'll have a wealthy spouse
                                            But you never know when either one might run out
                                            Don't mess too much with your hair
                                            Or by the time you're 40 it will look 85
                                            Be careful whose advice you buy but be patient with those who supply it
                                            Advice is a form of nostalgia, dispensing it is a way of fishing the past
                                            From the disposal, wiping it off, painting over the ugly parts
                                            And recycling it for more than it's worth
                                            But trust me on the sunscreen  ").Replace("\n", "     ");

        public Demo()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            //new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _textScrollerPos = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight - 30);
            _tipsScrollerPos = new Vector2(_graphics.PreferredBackBufferWidth, 30);
            _cometPos = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight - 60);
            _rocketYPos = _graphics.PreferredBackBufferHeight - 200;
            _rocketStartPos = new Vector2(-100, _rocketYPos);
            _rocketCurrentPos = _rocketStartPos;
            this.song = Content.Load<Song>("technogeek");
            MediaPlayer.Play(song);
            comets = false;
            _rocketTargetPos = new Vector2(_graphics.PreferredBackBufferWidth / 2 - _rocketSize.X / 2, _rocketYPos);
            _random = new System.Random();
            font = Content.Load<SpriteFont>("2P");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _cometTexture = Content.Load<Texture2D>("comet");
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
            _textScrollerPos.X = _textScrollerPos.X - 2;
            if (_textScrollerPos.X < -font.MeasureString(_scrollText).X + 1)
            {
                _textScrollerPos.X = _graphics.PreferredBackBufferWidth;
            }
            _tipsScrollerPos.X = _tipsScrollerPos.X - 5;
            if (_tipsScrollerPos.X < -font.MeasureString(_scrollTips).X + 1)
            {
                _tipsScrollerPos.X = _graphics.PreferredBackBufferWidth;
            }

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

            // Comets
            if (comets)
            {
                _cometPos.X = _cometPos.X - 10f;
                cometsTime = gameTime.TotalGameTime.TotalSeconds;
                if (_cometPos.X < -2000)
                {
                    comets = false;
                    _cometPos.Y =  _random.Next(-100, _graphics.PreferredBackBufferHeight);
                    _cometPos.X = _graphics.PreferredBackBufferWidth;
                }

            }
            else
            {
                if (gameTime.TotalGameTime.TotalSeconds - cometsTime > 1)
                {
                    comets = true;
                }
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
            _spriteBatch.DrawString(font, _scrollText, _textScrollerPos, Color.Green, 0, new Vector2(0,0), 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.DrawString(font, _scrollTips, _tipsScrollerPos, Color.Green, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            // Comets
            _spriteBatch.Draw(_cometTexture, _cometPos, null, Color.White);

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
