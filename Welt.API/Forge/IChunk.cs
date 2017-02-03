#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
using Microsoft.Xna.Framework;

namespace Welt.API.Forge
{
    public interface IChunk
    {
        BoundingBox BoundingBox { get; }
        Vector3I Index { get; }
        Vector3I Position { get; }
        byte MaxHeight { get; }
        bool IsModified { get; }
        bool IsLit { get; }
        bool IsTerrainPopulated { get; }

        void SetBlock(byte x, byte y, byte z, Block b);
        Block GetBlock(byte x, byte y, byte z);

        byte GetHeight(byte x, byte z);

        void SetBlockId(byte x, byte y, byte z, ushort id);
    }
}