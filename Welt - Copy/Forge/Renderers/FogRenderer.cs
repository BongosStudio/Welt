#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Forge.Renderers
{
    public class FogRenderer
    {
        protected Texture2D FogTexture;
        private readonly SpriteBatch m_SpriteBatch;

        public FogRenderer(GraphicsDevice device)
        {
            m_SpriteBatch = new SpriteBatch(device);
        }

        public void LoadContent()
        {
            FogTexture = WeltGame.Instance.GraphicsManager.CloudTexture;
        }

        public void Draw()
        {
            //m_SpriteBatch.Begin();
            //var rectangle = new Rectangle(0, 0, 1000, 750); // TODO: viewport
            //m_SpriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //m_SpriteBatch.Draw(FogTexture, new Vector2(0), rectangle, new Color(Color.DarkGray, 0.01f));
            //m_SpriteBatch.End();
        }
    }
}