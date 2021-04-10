using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace PingPongPlaya.Objects
{
    public class Wind
    {
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;

        private Random random = new Random();

        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private Body body;
        private SoundEffect windSound;

        public Wind(Body body, int width)
        {
            this.body = body;
            this.body.OnCollision += OnCollision;
            if (random.Next(2) == 1)
            {
                body.Position = new Vector2(-32, random.Next(20, 350));
                velocity = new Vector2(35, 0);
            }
            else
            {
                body.Position = new Vector2(width, random.Next(20, 350));
                velocity = new Vector2(-35, 0);
            }
        }

        private bool OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            windSound.Play();
            return true;
        }

        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            body.Position += velocity * t;
            position = body.Position;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("wind");
            windSound = content.Load<SoundEffect>("WindSounds/wind1");
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
