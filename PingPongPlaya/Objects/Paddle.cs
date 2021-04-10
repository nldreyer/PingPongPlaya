using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace PingPongPlaya.Objects
{
    public class Paddle
    {
        private Texture2D texture;
        private Texture2D paddleBottom;
        private MouseState currentMouseState;
        private MouseState priorMouseState;

        private Body body;

        public Paddle(Body body)
        {
            this.body = body;
        }

        /// <summary>
        /// Loads the sprite textures using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("paddle");
            paddleBottom = content.Load<Texture2D>("paddle_bottom");
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            body.Position = new Vector2(currentMouseState.X, currentMouseState.Y);
            body.LinearVelocity = new Vector2(currentMouseState.Position.X - priorMouseState.Position.X,
                                              currentMouseState.Position.Y - priorMouseState.Position.Y);
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(body.Position.X - 128, body.Position.Y - 16), null, Color.White);
            spriteBatch.Draw(paddleBottom, new Vector2(body.Position.X - 16, body.Position.Y - 16), null, Color.White);
        }
    }
}
