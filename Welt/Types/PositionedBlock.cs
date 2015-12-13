#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.Forge;

#endregion

namespace Welt.Types
{
    public struct PositionedBlock
    {
        public readonly Vector3I Position;
        public readonly Block Block;

        public PositionedBlock(Vector3I position, Block block)
        {
            this.Position=position;
            this.Block = block;
        }


    }
}
