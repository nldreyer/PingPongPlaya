using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using PingPongPlaya;
using PingPongPlaya.Objects;
using PingPongPlaya.StateManagement;
using Microsoft.Xna.Framework.Content;

namespace PingPongPlaya.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;

        private float _pauseAlpha;

        private SpriteBatch spriteBatch;
        private SoundEffect[] ballBounces;
        //private SoundEffect windSound;
        private Random random = new Random();
        private SpriteFont bangers;
        private SpriteFont bangersSmall;

        private PingPongBall pingPongBall;
        private Paddle paddle;
        //private Wind wind;
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

            pingPongBall = new PingPongBall(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 128));
            paddle = new Paddle();
            //wind = new Wind();
            ballBounces = new SoundEffect[3];
            ballBounces[0] = _content.Load<SoundEffect>("BallSounds/pong1");
            ballBounces[1] = _content.Load<SoundEffect>("BallSounds/pong2");
            ballBounces[2] = _content.Load<SoundEffect>("BallSounds/pong3");
            //windSound = _content.Load<SoundEffect>("WindSounds/wind1");
            bangers = _content.Load<SpriteFont>("bangers");
            bangersSmall = _content.Load<SpriteFont>("bangersSmall");
            currentTime = new TimeSpan(0, 0, 0);

            pingPongBall.LoadContent(_content);
            paddle.LoadContent(_content);
            //wind.LoadContent(_content);

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
                    if (currentTime > highScoreTime) highScoreTime = currentTime;
                    ScreenManager.RemoveAddScreen(this, new LostMenuScreen(highScoreTime), null);
                }
            }

            pingPongBall.Update(gameTime);
            paddle.Update(gameTime);
            //wind.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(new Color(60, 100, 245, 255));

            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            pingPongBall.Draw(gameTime, spriteBatch);
            paddle.Draw(gameTime, spriteBatch);
            //wind.Draw(gameTime, spriteBatch);

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
