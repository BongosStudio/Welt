#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System.Diagnostics;

#endregion

namespace Welt.Forge
{
    public enum BlockType : byte
    {
        None = 0,
        Dirt,
        Grass,
        Lava,
        Leaves,
        Rock,
        Sand,
        Tree,
        Water,
        Snow,
        RedFlower,
        LongGrass,
        MAXIMUM
    }

    #region Block structure

    public struct Block
    {
        public byte R, G, B;
        public byte Sun;
        public BlockType Type;

        public Block(BlockType blockType)
        {
            Type = blockType;
            Sun = 0;
            R = 0;
            G = 0;
            B = 0;
        }
    }

    public struct LowMemBlock
    {
        //blocktype + light amount stored in one byte 
        private byte store;

        public LowMemBlock(BlockType blockType, bool sunlit)
        {
            if (sunlit) store = (byte) (((byte) blockType << 4) | 15);
            else store = (byte) (((byte) blockType << 4) | 0);
        }

        public LowMemBlock(BlockType blockType, byte lightAmount)
        {
            store = (byte) (((byte) blockType << 4) | lightAmount);
        }

        public BlockType Type
        {
            get { return (BlockType) (store >> 4); }
            set { store = (byte) (((byte) value << 4) | (store & 0x0F)); }
        }

        public byte LightAmount
        {
            get { return (byte) (store & 0x0F); }
            set { store = (byte) (store | value); }
        }

        public bool Solid
        {
            get
            {
                if (Type == BlockType.None || Type == BlockType.Leaves || Type == BlockType.RedFlower)
                {
                    return false;
                }
                return true;
            }
        }

        public void debug(string s)
        {
            Debug.WriteLine("[" + s + "] -> " + store + " : " + Type + ", " + LightAmount);
        }
    }

    #endregion
}