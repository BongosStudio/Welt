#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Welt.API
{
    public abstract class BlockContainer : IList<ItemStack>
    {
        private readonly List<ItemStack> m_Collection;

        public int Count => m_Collection.Count;

        public bool IsReadOnly => false;

        protected BlockContainer(byte slotCount)
        {
            m_Collection = new List<ItemStack>(slotCount);
        }

        public ItemStack this[int index]
        {
            get { return m_Collection[index]; }
            set
            {
                m_Collection[index] = value;
                CollectionChanged?.Invoke(this, EventArgs.Empty);//TODO
            }
        }

        public virtual bool CanMoveTo(ItemStack stack, byte index)
        {
            return index < m_Collection.Count && m_Collection[index].TryMerge(ref stack);
        }

        public virtual bool TryMerge(ref ItemStack stack)
        {
            foreach (var slot in m_Collection)
            {
                if (!slot.TryMerge(ref stack)) continue;
                if (stack.Count > 0) continue;
                CollectionChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public IEnumerator<ItemStack> GetEnumerator()
        {
            return m_Collection.ToList().GetEnumerator();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Collection.GetEnumerator();
        }

        public int IndexOf(ItemStack item)
        {
            return m_Collection.IndexOf(item);
        }

        public void Insert(int index, ItemStack item)
        {
            m_Collection.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Collection.RemoveAt(index);
        }

        public void Add(ItemStack item)
        {
            m_Collection.Add(item);
        }

        public void Clear()
        {
            m_Collection.Clear();
        }

        public bool Contains(ItemStack item)
        {
            return m_Collection.Contains(item);
        }

        public void CopyTo(ItemStack[] array, int arrayIndex)
        {
            m_Collection.CopyTo(array, arrayIndex);
        }

        public bool Remove(ItemStack item)
        {
            return m_Collection.Remove(item);
        }

        public event EventHandler CollectionChanged;
    }
}