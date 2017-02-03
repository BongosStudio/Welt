using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Welt.API;

namespace Welt.Core
{
    public class TextureMap
    {
        private static Dictionary<string, Vector2[][]> m_UvMappings = new Dictionary<string, Vector2[][]>();
        private const int TEXTURE_ATLAS = 16;

        public Texture2D LoadBlockTextures(GraphicsDevice graphics, string directory)
        {
            #region Block Textures
            var i = new Vector2(0);
            var files = Directory.EnumerateFiles(directory, "*.png");
            var d = (float)Math.Ceiling(Math.Sqrt(files.Count()));
            var texture = new Bitmap((int)d * TEXTURE_ATLAS + TEXTURE_ATLAS, (int)d * TEXTURE_ATLAS + TEXTURE_ATLAS);
            var final = Graphics.FromImage(texture);
            foreach (var file in files)
            {
                var name = file.Replace(".png", "").Split('\\').Last();
                
                using (var image = (Bitmap)Image.FromFile(file))
                {
                    final.DrawImage(image, i.X*TEXTURE_ATLAS, i.Y*TEXTURE_ATLAS, TEXTURE_ATLAS, TEXTURE_ATLAS);
                }

                #region UV Mappings

                var ofs = TEXTURE_ATLAS / final.VisibleClipBounds.Width;

                var yOfs = i.Y * ofs;
                var xOfs = i.X * ofs;

                var uvList = new[]
                {
                    new Vector2[]
                        {
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs)
                        },
                        new Vector2[]
                        {
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs),
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs),
                            new Vector2(xOfs, yOfs + ofs)
                        },
                        new Vector2[]
                        {
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs)
                        },
                        new Vector2[]
                        {
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs)
                        },
                        new Vector2[]
                        {
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs),
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs),
                            new Vector2(xOfs, yOfs + ofs)
                        },
                        new Vector2[]
                        {
                            new Vector2(xOfs, yOfs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs, yOfs + ofs),
                            new Vector2(xOfs + ofs, yOfs),
                            new Vector2(xOfs + ofs, yOfs + ofs)
                        }
                    };

                #endregion

                m_UvMappings.Add(name, uvList);
                Debug.WriteLine($"Generated texture {name}:{i}");
                if (i.X < d)
                    i.X++;
                else
                {
                    i.Y++;
                    i.X = 0;
                }
            }
            final.Save();
            using (var stream = new MemoryStream())
            {
                texture.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                return Texture2D.FromStream(graphics, stream);
            }

            #endregion
            
        }

        public Texture2D LoadTexture(GraphicsDevice graphics, string name)
        {
            using (var stream = new FileStream(name, FileMode.Open))
            {
                return Texture2D.FromStream(graphics, stream);
            }
        }

        public static Vector2[] GetTexture(string name, BlockFaceDirection face)
        {
            var uvs = m_UvMappings[name][(int)face];
            return uvs;
        }
    }
}
