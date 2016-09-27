using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.API.Forge;
using Welt.Core;
using Welt.Rendering;
using Welt.Types;
using static Welt.Console.ThrowHelper;
using Welt.Core.Services;

namespace Welt.Forge.Builders
{
    /// <summary>
    ///     Builds the UV mappings and stitches the textures together if said textures
    ///     are detached.
    /// </summary>
    public static class TextureBuilder
    {
        public enum BlockTexture
        {
            None,
            Stone,
            Dirt,
            IronBlock,
            GrassSide,
            GrassTop,
            DiamondBlock,
            Lava,
            Leaves,
            Gravel,
            Clay,
            Sand,
            Snow,
            WoodTop,
            Wood,
            Water,
            Grass,
            Unknown0,
            Rose,
            StoneBrick,
            Brick,
            Torch,

            Max
        }

        public static Dictionary<int, Vector2[]> UvMappings = new Dictionary<int, Vector2[]>();

        private static Dictionary<string, Vector2> _textures; // TODO: replace Vector2 with BlockMetaDescription
        private static int _textureAtlas;
        private static Image _thaMapOfAllDemTexturez;

        private static readonly int[] _textureSizes =
        {
            16, 32, 64, 128, 256, 512, 1048, 2056
        };

        static TextureBuilder()
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes the builder and reads all textures used for the game. Should be called 
        ///     during Game.LoadContent.
        /// </summary>
        public static void Initialize()
        {
            ClearTextureMap();

            for (var i = 0; i < (int) BlockTexture.Max; ++i)
            {
                // TODO: have this automatically created because doing this by hand is tedious
                UvMappings.Add(i*6 + 0, GetTexture(i, BlockFaceDirection.XIncreasing));
                UvMappings.Add(i*6 + 1, GetTexture(i, BlockFaceDirection.XDecreasing));
                UvMappings.Add(i*6 + 2, GetTexture(i, BlockFaceDirection.YIncreasing));
                UvMappings.Add(i*6 + 3, GetTexture(i, BlockFaceDirection.YDecreasing));
                UvMappings.Add(i*6 + 4, GetTexture(i, BlockFaceDirection.ZIncreasing));
                UvMappings.Add(i*6 + 5, GetTexture(i, BlockFaceDirection.ZDecreasing));
            }
            
            // this is all commented out for future implementation. For now, we stick to what worked before so
            // we can actually get shit tested.
            #region Create BlockTextureModels

            // jesus fuck lets try this again. ALRIGHT.
            // step 1: get all the images. 
            //var images =
            //    Directory.GetFiles("Content/Textures/Blocks", "*.png", SearchOption.TopDirectoryOnly).ToArray();

            //// step 2: get the dominant image size
            //var chosenImages = new[]
            //{
            //    images[FastMath.NextRandom(images.Length)],
            //    images[FastMath.NextRandom(images.Length)],
            //    images[FastMath.NextRandom(images.Length)]
            //};

            //_textureAtlas = _textureSizes.Single(d => chosenImages.All(i => Image.FromFile(i).Width == d));

            //// step 3: get all the metas
            //var metas = Directory.GetFiles("Content/Textures/Metas", "*.wmeta", SearchOption.TopDirectoryOnly); 

            //// step 3: now that we have the images and the metas, we start piecing together _thaMapOfAllDemTexturez
            //// To figure out how many rows and collumns (as they must be the same because OCD is a bitch, we take the square root of
            //// how many images there are. 
            //// TODO: split an animated image into multiple images. This means we can't have animated images yet. Sorry.
            //var mapSize = FastMath.Ceiling(Math.Sqrt(images.Length));
            //var map = new Bitmap(mapSize*_textureAtlas, mapSize*_textureAtlas);

            //// the row
            //for (var x = 0; x < mapSize; x++)
            //{
            //    // the column
            //    for (var y = 0; y < mapSize; y++)
            //    {
            //        var vector = new Vector2(x*_textureAtlas, y*_textureAtlas);
            //        var file = images[x*mapSize + y];

            //        string name;
            //        var faces = ParseOwnedFaceTexture(file, out name); // the faces that have this texture

            //        _textures.Add(file.Split('.')[0], vector); // BUG: k not really but idk if this shit works

            //        using (var image = (Bitmap) Image.FromFile(file))
            //        {
            //            // the pixels in the image
            //            for (var ix = 0; ix < _textureAtlas; ix++)
            //            {
            //                for (var iy = 0; iy < _textureAtlas; iy++)
            //                {
            //                    map.SetPixel(x + ix, y + iy, image.GetPixel(ix, iy));
            //                }
            //            }
            //        }             
            //    }
            //}

            //_thaMapOfAllDemTexturez = map;

            #endregion
        }

