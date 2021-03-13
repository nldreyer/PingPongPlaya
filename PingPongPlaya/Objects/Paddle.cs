using PingPongPlaya.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingPongPlaya.Objects
{
    public class Paddle
    {
        private Texture2D texture;
        private Texture2D paddleBottom;
        private Vector2 position;
        private BoundingRectangle bounds;
        private MouseState currentMouseState;
        private MouseState priorMouseState;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Current instantaneous velocity
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("paddle");
            paddleBottom = content.Load<Texture2D>("paddle_bottom");
            bounds = new BoundingRectangle(new Vector2(0,0), 256, 32);
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            Velocity = new Vector2(currentMouseState.X - priorMouseState.X, currentMouseState.Y - priorMouseState.Y);
            position = new Vector2(currentMouseState.X - 128, currentMouseState.Y - 16);
            bounds.RectangleBounds.X = (int)position.X - 32;
            bounds.RectangleBounds.Y = (int)position.Y;
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White);
            spriteBatch.Draw(paddleBottom, new Vector2(position.X + 112, position.Y), null, Color.White);
        }
    }
}
