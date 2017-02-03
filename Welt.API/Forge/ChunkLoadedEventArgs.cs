using Microsoft.Xna.Framework;
using System;

namespace Welt.API.Forge
{
    public class ChunkLoadedEventArgs : EventArgs
    {
        public Vector3I Coordinates { get; set; }
        public IChunk Chunk { get; set; }

        public ChunkLoadedEventArgs(IChunk chunk)
        {
            Chunk = chunk;
            Coordinates = chunk.Index;
        }
    }
}

