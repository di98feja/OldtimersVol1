using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OldtimersVol1
{
    public class Game1 : Game
    {
        Texture2D _backgroundTexture;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _bgPos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600; 
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
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
            _spriteBatch.Draw(_backgroundTexture, _bgPos, Color.White);
            _spriteBatch.Draw(_backgroundTexture, new Vector2(_bgPos.X + 800, _bgPos.Y), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
