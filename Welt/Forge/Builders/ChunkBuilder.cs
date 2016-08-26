using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Models;
using Welt.Rendering;
using Welt.Types;

namespace Welt.Forge.Builders
{
    public class ChunkBuilder
    {
        public ChunkBuilder(IChunk ownedChunk)
        {
            OwnedChunk = ownedChunk;
            Position = new Vector2(ownedChunk.X, ownedChunk.Z);
            VertexList = new List<VertexPositionTextureLight>();
            WaterVertexList = new List<VertexPositionTextureLight>();
            GrassVertexList = new List<VertexPositionTextureLight>();
            IndexList = new List<short>();
            WaterIndexList = new List<short>();
            GrassIndexList = new List<short>();
        }

        public Vector2 Position { get; }
        public IChunk OwnedChunk { get; }
        public ChunkState State { get; set; } = ChunkState.AwaitingGenerate;
        public bool CanContinue { get; set; } = true;

        public BoundingBox BoundingBox
            =>
                new BoundingBox(new Vector3(Position.X, 0, Position.Y),
                    new Vector3(Position.X + Chunk.Width, Chunk.Height, Position.Y + Chunk.Depth));
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

        public Vector3I HighestSolidBlock = new Vector3I(0, 0, 0);
        //highestNoneBlock starts at 0 so it will be adjusted. if you start at highest it will never be adjusted ! 

        public Vector3I LowestNoneBlock = new Vector3I(0, Chunk.Height, 0);
        
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