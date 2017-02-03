using Microsoft.Xna.Framework.Graphics;

namespace Welt.Graphics
{
    public class GraphicsManager
    {
        public WeltGame Game;
        public Texture2D BlockTexture;
        public Texture2D CloudTexture;
        public Texture2D StarTexture;
        public Texture2D SunTexture;
        public Texture2D MoonTexture;
        public Texture2D ParticleTexture;

        private TextureMap m_TextureMap;

        public GraphicsManager(WeltGame game)
        {
            Game = game;
            m_TextureMap = new TextureMap();
        }

        public void Initialize()
        {
            BlockTexture = m_TextureMap.LoadBlockTextures(Game.GraphicsDevice, "textures\\blocks");
            CloudTexture = m_TextureMap.LoadTexture(Game.GraphicsDevice, "textures\\environment\\clouds.png");
            StarTexture = m_TextureMap.LoadTexture(Game.GraphicsDevice, "textures\\environment\\stars.jpg");
            SunTexture = m_TextureMap.LoadTexture(Game.GraphicsDevice, "textures\\environment\\sun.png");
        }
    }
}
