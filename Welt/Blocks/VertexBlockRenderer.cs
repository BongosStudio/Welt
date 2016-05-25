#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Welt.Forge;
using Welt.Types;

namespace Welt.Blocks
{

    /*
     * 
     * 
     * **************************************************************************************************************
     * 
     * 
     *    deprecated - but before deleting pull out the small optimisations of Vector.UnitX and the likes from BuildFaceVertices
     *        
     * 
     * **************************************************************************************************************
     * 
     * 
     */


    public class VertexBlockRenderer
    {
        private readonly World _mWorld;

        private static readonly Vector3 _vector101 = new Vector3(1, 0, 1);
        private static readonly Vector3 _vector110 = new Vector3(1, 1, 0);
        private static readonly Vector3 _vector011 = new Vector3(0, 1, 1);

        public VertexBlockRenderer(World world)
        {
            this._mWorld = world;
        }

        #region BuildBlockVertices
        /// <summary>
        /// BuildBlockVertices making a block
        /// TODO surrounding block faces for digging ?
        /// <param name="block">block to build</param>
        /// <param name="blockPosition"> in viewableWorld coordinates already offset with current chunk position  </param>         
        /// </summary>
        public void BuildBlockVertices(ref List<VertexPositionTextureShade> vertexList, Block block, Chunk chunk, Vector3I chunkRelativePosition)
        {
            //optimized by using chunk.Blocks[][][] except for "out of current chunk" blocks

            var blockPosition = chunk.Position + chunkRelativePosition;

            Block blockXDecreasing, blockXIncreasing, blockYDecreasing, blockYIncreasing, blockZDecreasing, blockZIncreasing;

            if (chunkRelativePosition.X == 0 ||
                chunkRelativePosition.Y == 0 ||
                chunkRelativePosition.Z == 0 ||
                chunkRelativePosition.X == Chunk.Max.X ||
                chunkRelativePosition.Y == Chunk.Max.Y ||
                chunkRelativePosition.Z == Chunk.Max.Z)
            {
                blockXDecreasing = _mWorld.GetBlock(blockPosition.X - 1, blockPosition.Y, blockPosition.Z);
                blockYDecreasing = _mWorld.GetBlock(blockPosition.X, blockPosition.Y - 1, blockPosition.Z);
                blockZDecreasing = _mWorld.GetBlock(blockPosition.X, blockPosition.Y, blockPosition.Z - 1);
                blockXIncreasing = _mWorld.GetBlock(blockPosition.X + 1, blockPosition.Y, blockPosition.Z);
                blockYIncreasing = _mWorld.GetBlock(blockPosition.X, blockPosition.Y + 1, blockPosition.Z);
                blockZIncreasing = _mWorld.GetBlock(blockPosition.X, blockPosition.Y, blockPosition.Z + 1);
            }
            else
            {
                //blockXDecreasing = chunk.Blocks[chunkRelativePosition.X - 1, chunkRelativePosition.Y, chunkRelativePosition.Z];
                blockXDecreasing = chunk.Blocks[(chunkRelativePosition.X - 1) * Chunk.FlattenOffset + chunkRelativePosition.Z * Chunk.Size.Y + chunkRelativePosition.Y];

                //blockXIncreasing = chunk.Blocks[chunkRelativePosition.X + 1, chunkRelativePosition.Y, chunkRelativePosition.Z];
                blockXIncreasing = chunk.Blocks[(chunkRelativePosition.X + 1) * Chunk.FlattenOffset + chunkRelativePosition.Z * Chunk.Size.Y + chunkRelativePosition.Y];

                //blockYDecreasing = chunk.Blocks[chunkRelativePosition.X, chunkRelativePosition.Y - 1, chunkRelativePosition.Z];
                blockYDecreasing = chunk.Blocks[chunkRelativePosition.X * Chunk.FlattenOffset + chunkRelativePosition.Z * Chunk.Size.Y + (chunkRelativePosition.Y - 1)];

                //blockYIncreasing = chunk.Blocks[chunkRelativePosition.X, chunkRelativePosition.Y + 1, chunkRelativePosition.Z];
                blockYIncreasing = chunk.Blocks[chunkRelativePosition.X * Chunk.FlattenOffset + chunkRelativePosition.Z * Chunk.Size.Y + (chunkRelativePosition.Y + 1)];

                //blockZDecreasing = chunk.Blocks[chunkRelativePosition.X, chunkRelativePosition.Y, chunkRelativePosition.Z - 1];
                blockZDecreasing = chunk.Blocks[chunkRelativePosition.X * Chunk.FlattenOffset + (chunkRelativePosition.Z - 1) * Chunk.Size.Y + chunkRelativePosition.Y];

                //blockZIncreasing = chunk.Blocks[chunkRelativePosition.X, chunkRelativePosition.Y, chunkRelativePosition.Z + 1];
                blockZIncreasing = chunk.Blocks[chunkRelativePosition.X * Chunk.FlattenOffset + (chunkRelativePosition.Z + 1) * Chunk.Size.Y + chunkRelativePosition.Y];
            }

            if (!Block.IsSolidBlock(blockXDecreasing.Id)) BuildFaceVertices(ref vertexList, blockPosition, BlockFaceDirection.XDecreasing, block.Id);
            if (!Block.IsSolidBlock(blockXIncreasing.Id)) BuildFaceVertices(ref vertexList, blockPosition, BlockFaceDirection.XIncreasing, block.Id);

            if (!Block.IsSolidBlock(blockYDecreasing.Id)) BuildFaceVertices(ref vertexList, blockPosition, BlockFaceDirection.YDecreasing, block.Id);
            if (!Block.IsSolidBlock(blockYIncreasing.Id)) BuildFaceVertices(ref vertexList, blockPosition, BlockFaceDirection.YIncreasing, block.Id);

            if (!Block.IsSolidBlock(blockZDecreasing.Id)) BuildFaceVertices(ref vertexList, blockPosition, BlockFaceDirection.ZDecreasing, block.Id);
            if (!Block.IsSolidBlock(blockZIncreasing.Id)) BuildFaceVertices(ref vertexList, blockPosition, BlockFaceDirection.ZIncreasing, block.Id);
        }
        #endregion

