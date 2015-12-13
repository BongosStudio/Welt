#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Welt.Profiling
{
    /// <summary>
    /// Class used for performance testing of framerates.
    /// </summary>
    public class FrameRateCounter : DrawableGameComponent
    {

        #region Fields

        readonly ContentManager _content;
        SpriteBatch _spriteBatch;
        SpriteFont _spriteFont;
        readonly string[] _numbers;

        int _frameRate;
        int _frameCounter;
        TimeSpan _elapsedTime = TimeSpan.Zero;
        #endregion

        /// <summary>
        /// Constructor initializes the numbers array for garbage free strings later.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public FrameRateCounter(Game game)
            : base(game)
        {
            _content = game.Content;
            _numbers = new string[10];
            for (var j = 0; j < 10; j++)
            {
                _numbers[j] = j.ToString();
            }
        }

        /// <summary>
        /// Loads the spritebatch and font needed to draw the framerate to screen.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = _content.Load<SpriteFont>("Fonts/OSDDisplay");
        }

        #region Update
        /// <summary>
        /// The framerate is calculated in this method.  It actually calculates
        /// the update rate, but when fixed time step and syncronize with retrace 
        /// are turned off, it is the same value.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime <= TimeSpan.FromSeconds(1)) return;
            _elapsedTime -= TimeSpan.FromSeconds(1);
            _frameRate = _frameCounter;
            _frameCounter = 0;
        }
        #endregion

        #region Draw
        /// <summary>
        /// Draws the framerate to screen with a shadow outline to make it easy
        /// to see in any game.
        /// </summary>
        /// <param name="gameTime"></param>
        public new void Draw(GameTime gameTime)
        {
            _frameCounter++;

            //Framerates over 1000 aren't important as we have lots of room for features.
            if (_frameRate >= 1000)
            {
                _frameRate = 999;
            }

            //Break the framerate down to single digit components so we can use
            //the number lookup to draw them.
            var fps1 = _frameRate / 100;
            var fps2 = (_frameRate - fps1 * 100) / 10;
            var fps3 = _frameRate - fps1 * 100 - fps2 * 10;

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont, _numbers[fps1], new Vector2(33, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, _numbers[fps1], new Vector2(32, 32), Color.White);

            _spriteBatch.DrawString(_spriteFont, _numbers[fps2], new Vector2(33 + _spriteFont.MeasureString(_numbers[fps1]).X, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, _numbers[fps2], new Vector2(32 + _spriteFont.MeasureString(_numbers[fps1]).X, 32), Color.White);

            _spriteBatch.DrawString(_spriteFont, _numbers[fps3], new Vector2(33 + _spriteFont.MeasureString(_numbers[fps1]).X + _spriteFont.MeasureString(_numbers[fps2]).X, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, _numbers[fps3], new Vector2(32 + _spriteFont.MeasureString(_numbers[fps1]).X + _spriteFont.MeasureString(_numbers[fps2]).X, 32), Color.White);

            _spriteBatch.End();
        }
        #endregion

    }
}
