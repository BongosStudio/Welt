using System;
using Welt.Forge;

namespace Welt.Events.Forge
{
    public class ChunkEventArgs : EventArgs
    {
        public ReadOnlyChunk Chunk { get; set; }

        public ChunkEventArgs(ReadOnlyChunk chunk)
        {
            Chunk = chunk;
        }
    }
}