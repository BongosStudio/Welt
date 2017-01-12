#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

namespace Welt.API.Forge
{
    public interface IBlockPalette
    {
        Block GetBlock(uint x, uint y, uint z);
        void SetBlock(uint x, uint y, uint z, Block value);

        ushort GetId(uint x, uint y, uint z);
        void SetId(uint x, uint y, uint z, ushort value);

        byte GetMetadata(uint x, uint y, uint z);
        void SetMetadata(uint x, uint y, uint z, byte value);

        byte GetRLight(uint x, uint y, uint z);
        void SetRLight(uint x, uint y, uint z, byte value);

        byte GetGLight(uint x, uint y, uint z);
        void SetGLight(uint x, uint y, uint z, byte value);

        byte GetBLight(uint x, uint y, uint z);
        void SetBLight(uint x, uint y, uint z, byte value);

        byte[] ToBinary();
        bool IsEmpty();
    }
}