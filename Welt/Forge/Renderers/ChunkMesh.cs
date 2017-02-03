using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Graphics;

namespace Welt.Forge.Renderers
{
    public class ChunkMesh : Mesh<VertexPositionTextureLightEffect>
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyChunk Chunk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="device"></param>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        public ChunkMesh(ReadOnlyChunk chunk, WeltGame game, VertexPositionTextureLightEffect[] vertices, int[] indices)
            : base(game, 1, true)
        {
            Chunk = chunk;
            Vertices = vertices;
            SetSubmesh(0, indices);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="device"></param>
        /// <param name="vertices"></param>
        /// <param name="opaqueIndices"></param>
        /// <param name="transparentIndices"></param>
        public ChunkMesh(ReadOnlyChunk chunk, WeltGame game, VertexPositionTextureLightEffect[] vertices, int[] opaqueIndices, int[] transparentIndices)
            : base(game, 2, true)
        {
            Chunk = chunk;
            Vertices = vertices;
            SetSubmesh(0, opaqueIndices);
            SetSubmesh(1, transparentIndices);
        }
    }
}
