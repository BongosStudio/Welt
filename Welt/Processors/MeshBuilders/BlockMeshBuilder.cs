using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Forge;
using Welt.Types;

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
                case BlockType.RED_FLOWER:
                    return PlantBuilder.BuildPlantVertexList;
                case BlockType.LONG_GRASS:
                    return GrassBuilder.BuildGrassVertexList;
                default:
                    return UniformCubeBuilder.BuildBlockVertexList;
            }
        }

        protected static void AddVertex(Chunk chunk, ushort blockType, Vector3I blockPosition, Vector3I chunkRelativePosition,
            Vector3 vertexAdd, Vector3 normal, Vector2 uv1, float sunLight, Color localLight)
        {
            var effect = BlockEffect.None;

            switch (blockType)
            {
                case BlockType.WATER:
                    effect = BlockEffect.LightLiquidEffect;
                    break;
                case BlockType.RED_FLOWER:
                case BlockType.LONG_GRASS:
                case BlockType.LEAVES:
                    effect = BlockEffect.VegetationWindEffect;
                    break;
            }

            if (!Block.IsTransparentBlock(blockType))
                chunk.PrimaryVertexList.Add(new VertexPositionTextureLightEffect(
                    (Vector3)blockPosition + vertexAdd, uv1, sunLight,
                    localLight.ToVector3(), effect));
            else
                chunk.SecondaryVertexList.Add(new VertexPositionTextureLightEffect(
                    (Vector3)blockPosition + vertexAdd, uv1, sunLight,
                    localLight.ToVector3(), effect));
        }

        #region AddIndex

        protected static void AddIndex(Chunk chunk, ushort blockType, short i1, short i2, short i3, short i4, short i5,
            short i6)
        {
            if (!Block.IsTransparentBlock(blockType))
            {
                chunk.PrimaryIndexList.Add((short)(chunk.PrimaryVertexCount + i1));
                chunk.PrimaryIndexList.Add((short)(chunk.PrimaryVertexCount + i2));
                chunk.PrimaryIndexList.Add((short)(chunk.PrimaryVertexCount + i3));
                chunk.PrimaryIndexList.Add((short)(chunk.PrimaryVertexCount + i4));
                chunk.PrimaryIndexList.Add((short)(chunk.PrimaryVertexCount + i5));
                chunk.PrimaryIndexList.Add((short)(chunk.PrimaryVertexCount + i6));
                chunk.PrimaryVertexCount += 4;
            }
            else
            {
                chunk.SecondaryIndexList.Add((short)(chunk.SecondaryVertexCount + i1));
                chunk.SecondaryIndexList.Add((short)(chunk.SecondaryVertexCount + i2));
                chunk.SecondaryIndexList.Add((short)(chunk.SecondaryVertexCount + i3));
                chunk.SecondaryIndexList.Add((short)(chunk.SecondaryVertexCount + i4));
                chunk.SecondaryIndexList.Add((short)(chunk.SecondaryVertexCount + i5));
                chunk.SecondaryIndexList.Add((short)(chunk.SecondaryVertexCount + i6));
                chunk.SecondaryVertexCount += 4;
            }
        }

        #endregion
    }
}
