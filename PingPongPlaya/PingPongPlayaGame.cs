using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PingPongPlaya
{
    public class PingPongPlayaGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SoundEffect[] ballBounces;
        private Random random = new Random();

        private PingPongBall pingPongBall;
        private Paddle paddle;

        public PingPongPlayaGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            pingPongBall = new PingPongBall(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));
            paddle = new Paddle();
            ballBounces = new SoundEffect[3];
            ballBounces[0] = Content.Load<SoundEffect>("BallSounds/pong1");
            ballBounces[1] = Content.Load<SoundEffect>("BallSounds/pong2");
            ballBounces[2] = Content.Load<SoundEffect>("BallSounds/pong3");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pingPongBall.LoadContent(Content);
            paddle.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (pingPongBall.Bounds.CollidesWith(paddle.Bounds))
            {
                pingPongBall.HitPaddle();
                ballBounces[random.Next(2)].Play();
            }
            if (pingPongBall.Bounds.OffScreenBounce(this, out Vector2 redirect))
            {
                if (redirect.X == -1)
                {
                    pingPongBall.HitSide(this);
                }
                else if (redirect.X == 0)
                {
                    Exit();
                }
            }

            pingPongBall.Update(gameTime);
            paddle.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            pingPongBall.Draw(gameTime, spriteBatch);
            paddle.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
