using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.Blocks;
using Welt.Core.Forge;
using Welt.Graphics;

namespace Welt.Forge.Renderers
{
    public class ChunkRenderer : Renderer<ReadOnlyChunk, VertexPositionTextureLightEffect>
    {
        [Flags]
        public enum VisibleFaces
        {
            None = 0,
            North = 1,
            South = 2,
            East = 4,
            West = 8,
            Top = 16,
            Bottom = 32,
            All = North | South | East | West | Top | Bottom
        }

        private class RenderState
        {
            public readonly List<VertexPositionTextureLightEffect> Vertices
                = new List<VertexPositionTextureLightEffect>();
            public readonly List<short> OpaqueIndices
                = new List<short>();
            public readonly List<short> TransparentIndices
                = new List<short>();
            public Dictionary<Vector3I, VisibleFaces> DrawableCoordinates
                = new Dictionary<Vector3I, VisibleFaces>();

            public void Clear()
            {
                Vertices.Clear();
                OpaqueIndices.Clear();
                TransparentIndices.Clear();
                DrawableCoordinates.Clear();
            }
        }

        private void AddBottomBlock(Vector3I coords, RenderState state, ReadOnlyChunk chunk)
        {
            var desiredFaces = VisibleFaces.None;
            if (coords.X == 0)
                desiredFaces |= VisibleFaces.West;
            else if (coords.X == Chunk.Width - 1)
                desiredFaces |= VisibleFaces.East;
            if (coords.Z == 0)
                desiredFaces |= VisibleFaces.North;
            else if (coords.Z == Chunk.Depth - 1)
                desiredFaces |= VisibleFaces.South;
            if (coords.Y == 0)
                desiredFaces |= VisibleFaces.Bottom;
            else if (coords.Y == Chunk.Depth - 1)
                desiredFaces |= VisibleFaces.Top;
            state.DrawableCoordinates.TryGetValue(coords, out var faces);
            faces |= desiredFaces;
            state.DrawableCoordinates[coords] = desiredFaces;
        }



        protected override bool TryRender(ReadOnlyChunk item, out Mesh<VertexPositionTextureLightEffect> result)
        {
            
        }
    }
}
