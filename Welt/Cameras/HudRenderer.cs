#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Controllers;
using Welt.Forge;
using Welt.Models;

#endregion

namespace Welt.Cameras
{
    public class HudRenderer
    {
        public HudRenderer(GraphicsDevice device, World world, PlayerRenderer playerRenderer)
        {
            m_graphicsDevice = device;
            PlayerRenderer = playerRenderer;
            this.World = world;
        }

        #region Initialize

        public void Initialize()
        {
            // Used for crosshair sprite/texture at the moment
            m_spriteBatch = new SpriteBatch(m_graphicsDevice);

            #region Minimap

            m_spriteBatchmap = new SpriteBatch(m_graphicsDevice);
            m_minimapTex = new Texture2D(m_graphicsDevice, 1, 1);
            var texcol = new Color[1];
            m_minimapTex.GetData(texcol);
            texcol[0] = Color.White;
            m_minimapTex.SetData(texcol);
            GenerateMinimapTexture();
            #endregion
        }

        #endregion

        public void LoadContent(ContentManager content)
        {
            // Crosshair
            m_crosshairTexture = content.Load<Texture2D>("Textures\\crosshair");
            m_crosshairMovingTexture = content.Load<Texture2D>("Textures\\crosshair_moving");
        }

        #region generateMinimapTexture

        public void GenerateMinimapTexture()
        {
            var x = (uint) PlayerRenderer.Camera.Position.X;
            var z = (uint) PlayerRenderer.Camera.Position.Z;

            var cx = x/Chunk.Size.X;
            var cz = z/Chunk.Size.Z;

            var chunk = World.Chunks[cx, cz];

            for (var xx = 0; xx < Chunk.Size.X; xx++)
            {
                for (var zz = 0; z < Chunk.Size.Z; z++)
                {
                    var y = chunk.HeightMap[xx, zz];
                    var blockcheck = chunk.Blocks[xx*Chunk.FlattenOffset + zz*Chunk.Size.Y + y].Id;
                    var index = xx * (Chunk.Size.X) + zz;
                    switch (blockcheck)
                    {
                        case BlockType.Grass:
                            m_maptexture[index] = new Color(0, y, 0);
                            break;
                        case BlockType.Dirt:
                            m_maptexture[index] = Color.Khaki;
                            break;
                        case BlockType.Snow:
                            m_maptexture[index] = new Color(y, y, y);
                            break;
                        case BlockType.Sand:
                            m_maptexture[index] = new Color(193 + (y / 2), 154 + (y / 2), 107 + (y / 2));
                            break;
                        case BlockType.Water:
                            m_maptexture[index] = new Color(0, 0, y + 64);
                            break;
                        case BlockType.Leaves:
                            m_maptexture[index] = new Color(0, 128, 0);
                            break;
                        default:
                            m_maptexture[index] = new Color(0, 0, 0);
                            break;
                    }
                }
            }

            //for (var xx = 0; xx < Chunk.Size.X; xx++)
            //{
            //    for (var zz = 0; zz < Chunk.Size.Z; zz++)
            //    {
            //        var offset = xx*Chunk.FlattenOffset + zz*Chunk.Size.Y;
            //        for (int y = Chunk.Max.Y; y > 0; y--)
            //        {
            //            var blockcheck = chunk.Blocks[offset + y].Id;
            //            if (blockcheck == BlockType.None) continue;
            //            var index = xx*(Chunk.Size.X) + zz;
            //            switch (blockcheck)
            //            {
            //                case BlockType.Grass:
            //                    m_maptexture[index] = new Color(0, y, 0);
            //                    break;
            //                case BlockType.Dirt:
            //                    m_maptexture[index] = Color.Khaki;
            //                    break;
            //                case BlockType.Snow:
            //                    m_maptexture[index] = new Color(y, y, y);
            //                    break;
            //                case BlockType.Sand:
            //                    m_maptexture[index] = new Color(193 + (y/2), 154 + (y/2), 107 + (y/2));
            //                    break;
            //                case BlockType.Water:
            //                    m_maptexture[index] = new Color(0, 0, y + 64);
            //                    break;
            //                case BlockType.Leaves:
            //                    m_maptexture[index] = new Color(0, 128, 0);
            //                    break;
            //                default:
            //                    m_maptexture[index] = new Color(0, 0, 0);
            //                    break;
            //            }
            //            y = 0;
            //        }
            //    }
            //}
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            // Draw the crosshair
            m_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            m_spriteBatch.Draw(PlayerRenderer.Player.IsMoving ? m_crosshairMovingTexture : m_crosshairTexture,
                new Vector2(
                    (m_graphicsDevice.Viewport.Width/2) - m_crosshairTexture.Width/2,
                    (m_graphicsDevice.Viewport.Height/2) - m_crosshairTexture.Height/2), Color.White);
            m_spriteBatch.End();

            #region minimap

            if (!ShowMinimap) return;
            //GenerateMinimapTexture();
            m_spriteBatchmap.Begin();
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    m_blockPos.X = i*8 + 650;
                    m_blockPos.Y = j*8 + 20;
                    m_spriteBatchmap.Draw(m_minimapTex, m_blockPos, m_maptexture[i*Chunk.Size.X + j]);
                }
            }
            m_spriteBatchmap.End();

            #endregion
        }

        #endregion

        #region Fields

        #region minimap

        // Minimap
        private SpriteBatch m_spriteBatchmap;
        private Texture2D m_minimapTex;
        private Color m_minimapBgCol = new Color(150, 150, 150, 150);
        private readonly Color[] m_maptexture = new Color[Chunk.Size.X*Chunk.Size.Z];
        private Rectangle m_minimapBgRect = new Rectangle(650, 20, 64, 64);
        private Rectangle m_blockPos = new Rectangle(0, 0, 8, 8);

        #endregion

        private readonly GraphicsDevice m_graphicsDevice;
        public readonly PlayerRenderer PlayerRenderer;
        public readonly World World;

        public bool ShowMinimap = false;

        // Crosshair
        private Texture2D m_crosshairTexture;
        private Texture2D m_crosshairMovingTexture;
        private SpriteBatch m_spriteBatch;

        #endregion
    }
}