using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Blocks;

namespace Welt.Graphics
{
    public class TextureMap
    {
        public static Texture2D Texture { get; private set; }
        private static Dictionary<string, Vector2[][]> m_UvMappings = new Dictionary<string, Vector2[][]>();
        private const int TEXTURE_ATLAS = 64;

        public static void LoadTextures(GraphicsDevice graphics, string directory)
        {
            var i = new Vector2(0);
            var data = new List<byte>();
            var files = Directory.EnumerateFiles(directory, "*.png");
            var d = (int)Math.Ceiling(Math.Sqrt(files.Count()));
            var texture = new Bitmap(d * TEXTURE_ATLAS, d * TEXTURE_ATLAS);
            var count = 0;
            foreach (var file in files)
            {
                //images.Add(file.Replace(".png", ""), (Bitmap)Image.FromFile(file));
                var name = file.Replace(".png", "");
                var ofs = 1f / TEXTURE_ATLAS;

                var yOfs = (int) (i.Y * ofs);
                var xOfs = (int) (i.X * ofs);
                using (var image = (Bitmap)Image.FromFile(file))
                {
                    for (var x = 0; x < TEXTURE_ATLAS; ++x)
                    {
                        for (var y = 0; y < TEXTURE_ATLAS; ++y)
                        {
                            // This does not assign correct X and Y coords to texture.
                            // work on tomorrow .3.
                            //data.Add(image.Value.GetPixel(x, y));
                            texture.SetPixel(xOfs + x, yOfs + y, image.GetPixel(x, y));
                        }
                    }
                }

                    #region UV Mappings

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
                Debug.WriteLine($"Assigning {name} to {i}");
                if (i.X < d)
                    i.X++;
                else
                {
                    i.Y++;
                    i.X = 0;
                }
                count++;
            }

            // enumerate the images, stitch, and add the UVs to local data
            //var data = new List<System.Drawing.Color>();
            //foreach (var image in images)
            //{
            //    var ofs = 1f / TEXTURE_ATLAS;

            //    var yOfs = i.Y * ofs;
            //    var xOfs = i.X * ofs;

            //    for (var x = 0; x < TEXTURE_ATLAS; ++x)
            //    {
            //        for (var y = 0; y < TEXTURE_ATLAS; ++y)
            //        {
            //            data.Add(image.Value.GetPixel(x, y));
            //            //texture.SetPixel((int) i.X + x, (int) i.Y + y, image.Value.GetPixel(x, y));
            //        }
            //    }

            //    #region UV Mappings

            //    var uvList = new[]
            //    {
            //        new Vector2[]
            //        {
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs)
            //        },
            //        new Vector2[]
            //        {
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs + ofs)
            //        },
            //        new Vector2[]
            //        {
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs)
            //        },
            //        new Vector2[]
            //        {
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs)
            //        },
            //        new Vector2[]
            //        {
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs + ofs)
            //        },
            //        new Vector2[]
            //        {
            //            new Vector2(xOfs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs, yOfs + ofs),
            //            new Vector2(xOfs + ofs, yOfs),
            //            new Vector2(xOfs + ofs, yOfs + ofs)
            //        }
            //    };

            //    #endregion

            //    m_UvMappings.Add(image.Key, uvList);
            //}
            //Texture.SetData(data.ToArray());
            using (var stream = new FileStream("test.png", FileMode.Create))
            {
                texture.Save(stream, ImageFormat.Png);
                //Texture.SaveAsPng(stream, d * TEXTURE_ATLAS, d * TEXTURE_ATLAS);
            }
        }
    }
}
