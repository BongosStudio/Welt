using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Game.Extensions;
using Welt.Logic.Forge;
using Welt.Models;
using Welt.Rendering;

namespace Welt.Forge.Builders
{
    public class ChunkBuilder
    {
        public ChunkBuilder(Chunk ownedChunk)
        {
            OwnedChunk = ownedChunk;
            Position = new Vector2(ownedChunk.X, ownedChunk.Z);
            VertexList = new List<VertexPositionTextureLight>();
            WaterVertexList = new List<VertexPositionTextureLight>();
            GrassVertexList = new List<VertexPositionTextureLight>();
            IndexList = new List<short>();
            WaterIndexList = new List<short>();
            GrassIndexList = new List<short>();
            SolidBlockMap = new BitArray(Chunk.Width*Chunk.Depth*ownedChunk.Height);

            ownedChunk.BlockAdded += HandleBlockAdjusted;
            ownedChunk.BlockRemoved += HandleBlockAdjusted;
        }

        public Vector2 Position { get; }
        public IChunk OwnedChunk { get; }
        public ChunkState State { get; set; } = ChunkState.AwaitingGenerate;
        public bool CanContinue { get; set; } = true;

        public BoundingBox BoundingBox
            => new BoundingBox(new Vector3(Position.X, 0, Position.Y),
                    new Vector3(Position.X + Chunk.Width, OwnedChunk.Height, Position.Y + Chunk.Depth));

        public VertexBuffer VertexBuffer;
        public VertexBuffer WaterVertexBuffer;
        public VertexBuffer GrassVertexBuffer;

        public IndexBuffer IndexBuffer;
        public IndexBuffer WaterIndexBuffer;
        public IndexBuffer GrassIndexBuffer;

        public List<short> IndexList;
        public List<short> WaterIndexList;
        public List<short> GrassIndexList;

        public List<VertexPositionTextureLight> VertexList;
        public List<VertexPositionTextureLight> WaterVertexList;
        public List<VertexPositionTextureLight> GrassVertexList;

        public short VertexCount;
        public short WaterVertexCount;
        public short GrassVertexCount;

        public BitArray SolidBlockMap;
        public LightTable LightTable;

        private void HandleBlockAdjusted(object sender, BlockChangedEventArgs args)
        {
            var block = OwnedChunk.GetBlock(args.X, args.Y, args.Z);
            var isSolid = BlockLogic.IsTransparentBlock(block.Id);
            SolidBlockMap[args.X*(Chunk.Width*Chunk.Depth) + args.Z*Chunk.Depth + args.Y] = isSolid;
        }

        public void CreateBlockMap()
        {
            (ushort Id, byte Metadata) block;
            for (var x = 0; x < Chunk.Width; ++x)
            {
                for (var z = 0; z < Chunk.Depth; ++z)
                {
                    for (var y = 0; y < OwnedChunk.Height; ++y)
                    {
                        block = OwnedChunk.GetBlock(x, y, z);
                        SolidBlockMap[x*(Chunk.Width*Chunk.Depth) + z*Chunk.Depth + y] = ForgeExtensions.IsBlockOpen(block.Id);
                    }
                }
            }
        }

        public void Clear()
        {
            VertexBuffer?.Dispose();
            WaterVertexBuffer?.Dispose();
            GrassVertexBuffer?.Dispose();
            IndexBuffer?.Dispose();
            WaterIndexBuffer?.Dispose();
            GrassIndexBuffer?.Dispose();
            IndexList?.Clear();
            WaterIndexList?.Clear();
            GrassIndexList?.Clear();
            VertexList?.Clear();
            WaterVertexList?.Clear();
            GrassVertexList?.Clear();
        }
    }
}