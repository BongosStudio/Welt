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

        readonly ContentManager m_content;
        SpriteBatch m_spriteBatch;
        SpriteFont m_spriteFont;
        readonly string[] m_numbers;

        int m_frameRate;
        int m_frameCounter;
        TimeSpan m_elapsedTime = TimeSpan.Zero;
        #endregion

        /// <summary>
        /// Constructor initializes the numbers array for garbage free strings later.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public FrameRateCounter(Game game)
            : base(game)
        {
            m_content = game.Content;
            m_numbers = new string[10];
            for (var j = 0; j < 10; j++)
            {
                m_numbers[j] = j.ToString();
            }
        }

        /// <summary>
        /// Loads the spritebatch and font needed to draw the framerate to screen.
        /// </summary>
        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_spriteFont = m_content.Load<SpriteFont>("Fonts/OSDDisplay");
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
            m_elapsedTime += gameTime.ElapsedGameTime;

            if (m_elapsedTime <= TimeSpan.FromSeconds(1)) return;
            m_elapsedTime -= TimeSpan.FromSeconds(1);
            m_frameRate = m_frameCounter;
            m_frameCounter = 0;
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
            m_frameCounter++;

            //Framerates over 1000 aren't important as we have lots of room for features.
            if (m_frameRate >= 1000)
            {
                m_frameRate = 999;
            }

            //Break the framerate down to single digit components so we can use
            //the number lookup to draw them.
            var fps1 = m_frameRate / 100;
            var fps2 = (m_frameRate - fps1 * 100) / 10;
            var fps3 = m_frameRate - fps1 * 100 - fps2 * 10;

            m_spriteBatch.Begin();

            m_spriteBatch.DrawString(m_spriteFont, m_numbers[fps1], new Vector2(33, 33), Color.Black);
            m_spriteBatch.DrawString(m_spriteFont, m_numbers[fps1], new Vector2(32, 32), Color.White);

            m_spriteBatch.DrawString(m_spriteFont, m_numbers[fps2], new Vector2(33 + m_spriteFont.MeasureString(m_numbers[fps1]).X, 33), Color.Black);
            m_spriteBatch.DrawString(m_spriteFont, m_numbers[fps2], new Vector2(32 + m_spriteFont.MeasureString(m_numbers[fps1]).X, 32), Color.White);

            m_spriteBatch.DrawString(m_spriteFont, m_numbers[fps3], new Vector2(33 + m_spriteFont.MeasureString(m_numbers[fps1]).X + m_spriteFont.MeasureString(m_numbers[fps2]).X, 33), Color.Black);
            m_spriteBatch.DrawString(m_spriteFont, m_numbers[fps3], new Vector2(32 + m_spriteFont.MeasureString(m_numbers[fps1]).X + m_spriteFont.MeasureString(m_numbers[fps2]).X, 32), Color.White);

            m_spriteBatch.End();
        }
        #endregion

    }
}
