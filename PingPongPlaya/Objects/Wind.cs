using PingPongPlaya.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingPongPlaya.Objects
{
    public class Wind
    {
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;

        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private BoundingRectangle bounds;
        private Random random;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("wind");
            bounds = new BoundingRectangle(new Vector2(0, 0), 32, 16);
            position = new Vector2(200,200);
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            position += velocity;
            bounds.RectangleBounds.X = (int)position.X - 32;
            bounds.RectangleBounds.Y = (int)position.Y;
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
                if (animationFrame > 8) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 32, 8, 32, 16);
            spriteBatch.Draw(texture, position, source, Color.White);
        }
    }
}
