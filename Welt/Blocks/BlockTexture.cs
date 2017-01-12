#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Welt.Blocks
{
    public enum BlockTexture
    {
        GrassTop,
        Stone,
        Dirt,
        GrassSide,
        WoodPlanks,
        StoneSlabSide,
        StoneSlabTop,
        Brick,
        TNTSide,
        TNTTop,
        TNTBottom,
        SpooderWeb,
        Rose,
        Dandelion,
        Water,
        Sapling,
        Rock,
        Obsidian,
        Sand,
        Gravel,
        TreeHorizontal,
        TreeVertical,

        MossStone = 36,
        Grass = 39,
        Leaves = 52,
        Snow = 66,
        Torch = 80,

        Lava = 225,

        Maximum
    }

    public static class TextureHelper
    {
        public const int Textureatlassize = 16;
        public static Dictionary<int, Vector2[]> UvMappings;

        static TextureHelper()
        {
            BuildUvMappings();
        }

        private static Dictionary<int, Vector2[]> BuildUvMappings()
        {
            UvMappings = new Dictionary<int, Vector2[]>();
            for (var i = 0; i < (int) BlockTexture.Maximum; i++)
            {
                UvMappings.Add((i*6), GetUvMapping(i, BlockFaceDirection.XIncreasing));
                UvMappings.Add((i*6) + 1, GetUvMapping(i, BlockFaceDirection.XDecreasing));
                UvMappings.Add((i*6) + 2, GetUvMapping(i, BlockFaceDirection.YIncreasing));
                UvMappings.Add((i*6) + 3, GetUvMapping(i, BlockFaceDirection.YDecreasing));
                UvMappings.Add((i*6) + 4, GetUvMapping(i, BlockFaceDirection.ZIncreasing));
                UvMappings.Add((i*6) + 5, GetUvMapping(i, BlockFaceDirection.ZDecreasing));
            }
            return UvMappings;
        }

        #region GetUVMapping

        public static Vector2[] GetUvMapping(int texture, BlockFaceDirection faceDir)
        {
            var textureIndex = texture;
            // Assumes a texture atlas of 8x8 textures

            var y = textureIndex/Textureatlassize;
            var x = textureIndex%Textureatlassize;

            var ofs = 1f/Textureatlassize;

            var yOfs = y*ofs;
            var xOfs = x*ofs;

            //ofs -= 0.01f;

            var uvList = new Vector2[6];

            switch (faceDir)
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

        #endregion
    }
}