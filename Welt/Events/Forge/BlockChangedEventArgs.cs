using System;
using Welt.Forge;
using Welt.API;

namespace Welt.Events.Forge
{
    public class BlockChangedEventArgs : EventArgs
    {
        public ushort Id { get; }
        public Vector3I Position { get; }
        public ReadOnlyChunk Chunk { get; }
        
        public BlockChangedEventArgs(ushort id, Vector3I position, ReadOnlyChunk chunk)
        {
            Id = id;
            Position = position;
            Chunk = chunk;
        }
    }
}
