#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    /// <summary>
    ///     Contains all block information within the assigned chunk.
    /// </summary>
    public class BlockPalette
    {
        public const int FlattenOffset = Chunk.Depth*Chunk.Width;

        private (ushort Id, byte Metadata)[] _palette;
        private byte[] _indices;
        private object _syncLock = new object(); // idk if this is even needed for locking but lets implement it for now.

        public BlockPalette(Chunk chunk)
        {
            _palette = new (ushort, byte)[256];
            _indices = new byte[Chunk.Width*Chunk.Depth*chunk.Height]; 
        }

        public (ushort Id, byte Metadata) GetBlock(uint x, uint y, uint z)
        {
            var index = FlattenIndex(x, y, z);
            if (_indices[index] == 0)
                return (0, 0);
            return _palette[_indices[index]];
        }

        public bool SetBlock(uint x, uint y, uint z, ushort id, byte metadata)
        {
            //if (_palette.Length >= 256) return false; // TODO: do something better than this...?
            var pi = AssureIndexed(id, metadata);
            _indices[FlattenIndex(x, y, z)] = (byte) pi;

            var index = FlattenIndex(x, y, z);
            return true;
        }

        private int AssureIndexed(ushort id, byte metadata)
        {
            if (GetIndexOf(id, metadata, out var index))
                return index;
            var i = GetNextAvailableIndex();
            _palette[i] = (id, metadata);
            return i;
        }

        private bool GetIndexOf(ushort id, byte metadata, out int index)
        {
            for (var i = 0; i < _palette.Length; ++i)
            {
                if (_palette[i].Id == id && _palette[i].Metadata == metadata)
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }

        private int GetNextAvailableIndex()
        {
            // we start at 1 because 0 is guaranteed as air block
            for (var i = 1; i < _palette.Length; ++i)
            {
                if (_palette[i].Id == 0)
                    return i;
            }
            throw new Exception(); // TODO: better handling
        }

        private static uint FlattenIndex(uint x, uint y, uint z)
        {
            return x*FlattenOffset + z*Chunk.Depth + y;
        }
    }
}