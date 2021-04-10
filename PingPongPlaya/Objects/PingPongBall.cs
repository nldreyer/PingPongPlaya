using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PingPongPlaya.StateManagement;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace PingPongPlaya.Objects
{
    public class PingPongBall
    {
        private const float ANIMATION_SPEED = 0.08f;
        private double animationTimer;
        private int animationFrame;

        private Random random = new Random();

        private Vector2 origin;
        private Texture2D texture;
        private int paddleHits;
        private Body body;
        private World world;
        private SoundEffect[] ballBounces;

        /// <summary>
        /// Counter for paddle hits
        /// </summary>
        public int PaddleHits => paddleHits;

        /// <summary>
        /// Creates a new ping pong ball
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public PingPongBall(Body body)
        {
            this.body = body;
            this.origin = new Vector2(32, 32);
            this.body.OnCollision += OnCollision;
            this.world = body.World;
        }

        private bool OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            paddleHits++;
            Vector2 normal;
            body.ContactList.Contact.GetWorldManifold(out normal, out FixedArray2<Vector2> points);
            body.ApplyLinearImpulse(normal * -10000);
            ballBounces[random.Next(3)].Play();
            return true;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ping_pong_ball");

            ballBounces = new SoundEffect[3];
            ballBounces[0] = content.Load<SoundEffect>("BallSounds/pong1");
            ballBounces[1] = content.Load<SoundEffect>("BallSounds/pong2");
            ballBounces[2] = content.Load<SoundEffect>("BallSounds/pong3");
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
            spriteBatch.Draw(texture, body.Position, source, Color.White, body.Rotation, origin, 1f, SpriteEffects.None, 0);
        }

        public bool BelowScreen(int bottom)
        {
            if (body.Position.Y > bottom)
            {
                return true;
            }
            else return false;
        }
    }
}
