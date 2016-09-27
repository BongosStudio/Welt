using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Extensions;
using Welt.Forge.Builders;
using Welt.Game.Builders.Forge.Blocks;
using Welt.Game.Extensions;
using Welt.Logic.Forge;
using Welt.Rendering;

namespace Welt.Forge.Processors
{
    public class VertexProcessor : IChunkProcessor
    {
        public ProcessorStatus Status { get; private set; }

        private GraphicsDevice _graphicsDevice;
        private static IBlockMesh[] _meshInstances =
        {
            new DefaultBlockMesh(),
            new CapBlockMesh(),
            new GrassBlockMesh()
        };

        public VertexProcessor(GraphicsDevice device)
        {
            _graphicsDevice = device;
        }

        public async Task<ChunkBuilder> ProcessChunk(Chunk chunk)
        {
            Status = ProcessorStatus.Processing;
            var builder = await CreateChunkBuilder(chunk);
            await BuildVertices(builder);
            return builder;
        }
        
        private Task<ChunkBuilder> CreateChunkBuilder(Chunk chunk)
        {
            return Task.Run(() =>
            {
                var builder = new ChunkBuilder(chunk);
                builder.CreateBlockMap();
                return builder;
            });
        }

        private Task BuildVertices(ChunkBuilder builder)
        {
            return Task.Run(() =>
            {
                var yLow = builder.OwnedChunk.GetLowestNoneBlock();
                var yHigh = builder.OwnedChunk.HeightMap.Max();

                for (var x = 0; x < Chunk.Width; ++x)
                {
                    for (var z = 0; z < Chunk.Depth; ++z)
                    {
                        for (var y = yHigh; y >= yLow; --y)
                        {
                            // we'll start at the top and work our way down.
                            var b = builder.OwnedChunk.GetBlock(x, y, z);
                            if (b.Id == 0)
                                continue;
                            // TODO: change this so we don't have to hardcode in BlockLogic.
                            if (BlockLogic.IsGrassBlock(b.Id))
                                AddMeshToBuffers(builder, _meshInstances[2], b.Id, x, y, z);
                            else if (BlockLogic.IsCapBlock(b.Id))
                                AddMeshToBuffers(builder, _meshInstances[1], b.Id, x, y, z);
                            else
                                AddMeshToBuffers(builder, _meshInstances[0], b.Id, x, y, z);
                        }
                    }
                }
            });
        }

        private void AddMeshToBuffers(ChunkBuilder builder, IBlockMesh mesh, ushort id, int x, int y, int z)
        {
            if (builder.IsBlockOpen(x + 1, y, z))
                CreateTextureAndAdjustBuffers(
                    builder, mesh.Right().Vertices, mesh.Right().Indices, mesh.Right().Uvs, id, x, y, z, BlockFaceDirection.XIncreasing);
            if (builder.IsBlockOpen(x - 1, y, z))
                CreateTextureAndAdjustBuffers(
                    builder, mesh.Left().Vertices, mesh.Left().Indices, mesh.Left().Uvs, id, x, y, z, BlockFaceDirection.XDecreasing);
            if (builder.IsBlockOpen(x, y + 1, z))
                CreateTextureAndAdjustBuffers(
                    builder, mesh.Top().Vertices, mesh.Top().Indices, mesh.Top().Uvs, id, x, y, z, BlockFaceDirection.YIncreasing);
            if (builder.IsBlockOpen(x, y - 1, z))
                CreateTextureAndAdjustBuffers(
                    builder, mesh.Bottom().Vertices, mesh.Bottom().Indices, mesh.Bottom().Uvs, id, x, y, z, BlockFaceDirection.YDecreasing);
            if (builder.IsBlockOpen(x, y, z + 1))
                CreateTextureAndAdjustBuffers(
                    builder, mesh.Front().Vertices, mesh.Front().Indices, mesh.Front().Uvs, id, x, y, z, BlockFaceDirection.ZIncreasing);
            if (builder.IsBlockOpen(x, y, z - 1))
                CreateTextureAndAdjustBuffers(
                    builder, mesh.Back().Vertices, mesh.Back().Indices, mesh.Back().Uvs, id, x, y, z, BlockFaceDirection.ZDecreasing);

            var position = new Vector3(builder.OwnedChunk.X + x, y, builder.OwnedChunk.Z + z);
            
        }

        private void CreateTextureAndAdjustBuffers(
            ChunkBuilder builder, 
            Vector3[] vertices, 
            byte[] indices, 
            byte[] uvi,
            ushort id, 
            int x, int y, int z,
            // TODO: vector shifting within the block (for things like torches or flowers. All we have to do is add another Vector3 then
            // add that to the x;y;z parameters.
            BlockFaceDirection direction)
        {
            var texture = BlockLogic.GetTexture(id, direction);
            var fi = (int) direction;
            var uvs = TextureBuilder.UvMappings[(int) texture*6 + fi].Pick(uvi);
            AddVertices(builder, vertices, new Vector3(x, y, z), Vector3.Zero, uvs);
            AddIndices(builder, indices);
        }

        // TODO: sunlight
        private void AddVertices(ChunkBuilder builder, Vector3[] vertices, Vector3 position, Vector3 light, Vector2[] uv)
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                builder.VertexList.Add(new VertexPositionTextureLight(vertices[i] + position, uv[i], 0, light));
            }
        }

        private void AddIndices(ChunkBuilder builder, byte[] indices)
        {
            foreach (var i in indices)
            {
                builder.IndexList.Add((short) (builder.VertexCount + i));
            }
            builder.VertexCount +=4;
        }
    }
}