        public static Vector2[] GetTexture(int texture, BlockFaceDirection face)
        {
            var ofs = 1f/_textureAtlas;

            var yOfs = texture*ofs;
            var xOfs = texture*ofs;
            var uvList = new Vector2[6];

            switch (face)
            {
                case BlockFaceDirection.XIncreasing:
                    uvList[0] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[1] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[2] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[3] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[4] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[5] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    break;
                case BlockFaceDirection.XDecreasing:
                    uvList[0] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[1] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[2] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    uvList[3] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[4] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    uvList[5] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    break;
                case BlockFaceDirection.YIncreasing:
                    uvList[0] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[1] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[2] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[3] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[4] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[5] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    break;
                case BlockFaceDirection.YDecreasing:
                    uvList[0] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[1] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[2] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[3] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[4] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[5] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    break;
                case BlockFaceDirection.ZIncreasing:
                    uvList[0] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[1] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[2] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    uvList[3] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[4] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    uvList[5] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    break;
                case BlockFaceDirection.ZDecreasing:
                    uvList[0] = new Vector2(xOfs, yOfs); // 0,0
                    uvList[1] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[2] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[3] = new Vector2(xOfs, yOfs + ofs); // 0,1
                    uvList[4] = new Vector2(xOfs + ofs, yOfs); // 1,0
                    uvList[5] = new Vector2(xOfs + ofs, yOfs + ofs); // 1,1
                    break;
            }
            return uvList;
        }
        
        private static void ClearTextureMap()
        {
            _textures = new Dictionary<string, Vector2>();
        }

        private static BlockFaceDirection[] ParseOwnedFaceTexture(string file, out string blockName)
        {
            var faces = new HashSet<BlockFaceDirection>();
            var builder = new StringBuilder();
            var hasFoundFaceDeclarations = false;
            foreach (var area in file.Split('_'))
            {
                switch (area)
                {
                    case "front":
                        faces.Add(BlockFaceDirection.ZIncreasing);
                        hasFoundFaceDeclarations = true;
                        break;
                    case "back":
                        faces.Add(BlockFaceDirection.ZDecreasing);
                        hasFoundFaceDeclarations = true;
                        break;
                    case "left":
                        faces.Add(BlockFaceDirection.XDecreasing);
                        hasFoundFaceDeclarations = true;
                        break;
                    case "right":
                        faces.Add(BlockFaceDirection.ZIncreasing);
                        hasFoundFaceDeclarations = true;
                        break;
                    case "top":
                        faces.Add(BlockFaceDirection.YIncreasing);
                        hasFoundFaceDeclarations = true;
                        break;
                    case "bottom":
                        faces.Add(BlockFaceDirection.YDecreasing);
                        hasFoundFaceDeclarations = true;
                        break;
                    case "sides":
                        faces.Add(BlockFaceDirection.XIncreasing);
                        faces.Add(BlockFaceDirection.XDecreasing);
                        faces.Add(BlockFaceDirection.ZIncreasing);
                        faces.Add(BlockFaceDirection.ZDecreasing);
                        break;
                    default:
                        if (!hasFoundFaceDeclarations) builder.Append($"_{area}");
                        break;
                }
            }

            if (faces.Count == 0)
            {
                faces = new HashSet<BlockFaceDirection>
                {
                    BlockFaceDirection.XDecreasing,
                    BlockFaceDirection.XIncreasing,
                    BlockFaceDirection.ZDecreasing,
                    BlockFaceDirection.ZIncreasing,
                    BlockFaceDirection.YDecreasing,
                    BlockFaceDirection.YIncreasing
                };
            }

            blockName = builder.ToString();
            return faces.ToArray();
        }
    }
}
 