#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class Floor : IFloor
    {
        public byte Level { get; }
        public IBlockPalette Blocks { get; }

        public Floor(byte level)
        {
            Level = level;
            Blocks = new BlockPalette();
        }
    }
}