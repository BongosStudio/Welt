#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.API.Forge;
using Welt.Forge;

#endregion

namespace Welt.Types
{
    public struct PositionedBlock
    {
        public readonly Vector3I Position;
        public readonly (ushort Id, byte Metadata) Block;

        public PositionedBlock(Vector3I position, (ushort Id, byte Metadata) block)
        {
            this.Position = position;
            this.Block = block;
        }


    }
}
