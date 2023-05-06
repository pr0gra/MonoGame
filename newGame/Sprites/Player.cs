using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newGame.Sprites
{
    public class Player : Sprite
    {
        public int Score;
        public bool HasDied = false;


        public Player(Texture2D texture)
          : base(texture)
        {
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();

            foreach (var sprite in sprites)
            {
                if (sprite is Player)
                    continue;

                if (sprite.Rectangle.Intersects(this.Rectangle))
                {
                    Score++;
                    sprite.IsRemoved = true;
                }

                if (sprite is Bomb && sprite.Rectangle.Intersects(this.Rectangle))
                {
                    this.HasDied = true;
                }
            }
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                Position.X -= Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Position.X += Speed;

            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Position.Y -= Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Position.Y += Speed;

            Position = Vector2.Clamp(Position, new Vector2(0, 0), new Vector2(Game1.ScreenWidth - this.Rectangle.Width, Game1.ScreenHeight - this.Rectangle.Height));
        }
    }
}
