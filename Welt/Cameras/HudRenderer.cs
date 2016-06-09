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
            _mGraphicsDevice = device;
            PlayerRenderer = playerRenderer;
            this.World = world;
        }

        #region Initialize

        public void Initialize()
        {
            // Used for crosshair sprite/texture at the moment
            _mSpriteBatch = new SpriteBatch(_mGraphicsDevice);

            #region Minimap

            _mSpriteBatchmap = new SpriteBatch(_mGraphicsDevice);
            _mMinimapTex = new Texture2D(_mGraphicsDevice, 1, 1);
            var texcol = new Color[1];
            _mMinimapTex.GetData(texcol);
            texcol[0] = Color.White;
            _mMinimapTex.SetData(texcol);
            GenerateMinimapTexture();
            #endregion
        }

        #endregion

        public void LoadContent(ContentManager content)
        {
            // Crosshair
            _mCrosshairTexture = content.Load<Texture2D>("Textures\\crosshair");
            _mCrosshairMovingTexture = content.Load<Texture2D>("Textures\\crosshair_moving");
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
                    var index = xx * Chunk.Size.X + zz;
                    switch (blockcheck)
                    {
                        case BlockType.GRASS:
                            _mMaptexture[index] = new Color(0, y, 0);
                            break;
                        case BlockType.DIRT:
                            _mMaptexture[index] = Color.Khaki;
                            break;
                        case BlockType.SNOW:
                            _mMaptexture[index] = new Color(y, y, y);
                            break;
                        case BlockType.SAND:
                            _mMaptexture[index] = new Color(193 + y / 2, 154 + y / 2, 107 + y / 2);
                            break;
                        case BlockType.WATER:
                            _mMaptexture[index] = new Color(0, 0, y + 64);
                            break;
                        case BlockType.LEAVES:
                            _mMaptexture[index] = new Color(0, 128, 0);
                            break;
                        default:
                            _mMaptexture[index] = new Color(0, 0, 0);
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
            if (PlayerRenderer.Player.IsPaused) return;
            _mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _mSpriteBatch.Draw(PlayerRenderer.Player.Entity.IsMoving ? _mCrosshairMovingTexture : _mCrosshairTexture,
                new Vector2(
                    _mGraphicsDevice.Viewport.Width/2 - _mCrosshairTexture.Width/2,
                    _mGraphicsDevice.Viewport.Height/2 - _mCrosshairTexture.Height/2), Color.White);
            _mSpriteBatch.End();

            #region minimap

            if (!ShowMinimap) return;
            //GenerateMinimapTexture();
            _mSpriteBatchmap.Begin();
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    _mBlockPos.X = i*8 + 650;
                    _mBlockPos.Y = j*8 + 20;
                    _mSpriteBatchmap.Draw(_mMinimapTex, _mBlockPos, _mMaptexture[i*Chunk.Size.X + j]);
                }
            }
            _mSpriteBatchmap.End();

            #endregion
        }

        #endregion

        #region Fields

        #region minimap

        // Minimap
        private SpriteBatch _mSpriteBatchmap;
        private Texture2D _mMinimapTex;
        private Color _mMinimapBgCol = new Color(150, 150, 150, 150);
        private readonly Color[] _mMaptexture = new Color[Chunk.Size.X*Chunk.Size.Z];
        private Rectangle _mMinimapBgRect = new Rectangle(650, 20, 64, 64);
        private Rectangle _mBlockPos = new Rectangle(0, 0, 8, 8);

        #endregion

        private readonly GraphicsDevice _mGraphicsDevice;
        public readonly PlayerRenderer PlayerRenderer;
        public readonly World World;

        public bool ShowMinimap = false;

        // Crosshair
        private Texture2D _mCrosshairTexture;
        private Texture2D _mCrosshairMovingTexture;
        private SpriteBatch _mSpriteBatch;

        #endregion
    }
}