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

            Create(1, 0, 0, 3, 0, 2); // grass block
            Create(2, 0, 1); // stone
            Create(3, 0, 16); // cobblestone
            Create(4, 0, 4); // wood planks
            Create(5, 0, 5, 5, 6); // stone slab
            Create(6, 0, 7); // bricks
            Create(7, 0, 8, 8, 9, 10); // tnt
            Create(8, 0, 11); // spooder web
            Create(9, 0, 12); // rose
            Create(9, 1, 13); // dandelion
            Create(14, 0, 14); // water
            Create(18, 0, 18); // sand
            Create(20, 0, 20, 20, 21); // wood log

            Create(39, 0, 39); // grass 
            Create(53, 0, 53); // leaves
            Create(66, 0, 66); // snow
            Create(80, 0, 80); // torch

            #endregion

            /*
            
            foreach (var btm in BlockTextureModel.Created) 
            {
                // whatever we have to do to stitch the textures into a map. Idk how we should do that yet.
            } 
             
             */
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