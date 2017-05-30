using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Core.Forge.BlockProviders;
using Welt.Extensions;
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
            public readonly List<VertexPositionNormalTextureEffect> Vertices = new List<VertexPositionNormalTextureEffect>();
            public readonly List<short> OpaqueIndices = new List<short>();
            public readonly List<short> TransparentIndices = new List<short>();
            public Dictionary<Vector3I, VisibleFaces> DrawableCoordinates = new Dictionary<Vector3I, VisibleFaces>();

            public VisibleFaces GetFacesFor(Vector3I position, bool createIfNone)
            {
                if (!DrawableCoordinates.TryGetValue(position, out var faces))
                {
                    if (createIfNone)
                        DrawableCoordinates.Add(position, VisibleFaces.None);
                    faces = VisibleFaces.None;
                }
                return faces;
            }

            public void AddFacesTo(Vector3I position, VisibleFaces faces)
            {
                if (Chunk.OutOfBounds((byte)position.X, (byte)position.Y, (byte)position.Z)) return;
                if (!DrawableCoordinates.ContainsKey(position))
                    DrawableCoordinates.Add(position, faces);
                else
                    DrawableCoordinates[position] |= faces;
            }

            public void Clear()
            {
                Vertices.Clear();
                OpaqueIndices.Clear();
                TransparentIndices.Clear();
                DrawableCoordinates.Clear();
            }
        }
        
        private void ProcessChunk(ReadOnlyWorld world, ReadOnlyChunk chunk, RenderState state)
        {
            state.Clear();
            for (var x = Chunk.Width; x >= 0; x--)
            {
                for (var z = Chunk.Depth; z >= 0; z--)
                {
                    //var maxy = chunk.Chunk.GetHeight(
                    for (var y = 0; y < Chunk.Height; y++)
                    {
                        var coords = new Vector3I(x, y, z);
                        var id = chunk.GetBlock(x, y, z).Id;
                        var worldCoords = new Vector3(chunk.GetPosition().X + x, y, chunk.GetPosition().Z + z);
                        var provider = BlockRepository.GetBlockProvider(id) ?? new DefaultBlockProvider();
                        if (WillRenderFace(provider, world.GetBlockProvider(worldCoords + Vector3.UnitX)))
                            state.AddFacesTo(coords, VisibleFaces.East);
                        if (WillRenderFace(provider, world.GetBlockProvider(worldCoords - Vector3.UnitX)))
                            state.AddFacesTo(coords, VisibleFaces.West);
                        if (WillRenderFace(provider, world.GetBlockProvider(worldCoords + Vector3.UnitZ)))
                            state.AddFacesTo(coords, VisibleFaces.North);
                        if (WillRenderFace(provider, world.GetBlockProvider(worldCoords - Vector3.UnitZ)))
                            state.AddFacesTo(coords, VisibleFaces.South);
                        if (WillRenderFace(provider, world.GetBlockProvider(worldCoords + Vector3.UnitY)))
                            state.AddFacesTo(coords, VisibleFaces.Top);
                        if (WillRenderFace(provider, world.GetBlockProvider(worldCoords - Vector3.UnitY)))
                            state.AddFacesTo(coords, VisibleFaces.Bottom);
                    }
                }
            }

            var enumerator = state.DrawableCoordinates.GetEnumerator();

            VertexPositionNormalTextureEffect[] vertices;
            short[] indices;
            do
            {
                var c = enumerator.Current;
                var pos = c.Key;
                var faces = c.Value;
                var id = chunk.GetBlock((byte)pos.X, (byte)pos.Y, (byte)pos.Z).Id;
                var provider = BlockRepository.GetBlockProvider(id);
                if ((faces & VisibleFaces.Top) != 0)
                {
                    BlockMeshBuilder.Render(provider, chunk, pos, BlockFaceDirection.YIncreasing, state.Vertices.Count, out vertices, out indices);
                    AddDataToState(state, vertices, indices, provider.WillRenderOpaque);
                }
                if ((faces & VisibleFaces.Bottom) != 0)
                {
                    BlockMeshBuilder.Render(provider, chunk, pos, BlockFaceDirection.YDecreasing, state.Vertices.Count, out vertices, out indices);
                    AddDataToState(state, vertices, indices, provider.WillRenderOpaque);
                }
                if ((faces & VisibleFaces.East) != 0)
                {
                    BlockMeshBuilder.Render(provider, chunk, pos, BlockFaceDirection.XIncreasing, state.Vertices.Count, out vertices, out indices);
                    AddDataToState(state, vertices, indices, provider.WillRenderOpaque);
                }
                if ((faces & VisibleFaces.West) != 0)
                {
                    BlockMeshBuilder.Render(provider, chunk, pos, BlockFaceDirection.XDecreasing, state.Vertices.Count, out vertices, out indices);
                    AddDataToState(state, vertices, indices, provider.WillRenderOpaque);
                }
                if ((faces & VisibleFaces.North) != 0)
                {
                    BlockMeshBuilder.Render(provider, chunk, pos, BlockFaceDirection.ZIncreasing, state.Vertices.Count, out vertices, out indices);
                    AddDataToState(state, vertices, indices, provider.WillRenderOpaque);
                }
                if ((faces & VisibleFaces.South) != 0)
                {
                    BlockMeshBuilder.Render(provider, chunk, pos, BlockFaceDirection.ZDecreasing, state.Vertices.Count, out vertices, out indices);
                    AddDataToState(state, vertices, indices, provider.WillRenderOpaque);
                }
            }
            while (enumerator.MoveNext());
        }

        private bool WillRenderFace(IBlockProvider source, IBlockProvider neighbor)
        {
            if (source.Id == 0 && neighbor.Id != 0) return true;
            if (source.Id != 0 && neighbor.Id == 0) return true;
            if (source.IsOpaque != neighbor.IsOpaque)
            {
                return true;
            }
            else
            {
                if (neighbor.WillRenderSameNeighbor && (source.Id == neighbor.Id)) return true;
            }
            return false;
        }
        
        private static void AddDataToState(RenderState state, VertexPositionNormalTextureEffect[] vertices, short[] indices, bool isOpaque)
        {
            state.Vertices.AddRange(vertices);
            if (isOpaque)
                state.OpaqueIndices.AddRange(indices);
            else
                state.TransparentIndices.AddRange(indices);
        }
        
        private bool IsOpaque(ReadOnlyChunk chunk, Vector3I position)
        {
            if (Chunk.OutOfBounds((byte)position.X, (byte)position.Y, (byte)position.Z)) return false;
            var provider = BlockRepository.GetBlockProvider(chunk.GetBlock((byte)position.X, (byte)position.Y, (byte)position.Z).Id);
            return provider.IsOpaque;
        }

        protected override bool TryRender(ReadOnlyChunk item, out Mesh<VertexPositionNormalTextureEffect> result)
        {
            var state = new RenderState();
            ProcessChunk(World, item, state);

            result = new ChunkMesh(item, Game, state.Vertices.ToArray(),
                state.OpaqueIndices.ToArray(), state.TransparentIndices.ToArray());
            state.Clear();
            return (result != null);
        }
    }
}
