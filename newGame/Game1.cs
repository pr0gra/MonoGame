using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using newGame.Models;
using newGame.Sprites;
using System.Collections.Generic;

namespace newGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Random Random;

        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<Sprite> _sprites;

        private SpriteFont _font;

        private float _timer;

        private Texture2D _coinTexture;

        private bool _hasStarted = false;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Random = new Random();

            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
        }


        private void Restart()
        {
            var playerTexture = Content.Load<Texture2D>("MainHero");

            _sprites = new List<Sprite>()
              {
                new Player(playerTexture)
                {
                  Position = new Vector2((ScreenWidth / 2) - (playerTexture.Width / 2), ScreenHeight - playerTexture.Height),
                  Input = new Input()
                  {
                    Left = Keys.A,
                    Right = Keys.D,
                    Up = Keys.W,
                    Down = Keys.S,
                  },
                  Speed = 10f,
                }
              };

            _hasStarted = false;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var playerTexture = Content.Load<Texture2D>("MainHero");

            _sprites = new List<Sprite>()
              {
                new Player(playerTexture)
                {
                  Input = new Input()
                  {
                    Left = Keys.A,
                    Right = Keys.D,
                    Up = Keys.W,
                    Down = Keys.S,
                  },
                  Position = new Vector2((ScreenWidth / 2) - (playerTexture.Width / 2), ScreenHeight - playerTexture.Height),
                  Speed = 10f,
                },
              };

            _font = Content.Load<SpriteFont>("Font");
            _coinTexture = Content.Load<Texture2D>("Coin");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _hasStarted = true;

            if (!_hasStarted)
                return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in _sprites)
                sprite.Update(gameTime, _sprites);

            PostUpdate();

            SpawnCoin();

            if (_timer > 0.25f)
            {
                _timer = 0f;
                _sprites.Add(new Bomb(Content.Load<Texture2D>("Bomb")));

            }

            for (int i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];

                if (sprite.IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }

                if (sprite is Player)
                {
                    var player = sprite as Player;

                    if (player.HasDied)
                    {
                        Restart();
                    }
                }
            }

            base.Update(gameTime);
        }

        private void SpawnCoin()
        {
            int xPos = 0;
            int yPos = 0;
            if (_timer > 1)
            {
                _timer = 0;

                xPos = Random.Next(0, ScreenWidth - _coinTexture.Width);
                yPos = Random.Next(0, ScreenHeight - _coinTexture.Height);
       
                _sprites.Add(new Sprite(_coinTexture)
                {
                    Position = new Vector2(xPos, yPos),
                });
            }
        }

        private void PostUpdate()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var sprite in _sprites)
                sprite.Draw(_spriteBatch);

            var fontY = 10;

            foreach (var sprite in _sprites)
            {
                if (sprite is Player)
                {
                    _spriteBatch.DrawString(_font, ((Player)sprite).Score.ToString(), new Vector2(10, fontY += 20), Color.Black);
                    //if ((Player)sprite.Score == 10)
                  //  {
                   //     Restart();
                   //}
                }
            }

           

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}