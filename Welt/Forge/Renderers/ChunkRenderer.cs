using System;
using System.Collections.Generic;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Graphics;
using Welt.Processors.MeshBuilders;

namespace Welt.Forge.Renderers
{
    public class ChunkRenderer : Renderer<ReadOnlyChunk, VertexPositionNormalTextureEffect>
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

        private static readonly Vector3I[] AdjacentCoordinates =
        {
            Vector3I.Up,
            Vector3I.Down,
            Vector3I.Forward,
            Vector3I.Backward,
            Vector3I.Right,
            Vector3I.Left
        };

        private static readonly VisibleFaces[] AdjacentCoordFaces =
        {
            VisibleFaces.Bottom,
            VisibleFaces.Top,
            VisibleFaces.South,
            VisibleFaces.North,
            VisibleFaces.West,
            VisibleFaces.East
        };
        private ReadOnlyWorld World { get; set; }
        private WeltGame Game { get; set; }
        private IBlockRepository BlockRepository { get; set; }

        public ChunkRenderer(ReadOnlyWorld world, WeltGame game, IBlockRepository blockRepository)
            : base()
        {
            World = world;
            BlockRepository = blockRepository;
            Game = game;
        }

        private class RenderState
        {
            public readonly List<VertexPositionNormalTextureEffect> Vertices
                = new List<VertexPositionNormalTextureEffect>();
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
            else if (coords.Y == Chunk.Height - 1)
                desiredFaces |= VisibleFaces.Top;
            state.DrawableCoordinates.TryGetValue(coords, out var faces);
            faces |= desiredFaces;
            state.DrawableCoordinates[coords] = desiredFaces;
        }

        private void AddAdjacentBlocks(Vector3I coords, RenderState state, ReadOnlyChunk chunk)
        {
            // Add adjacent blocks
            for (int i = 0; i < AdjacentCoordinates.Length; i++)
            {
                var next = coords + AdjacentCoordinates[i];
                if (next.X < 0 || next.X >= Chunk.Width
                    || next.Y < 0 || next.Y >= Chunk.Height
                    || next.Z < 0 || next.Z >= Chunk.Depth)
                {
                    continue;
                }
                var provider = BlockRepository.GetBlockProvider(chunk.GetBlock((byte)next.X, (byte)next.Y, (byte)next.Z).Id);
                if (provider.IsOpaque)
                {
                    if (!state.DrawableCoordinates.TryGetValue(next, out VisibleFaces faces))
                        faces = VisibleFaces.None;
                    faces |= AdjacentCoordFaces[i];
                    state.DrawableCoordinates[next] = faces;
                }
            }
        }

        private void AddTransparentBlock(Vector3I coords, RenderState state, ReadOnlyChunk chunk)
        {
            // Add adjacent blocks
            VisibleFaces faces = VisibleFaces.None;
            for (int i = 0; i < AdjacentCoordinates.Length; i++)
            {
                var next = coords + AdjacentCoordinates[i];
                if (next.X < 0 || next.X >= Chunk.Width
                    || next.Y < 0 || next.Y >= Chunk.Height
                    || next.Z < 0 || next.Z >= Chunk.Depth)
                {
                    faces |= AdjacentCoordFaces[i];
                    continue;
                }
                if (chunk.GetBlock((byte)next.X, (byte)next.Y, (byte)next.Z).Id == 0)
                    faces |= AdjacentCoordFaces[i];
            }
            if (faces != VisibleFaces.None)
                state.DrawableCoordinates[coords] = faces;
        }

        private void UpdateFacesFromAdjacent(Vector3I adjacent, ReadOnlyChunk chunk,
            VisibleFaces mod, ref VisibleFaces faces)
        {
            if (chunk == null)
                return;
            var provider = BlockRepository.GetBlockProvider(chunk.GetBlock((byte)adjacent.X, (byte)adjacent.Y, (byte)adjacent.Z).Id);
            if (!provider.IsOpaque)
                faces |= mod;
        }

