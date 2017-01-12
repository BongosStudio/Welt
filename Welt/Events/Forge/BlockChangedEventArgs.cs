using System;
using System.Collections.Generic;
using System.Linq;
using Welt.Forge;
using Welt.Models;
using Welt.Types;

namespace Welt.Events.Forge
{
    public class BlockChangedEventArgs : EventArgs
    {
        public ushort Id { get; }
        public Vector3I Position { get; }
        public Chunk Chunk { get; }
        
        public BlockChangedEventArgs(ushort id, Vector3I position, Chunk chunk)
        {
            Id = id;
            Position = position;
            Chunk = chunk;
        }
    }
}
