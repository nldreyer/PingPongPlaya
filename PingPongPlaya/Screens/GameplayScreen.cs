using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using PingPongPlaya;
using PingPongPlaya.Objects;
using PingPongPlaya.StateManagement;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Dynamics;

namespace PingPongPlaya.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private World world;
        private int worldBottom;

        private float _pauseAlpha;

        private SpriteBatch spriteBatch;
        private SpriteFont bangers;
        private SpriteFont bangersSmall;

        private PingPongBall pingPongBall;
        private Paddle paddle;
        private Wind[] winds;
        private Wind wind;
        private TimeSpan currentTime;
        private TimeSpan highScoreTime;

        public GameplayScreen(TimeSpan? highScoreTime)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            if (highScoreTime.HasValue) this.highScoreTime = (TimeSpan)highScoreTime;
            else highScoreTime = new TimeSpan(0, 0, 0);
        }

        public override void Activate()
        {
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            world = new World();
            world.Gravity = new Vector2(0, 400); // Not sure why this does not affect the ball very much. Ball seems to have an extremely low terminal velocity? Need to edit dependency source code?
            worldBottom = ScreenManager.GraphicsDevice.Viewport.Height;

            var top = int.MinValue;
            var left = 0;
            var bottom = int.MaxValue;
            var right = ScreenManager.GraphicsDevice.Viewport.Width;
            var edges = new Body[] {
                world.CreateEdge(new Vector2(left, top), new Vector2(left, bottom)),
                world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom))
            };

            foreach (var edge in edges)
            {
                edge.BodyType = BodyType.Static;
                edge.SetRestitution(1);
            }

            var ballBody = world.CreateCircle(32, 0.25f, new Vector2(right / 2, worldBottom / 2 - 128), BodyType.Dynamic);
            ballBody.IsBullet = true;
            ballBody.SetRestitution(10);

            var paddleBody = world.CreateRectangle(256, 8, 1f, default, 0, BodyType.Kinematic);
            paddleBody.IsBullet = true;
            paddleBody.SetFriction(10);
            paddleBody.SetRestitution(50);

            pingPongBall = new PingPongBall(ballBody);
            paddle = new Paddle(paddleBody);

            var windBody = world.CreateRectangle(32, 16, 1f);
            windBody.SetRestitution(50);
            windBody.IgnoreGravity = true;

            wind = new Wind(windBody, right);
            bangers = _content.Load<SpriteFont>("bangers");
            bangersSmall = _content.Load<SpriteFont>("bangersSmall");
            currentTime = new TimeSpan(0, 0, 0);

            pingPongBall.LoadContent(_content);
            paddle.LoadContent(_content);
            wind.LoadContent(_content);

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            currentTime += gameTime.ElapsedGameTime;

            if (pingPongBall.BelowScreen(worldBottom))
            {
                ScreenManager.RemoveAddScreen(this, new LostMenuScreen(highScoreTime), null);
            }

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            paddle.Update(gameTime);
            wind.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(new Color(60, 100, 245, 255));

            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            pingPongBall.Draw(gameTime, spriteBatch);
            paddle.Draw(gameTime, spriteBatch);
            wind.Draw(gameTime, spriteBatch);

            float hitHeight = bangers.MeasureString("Hits").Y;
            Vector2 highScoreSize = bangers.MeasureString($"High Score: {highScoreTime:hh\\:mm\\:ss}");
            spriteBatch.DrawString(bangersSmall, $"Time: {currentTime:hh\\:mm\\:ss}", new Vector2(5, 5), Color.WhiteSmoke);
            spriteBatch.DrawString(bangersSmall, $"Hits: {pingPongBall.PaddleHits}", new Vector2(5, (int)hitHeight), Color.WhiteSmoke);
            spriteBatch.DrawString(bangersSmall, $"High Score: {highScoreTime:hh\\:mm\\:ss}", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - highScoreSize.X + 120, 5), Color.WhiteSmoke);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