        #region BuildFaceVertices
        public void BuildFaceVertices(ref List<VertexPositionTextureShade> vertexList, Vector3I blockPosition, BlockFaceDirection faceDir, ushort blockType)
        {
            var texture = Block.GetTexture(blockType, faceDir);

            //Debug.WriteLine(string.Format("BuildBlockVertices ({0},{1},{2}) : {3} ->{4} :", x, y, z, faceDir, texture));

            var faceIndex = 0;
            switch (faceDir)
            {
                case BlockFaceDirection.XIncreasing:
                    faceIndex = 0;
                    break;
                case BlockFaceDirection.XDecreasing:
                    faceIndex = 1;
                    break;
                case BlockFaceDirection.YIncreasing:
                    faceIndex = 2;
                    break;
                case BlockFaceDirection.YDecreasing:
                    faceIndex = 3;
                    break;
                case BlockFaceDirection.ZIncreasing:
                    faceIndex = 4;
                    break;
                case BlockFaceDirection.ZDecreasing:
                    faceIndex = 5;
                    break;
            }

            var uvList = TextureHelper.UvMappings[(int)texture * 6 + faceIndex];

            float light = 2;//TODO light hardcoded to 2

            switch (faceDir)
            {
                case BlockFaceDirection.XIncreasing:
                    {
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.One, Vector3.UnitX, light, uvList[0]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector110, Vector3.UnitX, light, uvList[1]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector101, Vector3.UnitX, light, uvList[2]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector101, Vector3.UnitX, light, uvList[3]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector110, Vector3.UnitX, light, uvList[4]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitX, Vector3.UnitX, light, uvList[5]));
                    }
                    break;

                case BlockFaceDirection.XDecreasing:
                    {
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitY, Vector3.Left, light, uvList[0]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector011, Vector3.Left, light, uvList[1]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitZ, Vector3.Left, light, uvList[2]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitY, Vector3.Left, light, uvList[3]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitZ, Vector3.Left, light, uvList[4]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.Zero, Vector3.Left, light, uvList[5]));
                    }
                    break;

                case BlockFaceDirection.YIncreasing:
                    {
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitY, Vector3.UnitY, light, uvList[0]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector110, Vector3.UnitY, light, uvList[1]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.One, Vector3.UnitY, light, uvList[2]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitY, Vector3.UnitY, light, uvList[3]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.One, Vector3.UnitY, light, uvList[4]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector011, Vector3.UnitY, light, uvList[5]));
                    }
                    break;

                case BlockFaceDirection.YDecreasing:
                    {
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector101, Vector3.Down, light, uvList[0]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitX, Vector3.Down, light, uvList[1]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitZ, Vector3.Down, light, uvList[2]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitZ, Vector3.Down, light, uvList[3]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitX, Vector3.Down, light, uvList[4]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.Zero, Vector3.Down, light, uvList[5]));
                    }
                    break;

                case BlockFaceDirection.ZIncreasing:
                    {
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector011, Vector3.UnitZ, light, uvList[0]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.One, Vector3.UnitZ, light, uvList[1]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector101, Vector3.UnitZ, light, uvList[2]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector011, Vector3.UnitZ, light, uvList[3]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector101, Vector3.UnitZ, light, uvList[4]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitZ, Vector3.UnitZ, light, uvList[5]));
                    }
                    break;

                case BlockFaceDirection.ZDecreasing:
                    {
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, _vector110, Vector3.Forward, light, uvList[0]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitY, Vector3.Forward, light, uvList[1]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitX, Vector3.Forward, light, uvList[2]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitX, Vector3.Forward, light, uvList[3]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.UnitY, Vector3.Forward, light, uvList[4]));
                        vertexList.Add(ToVertexPositionTextureShade(blockPosition, Vector3.Zero, Vector3.Forward, light, uvList[5]));
                    }
                    break;
            }
        }
        #endregion

        private VertexPositionTextureShade ToVertexPositionTextureShade(Vector3I blockPosition, Vector3 vertexAdd, Vector3 normal, float light, Vector2 uv)
        {
            return new VertexPositionTextureShade((Vector3) blockPosition + vertexAdd, normal, light, uv);
        }

    }
}
