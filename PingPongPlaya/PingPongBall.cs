using System;
using System.Collections.Generic;
using System.Text;
using CollisionExample.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongPlaya
{
    public class PingPongBall
    {
        private const float ANIMATION_SPEED = 0.08f;
        private double animationTimer;
        private int animationFrame;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration = new Vector2(0, 800);
        private Texture2D texture;
        private BoundingCircle bounds;
        private Random random = new Random();

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// Creates a new ping pong ball
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public PingPongBall(Vector2 position)
        {
            this.position = position;
            this.bounds = new BoundingCircle(position + new Vector2(32, 32), 32);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ping_pong_ball");
        }

        /// <summary>
        /// Updates the paddle
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity += acceleration * t;
            position += velocity * t;
            bounds.Center = position;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 64, 0, 64, 64);
            spriteBatch.Draw(texture, position, source, Color.White);
        }

        public void HitSide(Game g)
        {
            if (position.X <= 0)
            {
                position.X = 1;
            }
            else
            {
                position.X = g.GraphicsDevice.Viewport.Width - 65;
            }
            velocity *= new Vector2(-1, 1);
        }

        public void HitPaddle()
        {
            int x = random.Next(200);
            if (random.Next(1) == 0) x *= -1;

            velocity = new Vector2(x, -600);
        }
    }
}
