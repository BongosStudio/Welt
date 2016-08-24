using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.API.Forge;
using Welt.Blocks;
using Welt.Types;
using static Welt.Console.ThrowHelper;
using static Welt.Forge.BlockTextureModel;


namespace Welt.Forge.Builders
{
    /// <summary>
    ///     Builds the UV mappings and stitches the textures together if said textures
    ///     are detached.
    /// </summary>
    public static class TextureBuilder
    {
        private static bool _initialized;
        private const int TEXTURE_ATLAS = 16;
        // TODO: change this texture atlas size. Make it dynamic because this shit ain't gonna be static.
        // perhaps do a Texture2D.Height/16 = Y and Texture2D.Width/16 = X?

        /// <summary>
        ///     Initializes the builder and reads all textures used for the game. Should be called 
        ///     during Game.LoadContent.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) Throw(new InvalidOperationException("Texture builder has already been called"));

            #region Create BlockTextureModels

            /*foreach (var btm in BlockTextureModel.Created)
            {
                // whatever we have to do to stitch the textures into a map. Idk how we should do that yet.
            }

            foreach (var block in Block.RegisteredTypes)
            {
                // look for any files of any sides. ie: stone_top_left, stone, stone_bottom_top, stone_sides, etc.
                // valid names are top, bottom, front, back, left, right, sides
            }
            */

            #endregion


            _initialized = true;
        }

        public static IEnumerable<Vector2> CreateTexture(ushort id, byte md, Mesh mesh)
        {
            var btm = Find(id, md);
            var v = new List<Vector2>();

            if (mesh.IsParentMesh())
            {
                foreach (var submesh in mesh.Submeshes)
                {
                    v.AddRange(CreateTexture(id, md, submesh));
                }
                return v;
            }

            switch (mesh.Face)
            {
                case BlockFaceDirection.XIncreasing:
                    return GetUvMapping(btm.XIncreasingTexture, BlockFaceDirection.XIncreasing);
                case BlockFaceDirection.XDecreasing:
                    return GetUvMapping(btm.XDecreasingTexture, BlockFaceDirection.XDecreasing);
                case BlockFaceDirection.ZIncreasing:
                    return GetUvMapping(btm.ZIncreasingTexture, BlockFaceDirection.ZIncreasing);
                case BlockFaceDirection.ZDecreasing:
                    return GetUvMapping(btm.ZDecreasingTexture, BlockFaceDirection.ZDecreasing);
                case BlockFaceDirection.YIncreasing:
                    return GetUvMapping(btm.YIncreasingTexture, BlockFaceDirection.YIncreasing);
                case BlockFaceDirection.YDecreasing:
                    return GetUvMapping(btm.YDecreasingTexture, BlockFaceDirection.YDecreasing);
                default:
                    throw new ArgumentException("Invalid face supplied for " + nameof(mesh));
            }
        }

        private static IEnumerable<Vector2> GetUvMapping(int texture, BlockFaceDirection face)
        {

            var y = texture/TEXTURE_ATLAS;
            var x = texture%TEXTURE_ATLAS;

            const float ofs = 0.0625F; // 1f/TEXTURE_ATLAS

            var yOfs = y*ofs;
            var xOfs = x*ofs;
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
    }
}
 