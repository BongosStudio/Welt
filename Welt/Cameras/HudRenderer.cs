#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Forge;

#endregion

namespace Welt.Cameras
{
    public class HudRenderer
    {
        public HudRenderer(GraphicsDevice device, World world, FirstPersonCamera camera)
        {
            _graphicsDevice = device;
            Camera = camera;
            this.World = world;
        }

        #region Initialize

        public void Initialize()
        {
            // Used for crosshair sprite/texture at the moment
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            #region Minimap

            _spriteBatchmap = new SpriteBatch(_graphicsDevice);
            _minimapTex = new Texture2D(_graphicsDevice, 1, 1);
            var texcol = new Color[1];
            _minimapTex.GetData(texcol);
            texcol[0] = Color.White;
            _minimapTex.SetData(texcol);

            #endregion
        }

        #endregion

        public void LoadContent(ContentManager content)
        {
            // Crosshair
            _crosshairTexture = content.Load<Texture2D>("Textures\\crosshair");
        }

        #region generateMinimapTexture

        public void GenerateMinimapTexture()
        {
            var x = (uint) Camera.Position.X;
            var z = (uint) Camera.Position.Z;

            var cx = x/Chunk.SIZE.X;
            var cz = z/Chunk.SIZE.Z;

            var chunk = World.Chunks[cx, cz];

            for (var xx = 0; xx < Chunk.SIZE.X; xx++)
            {
                for (var zz = 0; zz < Chunk.SIZE.Z; zz++)
                {
                    var offset = xx*Chunk.FlattenOffset + zz*Chunk.SIZE.Y;
                    for (int y = Chunk.MAX.Y; y > 0; y--)
                    {
                        var blockcheck = chunk.Blocks[offset + y].Type;
                        if (blockcheck == BlockType.None) continue;
                        var index = xx*(Chunk.SIZE.X) + zz;
                        switch (blockcheck)
                        {
                            case BlockType.Grass:
                                _maptexture[index] = new Color(0, y, 0);
                                break;
                            case BlockType.Dirt:
                                _maptexture[index] = Color.Khaki;
                                break;
                            case BlockType.Snow:
                                _maptexture[index] = new Color(y, y, y);
                                break;
                            case BlockType.Sand:
                                _maptexture[index] = new Color(193 + (y/2), 154 + (y/2), 107 + (y/2));
                                break;
                            case BlockType.Water:
                                _maptexture[index] = new Color(0, 0, y + 64);
                                break;
                            case BlockType.Leaves:
                                _maptexture[index] = new Color(0, 128, 0);
                                break;
                            default:
                                _maptexture[index] = new Color(0, 0, 0);
                                break;
                        }
                        y = 0;
                    }
                }
            }
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            // Draw the crosshair
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(_crosshairTexture, new Vector2(
                (_graphicsDevice.Viewport.Width/2) - 10,
                (_graphicsDevice.Viewport.Height/2) - 10), Color.White);
            _spriteBatch.End();

            #region minimap

            if (ShowMinimap)
            {
                GenerateMinimapTexture();
                _spriteBatchmap.Begin();
                for (var i = 0; i < 16; i++)
                {
                    for (var j = 0; j < 16; j++)
                    {
                        _blockPos.X = i*8 + 650;
                        _blockPos.Y = j*8 + 20;
                        _spriteBatchmap.Draw(_minimapTex, _blockPos, _maptexture[i*Chunk.SIZE.X + j]);
                    }
                }
                _spriteBatchmap.End();
            }

            #endregion
        }

        #endregion

        #region Fields

        #region minimap

        // Minimap
        private SpriteBatch _spriteBatchmap;
        private Texture2D _minimapTex;
        private Color _minimapBgCol = new Color(150, 150, 150, 150);
        private readonly Color[] _maptexture = new Color[Chunk.SIZE.X*Chunk.SIZE.Z];
        private Rectangle _minimapBgRect = new Rectangle(650, 20, 64, 64);
        private Rectangle _blockPos = new Rectangle(0, 0, 8, 8);

        #endregion

        private readonly GraphicsDevice _graphicsDevice;
        public readonly FirstPersonCamera Camera;
        public readonly World World;

        public bool ShowMinimap = false;

        // Crosshair
        private Texture2D _crosshairTexture;
        private SpriteBatch _spriteBatch;

        #endregion
    }
}