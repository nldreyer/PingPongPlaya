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
        private SoundEffect wind;
        private Random random = new Random();
        private SpriteFont bangers;
        private SpriteFont bangersSmall;
        private GameState gameState;

        private PingPongBall pingPongBall;
        private Paddle paddle;
        private TimeSpan currentTime;
        private TimeSpan highScoreTime;

        public PingPongPlayaGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1024;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            pingPongBall = new PingPongBall(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 128));
            paddle = new Paddle();
            ballBounces = new SoundEffect[3];
            ballBounces[0] = Content.Load<SoundEffect>("BallSounds/pong1");
            ballBounces[1] = Content.Load<SoundEffect>("BallSounds/pong2");
            ballBounces[2] = Content.Load<SoundEffect>("BallSounds/pong3");
            wind = Content.Load<SoundEffect>("WindSounds/wind1");
            bangers = Content.Load<SpriteFont>("bangers");
            bangersSmall = Content.Load<SpriteFont>("bangersSmall");
            currentTime = new TimeSpan(0, 0, 0);
            highScoreTime = new TimeSpan(0, 0, 0);
            gameState = GameState.Menu;
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

            switch (gameState)
            {
                case GameState.Menu:

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        pingPongBall.Reset(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 128));
                        gameState = GameState.Active;
                    }

                    break;

                case GameState.Active:

                    currentTime += gameTime.ElapsedGameTime;

                    if (pingPongBall.Bounds.CollidesWith(paddle.Bounds))
                    {
                        pingPongBall.HitPaddle(paddle.Velocity);
                        ballBounces[random.Next(3)].Play();
                    }
                    if (pingPongBall.Bounds.OffScreenBounce(this, out Vector2 redirect))
                    {
                        if (redirect.X == -1)
                        {
                            pingPongBall.HitSide(this);
                        }
                        else if (redirect.X == 0)
                        {
                            gameState = GameState.Lost;
                        }
                    }

                    pingPongBall.Update(gameTime);
                    paddle.Update(gameTime);

                    break;

                case GameState.Lost:

                    if (currentTime > highScoreTime)
                    {
                        highScoreTime = currentTime;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        pingPongBall.Reset(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 128));
                        currentTime = new TimeSpan(0, 0, 0);
                        gameState = GameState.Active;
                    }

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(60, 100, 245, 255));

            spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.Menu:

                    Vector2 titleLen = bangers.MeasureString("Ping Pong Playa");
                    Vector2 spaceLen = bangers.MeasureString("Press space to play");
                    spriteBatch.DrawString(bangers, "Ping Pong Playa", new Vector2(GraphicsDevice.Viewport.Width / 2 - (titleLen.X / 2), GraphicsDevice.Viewport.Height / 2 - (titleLen.Y / 2) - 64), Color.Black);
                    spriteBatch.DrawString(bangers, "Press space to play", new Vector2(GraphicsDevice.Viewport.Width / 2 - (spaceLen.X / 2), GraphicsDevice.Viewport.Height / 2 + (spaceLen.Y / 2) - 64), Color.Black);

                    break;
                case GameState.Active:

                    pingPongBall.Draw(gameTime, spriteBatch);
                    paddle.Draw(gameTime, spriteBatch);

                    float hitHeight = bangers.MeasureString("Hits").Y;
                    Vector2 highScoreSize = bangers.MeasureString($"High Score: {highScoreTime:hh\\:mm\\:ss}");
                    spriteBatch.DrawString(bangersSmall, $"Time: {currentTime:hh\\:mm\\:ss}", new Vector2(5, 5), Color.WhiteSmoke);
                    spriteBatch.DrawString(bangersSmall, $"Hits: {pingPongBall.PaddleHits}", new Vector2(5, (int)hitHeight), Color.WhiteSmoke);
                    spriteBatch.DrawString(bangersSmall, $"High Score: {highScoreTime:hh\\:mm\\:ss}", new Vector2(GraphicsDevice.Viewport.Width - highScoreSize.X + 120, 5), Color.WhiteSmoke);

                    break;
                case GameState.Lost:

                    GraphicsDevice.Clear(new Color(80, 120, 245, 255));

                    Vector2 lostSize = bangers.MeasureString("You lost!");
                    Vector2 againSize = bangers.MeasureString("Play again? (Space)");
                    spriteBatch.DrawString(bangers, "You lost!", new Vector2(GraphicsDevice.Viewport.Width / 2 - (lostSize.X / 2), GraphicsDevice.Viewport.Height / 2 - (lostSize.Y / 2) - 64), Color.Black);
                    spriteBatch.DrawString(bangers, "Play again? (Space)", new Vector2(GraphicsDevice.Viewport.Width / 2 - (againSize.X / 2), GraphicsDevice.Viewport.Height / 2 + (againSize.Y / 2) - 64), Color.Black);

                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
