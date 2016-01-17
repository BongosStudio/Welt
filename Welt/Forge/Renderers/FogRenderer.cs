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
        private readonly SpriteBatch _mSpriteBatch;

        public FogRenderer(GraphicsDevice device)
        {
            _mSpriteBatch = new SpriteBatch(device);
        }

        public void LoadContent()
        {
            FogTexture = WeltGame.Instance.Content.Load<Texture2D>("Textures\\cloudMap");
        }

        public void Draw()
        {
            //m_spriteBatch.Begin();
            //var rectangle = new Rectangle(0, 0, 1000, 750); // TODO: viewport
            //m_spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //m_spriteBatch.Draw(FogTexture, new Vector2(0), rectangle, new Color(Color.DarkGray, 0.01f));
            //m_spriteBatch.End();
        }
    }
}