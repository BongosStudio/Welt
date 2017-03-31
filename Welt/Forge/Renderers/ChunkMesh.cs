using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Welt.Graphics;
using Welt.API;
using Welt.Annotations;

namespace Welt.Forge.Renderers
{
    public class ChunkMesh : Mesh<VertexPositionNormalTextureEffect>
    {
        /// <summary>
        ///     Returns the chunk the mesh analyzes.
        /// </summary>
        public ReadOnlyChunk Chunk { get; set; }

        /// <summary>
        ///     Creates a chunk mesh from the supplied vertices and indices.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="device"></param>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        public ChunkMesh(ReadOnlyChunk chunk, WeltGame game, [NotNull]VertexPositionNormalTextureEffect[] vertices, [NotNull]short[] indices)
            : base(game, 1, true)
        {
            Chunk = chunk;
            Vertices = vertices;
            SetSubmesh(0, indices);
            BoundingBox = RecalculateBounds(null);
        }

        /// <summary>
        ///     Creates a chunk mesh from the supplied vertices and separated indices.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="device"></param>
        /// <param name="vertices"></param>
        /// <param name="opaqueIndices"></param>
        /// <param name="transparentIndices"></param>
        public ChunkMesh(ReadOnlyChunk chunk, WeltGame game, [NotNull]VertexPositionNormalTextureEffect[] vertices, [NotNull]short[] opaqueIndices, [NotNull]short[] transparentIndices)
            : base(game, 2, true)
        {
            Chunk = chunk;
            Vertices = vertices;
            SetSubmesh(0, opaqueIndices);
            SetSubmesh(1, transparentIndices);
            BoundingBox = RecalculateBounds(null);
        }

        protected override BoundingBox RecalculateBounds(VertexPositionNormalTextureEffect[] vertices)
        {
            return new BoundingBox(Chunk.GetPosition(), (Vector3)Chunk.GetPosition() + (Vector3)Core.Forge.Chunk.Size);
        }
    }
}
