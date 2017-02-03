#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge;

namespace Welt.API
{
    public struct ItemStack
    {
        public BlockDescriptor Block;
        public byte Count;
        
        public ItemStack(BlockDescriptor block, byte count = 1)
        {
            Block = block;
            Count = count;
        }

        public bool TryMerge(ref ItemStack stack)
        {
            if (stack.Block.Id != Block.Id && stack.Block.Metadata != Block.Metadata && Block.Id != 0) return false;
            byte size = 64; // TODO determine this
            if (stack.Count + Count > size)
            {
                if (stack.Count + Count > size*2) return false;
                var stackCount = (byte) (stack.Count + Count - size);
                Count = size;
                stack = new ItemStack(Block, stackCount);
                return true;
            }
            stack.Count += Count;
            return true;
        }
    }
}