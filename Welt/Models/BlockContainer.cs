#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Welt.Models
{
    public abstract class BlockContainer
    {
        private readonly BlockStack[] _collection;

        protected BlockContainer(byte slotCount)
        {
            _collection = new BlockStack[slotCount];
        }

        public BlockStack this[int index]
        {
            get { return _collection[index]; }
            set
            {
                _collection[index] = value;
                CollectionChanged?.Invoke(this, EventArgs.Empty);//TODO
            }
        }

        public virtual bool CanMoveTo(BlockStack stack, byte index)
        {
            return index < _collection.Length && _collection[index].TryMerge(ref stack);
        }

        public virtual bool TryMerge(ref BlockStack stack)
        {
            foreach (var slot in _collection)
            {
                if (!slot.TryMerge(ref stack)) continue;
                if (stack.Count > 0) continue;
                CollectionChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public IEnumerator<BlockStack> GetEnumerator()
        {
            return _collection.ToList().GetEnumerator();
        }

        public static BlockContainer FromBinary(byte[] buffer)
        {
            switch (buffer[0])
            {
                case 0:         // is an inventory collection
                    break;
                case 1:         // is a chest collection
                    break;
                case 2:         // is a crate collection
                    break;
                case 3:         // is a furnace collection
                    break;
                default:
                    // TODO: attempt to catch within a plugin/mod seated collection
                    break;
            }
            return null;
        }

        public event EventHandler CollectionChanged;
    }
}