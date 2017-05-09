using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Components
{
    public class FpsComponent : IVisualComponent
    {
        public WeltGame Game { get; }

        public GraphicsDevice Graphics { get; }

        private SpriteBatch m_SpriteBatch;
        private int m_FrameRate = 0;
        private int m_FrameCounter = 0;
        private TimeSpan m_Elapsed = TimeSpan.Zero;

        public FpsComponent(WeltGame game)
        {
            Game = game;
            Graphics = game.GraphicsDevice;
        }

        public void Dispose()
        {

        }

        public void Draw(GameTime gameTime)
        {
            m_FrameCounter++;
            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(Game.GraphicsManager.Font, $"FPS: {m_FrameRate}", new Vector2(10, 10), Color.Yellow);
            m_SpriteBatch.End();
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {
            m_SpriteBatch = new SpriteBatch(Graphics);
        }

        public void Update(GameTime gameTime)
        {
            m_Elapsed += gameTime.ElapsedGameTime;
            if (m_Elapsed <= TimeSpan.FromSeconds(1)) return;
            m_Elapsed -= TimeSpan.FromSeconds(1);
            m_FrameRate = m_FrameCounter;
            m_FrameCounter = 0;
        }
    }
}
