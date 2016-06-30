#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Linq;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class BlockPalette : IBlockPalette
    {
        private readonly Block[] _palette;
        private readonly byte[] _indexed;

        private const int FLATTEN_OFFSET = Chunk.DEPTH*16;

        public BlockPalette()
        {
            _palette = new Block[byte.MaxValue]; // TODO: maybe we should change the size? With light
                                                 // being custom, this may be a problem in the future
            _indexed = new byte[Chunk.WIDTH*Chunk.DEPTH*16];
        }

        public Block GetBlock(uint x, uint y, uint z)
        {
            return _palette[_indexed[FlattenIndex(x, y%16, z)]];
        }

        public void SetBlock(uint x, uint y, uint z, Block value)
        {
            byte index;
            if (TryGetIndex(value, out index))
            {
                _indexed[FlattenIndex(x, y%16, z)] = index;
            }
            else
            {
                var slot = GetNextOpenSlot();
                _palette[slot] = value;
                _indexed[FlattenIndex(x, y%16, z)] = slot;
            }
        }

        public ushort GetId(uint x, uint y, uint z)
        {
            return GetBlock(x, y, z).Id;
        }

        public void SetId(uint x, uint y, uint z, ushort value)
        {
            var block = GetBlock(x, y, z);
            block.Id = value;
            SetBlock(x, y, z, block);
        }

        public byte GetMetadata(uint x, uint y, uint z)
        {
            return GetBlock(x, y, z).Metadata;
        }

        public void SetMetadata(uint x, uint y, uint z, byte value)
        {
            var block = GetBlock(x, y, z);
            block.Metadata = value;
            SetBlock(x, y, z, block);
        }

        public byte GetRLight(uint x, uint y, uint z)
        {
            return GetBlock(x, y, z).R;
        }

        public void SetRLight(uint x, uint y, uint z, byte value)
        {
            var block = GetBlock(x, y, z);
            block.R = value;
            SetBlock(x, y, z, block);
        }

        public byte GetGLight(uint x, uint y, uint z)
        {
            return GetBlock(x, y, z).G;
        }

        public void SetGLight(uint x, uint y, uint z, byte value)
        {
            var block = GetBlock(x, y, z);
            block.G = value;
            SetBlock(x, y, z, block);
        }

        public byte GetBLight(uint x, uint y, uint z)
        {
            return GetBlock(x, y, z).B;
        }

        public void SetBLight(uint x, uint y, uint z, byte value)
        {
            var block = GetBlock(x, y, z);
            block.B = value;
            SetBlock(x, y, z, block);
        }

        public byte[] ToBinary()
        {
            // the layout for the binary data of a palette will fall into such:
            // BYTE : count of blocks
            // BYTE[] _
            //         |    USHORT : block id
            //         |    BYTE : block metadata
            //         |
            //         |
        }

        public bool IsEmpty()
        {
            return _palette.All(b => b == default(Block)); 
            // we're choosing to look at `_palette` because it only has 256 objects
            // which means we can iterate through it faster. We know that if there's
            // no blocks in it, that all of `_indexed` references a default block.
        }

        private static uint FlattenIndex(uint x, uint y, uint z)
        {
            return x*FLATTEN_OFFSET + z*16 + y;
        }

        private bool TryGetIndex(Block block, out byte index)
        {
            for (byte i = 1;; i++)
            {
                if (_palette[i] == default(Block))
                {
                    index = 0;
                    return false;
                }
                if (_palette[i] == block)
                {
                    index = i;
                    return true;
                }
            }
        }

        private byte GetNextOpenSlot()
        {
            for (byte i = 1;; i++)
            {
                var b = _palette[i];
                // first check if any index this block. We need to clear as many unused slots as
                // we can
                if (_indexed.Contains(i)) continue;
                // clearly the table does, so lets see if it's now a default block 
                if (b == default(Block)) return i;
            }
        }
    }
}