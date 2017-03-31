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
        ref List<VertexPositionNormalTextureEffect> vertices, ref List<short> indices);
    public class BlockMeshBuilder
    {
        protected const byte MAX_SUN_VALUE = 15;
        private static object m_Lock = new object();

        protected static readonly Vector3[] Normals =
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };

        protected static BlockVertexBuilder GetVertexBuilder(ushort id)
        {
            switch (id)
            {
                case BlockType.TORCH:
                    return TorchBuilder.BuildBlockVertexList;
                case BlockType.FLOWER_ROSE:
                    return PlantBuilder.BuildBlockVertexList;
                case BlockType.LONG_GRASS:
                    return GrassBuilder.BuildBlockVertexList;
                case BlockType.SNOW:
                    return SnowCapBuilder.BuildBlockVertexList;
                case BlockType.LADDER:
                    return LadderBuilder.BuildBlockVertexList;
                default:
                    return UniformCubeBuilder.BuildBlockVertexList;
            }
        }

        public static void Render(IBlockProvider provider, ReadOnlyChunk chunk, Vector3I position, ChunkRenderer.VisibleFaces faces, int vertexCount, out VertexPositionNormalTextureEffect[] vertices, out short[] indices)
        {
            var v = new List<VertexPositionNormalTextureEffect>();
            var i = new List<short>();
            GetVertexBuilder(provider.Id)(provider, chunk, position, faces, vertexCount, ref v, ref i);
            vertices = v.ToArray();
            indices = i.ToArray();
        }

        protected static void RenderMesh(IBlockProvider provider, Vector3I blockPosition, 
            Vector3[] vadds, Vector3 normal, Vector2[] uvs, short[] ins, int currentVertexCount, 
            ref List<VertexPositionNormalTextureEffect> vertices, ref List<short> indices)
        {
            for (var i = 0; i < vadds.Length; i++)
            {
                vertices.Add(new VertexPositionNormalTextureEffect(
                    vadds[i] + (Vector3)blockPosition, normal,
                    uvs[i], (uint)provider.DisplayEffect));
            }
            indices.AddRange(ins.Select(i => (short)(i + currentVertexCount)));
        }
    }
}
