using System;
using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Forge;
using Welt.Types;
using Welt.Models;
using System.Diagnostics;
using System.Linq;

namespace Welt.Processors.MeshBuilders
{
    public delegate void BlockVertexBuilder(ushort id, Chunk chunk, Vector3I chunkRelativePosition);
    public class BlockMeshBuilder
    {
        protected const byte MAX_SUN_VALUE = 15;
        private static object m_Lock = new object();
        public static BlockVertexBuilder GetVertexBuilder(ushort id)
        {
            switch (id)
            {
                case BlockType.TORCH:
                    return TorchBuilder.BuildPostVertexList;
                case BlockType.FLOWER_ROSE:
                    return PlantBuilder.BuildPlantVertexList;
                case BlockType.LONG_GRASS:
                    return GrassBuilder.BuildGrassVertexList;
                case BlockType.SNOW:
                    return SnowCapBuilder.BuildBlockVertexList;
                case BlockType.LADDER:
                    return LadderBuilder.BuildLadderVertexList;
                default:
                    return UniformCubeBuilder.BuildBlockVertexList;
            }
        }

        protected static void AddPlane(Chunk chunk, ushort blockType, Vector3I blockPosition, Vector3I chunkRelPos, 
            BlockFaceDirection face, float[] sun, Color[] local, Vector3[] vadds, Vector2[] uvs, short[] ins)
        {
            if (vadds.Length != uvs.Length) throw new ArgumentException("vadds and uvs must be same size");

            var effect = BlockEffect.None;

            switch (blockType)
            {
                case BlockType.WATER:
                    effect = BlockEffect.LightLiquidEffect;
                    break;
                case BlockType.FLOWER_ROSE:
                case BlockType.LONG_GRASS:
                case BlockType.LEAVES:
                    effect = BlockEffect.VegetationWindEffect;
                    break;
            }
            var overlay = BlockProvider.GetProvider(blockType).GetOverlay(face);
            var vertices = new VertexPositionTextureLightEffect[vadds.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new VertexPositionTextureLightEffect(
                    vadds[i] + (Vector3)blockPosition,
                    uvs[i], overlay, sun[i], local[i].ToVector3(), effect);
            }
            if (!Block.IsOpaqueBlock(blockType))
            {
                lock (chunk)
                {
                    chunk.PrimaryVertexList.AddRange(vertices);
                    chunk.PrimaryIndexList.AddRange(ins.Select(ii => (short)(ii + chunk.PrimaryVertexCount)));
                    
                    //chunk.PrimaryPlaneIndex.Add(
                    //    FastMath.Get4DHash((int)chunkRelPos.X, (int)chunkRelPos.Y, (int)chunkRelPos.Z, (int)face),
                    //    (chunk.PrimaryVertexCount, vertices.Length));
                    chunk.PrimaryVertexCount += vertices.Length;
                }
            }
            else
            {
                lock (chunk)
                {
                    chunk.SecondaryVertexList.AddRange(vertices);
                    chunk.SecondaryIndexList.AddRange(ins.Select(ii => (short)(ii + chunk.SecondaryVertexCount)));
                    //chunk.SecondaryPlaneIndex.Add(
                    //    FastMath.Get4DHash((int)chunkRelPos.X, (int)chunkRelPos.Y, (int)chunkRelPos.Z, (int)face),
                    //    (chunk.SecondaryVertexCount, vertices.Length));
                    chunk.SecondaryVertexCount += vertices.Length;
                }
            }
            
        }
    }
}
