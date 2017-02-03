using System;

namespace Welt.API.Forge
{
    public class BlockChangeEventArgs : EventArgs
    {
        public BlockChangeEventArgs(Vector3I position, BlockDescriptor oldBlock, BlockDescriptor newBlock)
        {
            Position = position;
            OldBlock = oldBlock;
            NewBlock = newBlock;
        }

        public Vector3I Position;
        public BlockDescriptor OldBlock;
        public BlockDescriptor NewBlock;
    }
}