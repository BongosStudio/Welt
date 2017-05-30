#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Welt.API.Forge
{
    public interface IWorld : IEnumerable<IChunk>
    {
        string Name { get; }
        long Seed { get; }
        int Size { get; }
        Vector3B Position { get; }
        int TimeOfDay { get; set; }
        Vector2 SpawnPoint { get; set; }
        WorldType WorldType { get; }
        Action<object, ChunkLoadedEventArgs> ChunkGenerated { get; set; }
        Action<object, ChunkLoadedEventArgs> ChunkLoaded { get; set; }
        Action<object, BlockChangeEventArgs> BlockChanged { get; set; }

        IChunk ChunkAt(Vector3I coordinates, bool generate = false);
        Vector3I FindBlockPosition(Vector3I worldCoords, out IChunk chunk, bool generate = true);

        Block GetBlock(Vector3I position);
        Block SetBlock(Vector3I position, Block block);
        BlockDescriptor GetBlockData(Vector3I position);

        IChunk GetChunk(Vector3I position, bool generate);
        void SetChunk(Vector3I position, IChunk value);

        bool IsValidPosition(Vector3I position);
    }
}