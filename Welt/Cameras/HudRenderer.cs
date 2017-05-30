#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Forge;
using Welt.Core.Forge;
using Welt.API.Forge;

#endregion

namespace Welt.Cameras
{
    public class HudRenderer
    {
        public HudRenderer(GraphicsDevice device, ReadOnlyWorld world, PlayerRenderer playerRenderer)
        {
            m_GraphicsDevice = device;
            PlayerRenderer = playerRenderer;
            this.World = world;
        }

        #region Initialize

        public void Initialize()
        {
            // Used for crosshair sprite/texture at the moment
            m_SpriteBatch = new SpriteBatch(m_GraphicsDevice);

            #region Minimap
            
            
            #endregion
        }

        #endregion

        public void LoadContent(ContentManager content)
        {
            // Crosshair
            m_CrosshairTexture = content.Load<Texture2D>("Textures\\crosshair");
            m_CrosshairMovingTexture = content.Load<Texture2D>("Textures\\crosshair_moving");
        }

        #region generateMinimapTexture

        public void GenerateMinimapTexture()
        {
            
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            // Draw the crosshair
            if (PlayerRenderer.Player.IsPaused) return;
            m_SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            m_SpriteBatch.Draw(m_CrosshairTexture,
                new Vector2(
                    m_GraphicsDevice.Viewport.Width/2 - m_CrosshairTexture.Width/2,
                    m_GraphicsDevice.Viewport.Height/2 - m_CrosshairTexture.Height/2), Color.White);
            m_SpriteBatch.End();

            #region minimap

            if (!ShowMinimap) return;

            #endregion
        }

        #endregion

        #region Fields

        #region minimap

        // Minimap
        

        #endregion

        private readonly GraphicsDevice m_GraphicsDevice;
        public readonly PlayerRenderer PlayerRenderer;
        public readonly ReadOnlyWorld World;

        public bool ShowMinimap = false;

        // Crosshair
        private Texture2D m_CrosshairTexture;
        private Texture2D m_CrosshairMovingTexture;
        private SpriteBatch m_SpriteBatch;

        #endregion
    }
}