        private void AddChunkBoundaryBlocks(Vector3I coords, RenderState state, ReadOnlyChunk chunk)
        {
            VisibleFaces faces;
            if (!state.DrawableCoordinates.TryGetValue(coords, out faces))
                faces = VisibleFaces.None;
            VisibleFaces oldFaces = faces;

            if (coords.X == 0)
            {
                var adjacent = coords;
                adjacent.X = Chunk.Width - 1;
                var nextChunk = World.GetChunk(chunk.Chunk.Index + Vector3I.Left);
                UpdateFacesFromAdjacent(adjacent, nextChunk, VisibleFaces.West, ref faces);
            }
            else if (coords.X == Chunk.Width - 1)
            {
                var adjacent = coords;
                adjacent.X = 0;
                var nextChunk = World.GetChunk(chunk.Chunk.Index + Vector3I.Right);
                UpdateFacesFromAdjacent(adjacent, nextChunk, VisibleFaces.East, ref faces);
            }

            if (coords.Z == 0)
            {
                var adjacent = coords;
                adjacent.Z = Chunk.Depth - 1;
                var nextChunk = World.GetChunk(chunk.Chunk.Index + Vector3I.Forward);
                UpdateFacesFromAdjacent(adjacent, nextChunk, VisibleFaces.North, ref faces);
            }
            else if (coords.Z == Chunk.Depth - 1)
            {
                var adjacent = coords;
                adjacent.Z = 0;
                var nextChunk = World.GetChunk(chunk.Chunk.Index + Vector3I.Backward);
                UpdateFacesFromAdjacent(adjacent, nextChunk, VisibleFaces.South, ref faces);
            }

            if (oldFaces != faces)
                state.DrawableCoordinates[coords] = faces;
        }

        private void ProcessChunk(ReadOnlyWorld world, ReadOnlyChunk chunk, RenderState state)
        {
            state.Clear();

            for (byte x = 0; x < Chunk.Width; x++)
            {
                for (byte z = 0; z < Chunk.Depth; z++)
                {
                    for (byte y = 0; y < Chunk.Height; y++)
                    {
                        var coords = new Vector3I(x, y, z);
                        var id = chunk.GetBlock(x, y, z).Id;
                        var provider = BlockRepository.GetBlockProvider(id);
                        if (id != 0 && coords.Y == 0)
                            AddBottomBlock(coords, state, chunk);
                        if (!provider.IsOpaque)
                        {
                            AddAdjacentBlocks(coords, state, chunk);
                            if (id != 0)
                                AddTransparentBlock(coords, state, chunk);
                        }
                        else
                        {
                            if (coords.X == 0 || coords.X == Chunk.Width - 1 ||
                                coords.Z == 0 || coords.Z == Chunk.Depth - 1)
                            {
                                AddChunkBoundaryBlocks(coords, state, chunk);
                            }
                        }
                    }
                }
            }
            var enumerator = state.DrawableCoordinates.GetEnumerator();
            for (int j = 0; j <= state.DrawableCoordinates.Count; j++)
            {
                var coords = enumerator.Current;
                enumerator.MoveNext();
                var c = coords.Key;
                var descriptor = BlockDescriptor.FromBlock(chunk.GetBlock((byte)coords.Key.X, (byte)coords.Key.Y, (byte)coords.Key.Z), chunk.Chunk, coords.Key);
                
                var provider = BlockRepository.GetBlockProvider(descriptor.Id);
                BlockMeshBuilder.Render(provider, chunk, coords.Key, coords.Value, state.Vertices.Count, out var vertices, out var indices);
                state.Vertices.AddRange(vertices);
                if (provider.WillRenderOpaque)
                    state.OpaqueIndices.AddRange(indices);
                else
                    state.TransparentIndices.AddRange(indices);
            }
        }

        protected override bool TryRender(ReadOnlyChunk item, out Mesh<VertexPositionNormalTextureEffect> result)
        {
            var state = new RenderState();
            ProcessChunk(World, item, state);

            result = new ChunkMesh(item, Game, state.Vertices.ToArray(),
                state.OpaqueIndices.ToArray(), state.TransparentIndices.ToArray());

            return (result != null);
        }
    }
}
