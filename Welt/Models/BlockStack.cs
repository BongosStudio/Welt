#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Collections;
using System.Collections.Generic;
using Welt.Forge;

namespace Welt.Models
{
    public struct BlockStack
    {
        public Block Block;
        public byte Count;
        
        public BlockStack(Block block, byte count = 1)
        {
            Block = block;
            Count = count;
        }

        public bool TryMerge(ref BlockStack stack)
        {
            if (stack.Block != Block && Block.Id != 0) return false;
            var size = Block.GetStackSize(Block.Id);
            if (stack.Count + Count > size)
            {
                if (stack.Count + Count > size*2) return false;
                var stackCount = (byte) (stack.Count + Count - size);
                Count = size;
                stack = new BlockStack(Block, stackCount);
                return true;
            }
            stack.Count += Count;
            return true;
        }
    }
}