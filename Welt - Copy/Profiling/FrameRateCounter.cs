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

        readonly ContentManager _mContent;
        SpriteBatch _mSpriteBatch;
        SpriteFont _mSpriteFont;
        readonly string[] _mNumbers;

        int _mFrameRate;
        int _mFrameCounter;
        TimeSpan _mElapsedTime = TimeSpan.Zero;
        #endregion

        /// <summary>
        /// Constructor initializes the numbers array for garbage free strings later.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public FrameRateCounter(Game game)
            : base(game)
        {
            _mContent = game.Content;
            _mNumbers = new string[10];
            for (var j = 0; j < 10; j++)
            {
                _mNumbers[j] = j.ToString();
            }
        }

        /// <summary>
        /// Loads the spritebatch and font needed to draw the framerate to screen.
        /// </summary>
        protected override void LoadContent()
        {
            _mSpriteBatch = new SpriteBatch(GraphicsDevice);
            _mSpriteFont = _mContent.Load<SpriteFont>("Fonts/OSDDisplay");
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
            _mElapsedTime += gameTime.ElapsedGameTime;

            if (_mElapsedTime <= TimeSpan.FromSeconds(1)) return;
            _mElapsedTime -= TimeSpan.FromSeconds(1);
            _mFrameRate = _mFrameCounter;
            _mFrameCounter = 0;
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
            _mFrameCounter++;

            //Framerates over 1000 aren't important as we have lots of room for features.
            if (_mFrameRate >= 1000)
            {
                _mFrameRate = 999;
            }

            //Break the framerate down to single digit components so we can use
            //the number lookup to draw them.
            var fps1 = _mFrameRate / 100;
            var fps2 = (_mFrameRate - fps1 * 100) / 10;
            var fps3 = _mFrameRate - fps1 * 100 - fps2 * 10;

            _mSpriteBatch.Begin();

            _mSpriteBatch.DrawString(_mSpriteFont, _mNumbers[fps1], new Vector2(33, 33), Color.Black);
            _mSpriteBatch.DrawString(_mSpriteFont, _mNumbers[fps1], new Vector2(32, 32), Color.White);

            _mSpriteBatch.DrawString(_mSpriteFont, _mNumbers[fps2], new Vector2(33 + _mSpriteFont.MeasureString(_mNumbers[fps1]).X, 33), Color.Black);
            _mSpriteBatch.DrawString(_mSpriteFont, _mNumbers[fps2], new Vector2(32 + _mSpriteFont.MeasureString(_mNumbers[fps1]).X, 32), Color.White);

            _mSpriteBatch.DrawString(_mSpriteFont, _mNumbers[fps3], new Vector2(33 + _mSpriteFont.MeasureString(_mNumbers[fps1]).X + _mSpriteFont.MeasureString(_mNumbers[fps2]).X, 33), Color.Black);
            _mSpriteBatch.DrawString(_mSpriteFont, _mNumbers[fps3], new Vector2(32 + _mSpriteFont.MeasureString(_mNumbers[fps1]).X + _mSpriteFont.MeasureString(_mNumbers[fps2]).X, 32), Color.White);

            _mSpriteBatch.End();
        }
        #endregion

    }
}
