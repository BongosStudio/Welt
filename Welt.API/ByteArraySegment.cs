using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Welt.API
{
    public class ByteArraySegment : ICollection<byte>
    {
        private readonly byte[] m_Array;
        private readonly int m_Start;
        private readonly int m_Count;

        public ByteArraySegment(byte[] array, int start, int count)
        {
            this.m_Array = array;
            this.m_Start = start;
            this.m_Count = count;
        }

        public void Add(byte item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(byte item)
        {
            return m_Array.Contains(item);
        }

        public void CopyTo(byte[] target, int index)
        {
            Buffer.BlockCopy(m_Array, m_Start, target, index, m_Count);
        }

        public bool Remove(byte item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return m_Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public byte this[int index]
        {
            get
            {
                return m_Array[index];
            }
            set
            {
                if (index > m_Array.Length)
                    throw new ArgumentOutOfRangeException("value");

                m_Array[index] = value;
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return new ByteArraySegmentEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class ByteArraySegmentEnumerator : IEnumerator<byte>
        {
            private byte current;
            private int pos;

            private readonly ByteArraySegment _segment;

            public ByteArraySegmentEnumerator(ByteArraySegment segment)
            {
                _segment = segment;
                pos = segment.m_Start;
            }

            public bool MoveNext()
            {
                if (pos >= _segment.Count)
                    return false;

                current = _segment.m_Array[++pos];

                return true;
            }

            public void Reset()
            {
                pos = _segment.m_Start;
            }

            public byte Current
            {
                get
                {
                    return current;
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

        }
    }
}
