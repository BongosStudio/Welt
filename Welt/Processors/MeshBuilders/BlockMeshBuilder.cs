using System;
using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Forge;
using Welt.Graphics;
using Welt.API;
using System.Linq;
using Welt.Core.Forge;
using Welt.API.Forge;
using System.Collections.Generic;
using Welt.Forge.Renderers;

namespace Welt.Processors.MeshBuilders
{
    public delegate void BlockVertexBuilder(
        IBlockProvider provider, ReadOnlyChunk chunk, Vector3I chunkRelativePosition, 
        ChunkRenderer.VisibleFaces faces, int vertexCount, 
        ref List<VertexPositionTextureLightEffect> vertices, ref List<short> indices);
    public class BlockMeshBuilder
    {
        protected const byte MAX_SUN_VALUE = 15;
        private static object m_Lock = new object();
        protected static BlockVertexBuilder GetVertexBuilder(ushort id)
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

        public static void Render(IBlockProvider provider, ReadOnlyChunk chunk, Vector3I position, ChunkRenderer.VisibleFaces faces, int vertexCount, out VertexPositionTextureLightEffect[] vertices, out short[] indices)
        {
            var v = new List<VertexPositionTextureLightEffect>();
            var i = new List<short>();
            GetVertexBuilder(provider.Id)(provider, chunk, position, faces, vertexCount, ref v, ref i);
            vertices = v.ToArray();
            indices = i.ToArray();
        }

        protected static void RenderMesh(IBlockProvider provider, Vector3I blockPosition,
            float[] sun, Color[] local, Vector3[] vadds, Vector2[] uvs, short[] ins, int currentVertexCount, 
            ref List<VertexPositionTextureLightEffect> vertices, ref List<short> indices)
        {
            for (var i = 0; i < vadds.Length; i++)
            {
                vertices.Add(new VertexPositionTextureLightEffect(
                    vadds[i] + (Vector3)blockPosition,
                    uvs[i], new Vector4(), sun[i], local[i].ToVector3(), provider.DisplayEffect));
            }
            indices.AddRange(ins.Select(i => (short)(i + currentVertexCount)));
        }
    }
}
