using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Welt.Core;

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

        public SpriteFont Font;

        private TextureMap m_TextureMap;

        public GraphicsManager(WeltGame game)
        {
            Game = game;
            m_TextureMap = new TextureMap();
        }

        public void Initialize()
        {
            BlockTexture = m_TextureMap.LoadBlockTextures(Game.GraphicsDevice, "resources\\textures\\blocks");
            CloudTexture = m_TextureMap.LoadTexture(Game.GraphicsDevice, "resources\\textures\\environment\\clouds.png");
            StarTexture = m_TextureMap.LoadTexture(Game.GraphicsDevice, "resources\\textures\\environment\\stars.jpg");
            Font = Game.Content.Load<SpriteFont>("Fonts\\console");
            SunTexture = m_TextureMap.LoadTexture(Game.GraphicsDevice, "resources\\textures\\environment\\sun.png");
            MoonTexture = m_TextureMap.LoadTexture(Game.GraphicsDevice, "resources\\textures\\environment\\moon.png");
        }
    }
}
