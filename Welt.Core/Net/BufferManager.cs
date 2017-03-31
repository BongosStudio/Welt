using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Welt.Core.Net
{
    public class BufferManager
    {
        private readonly object m_BufferLock = new object();

        private readonly List<byte[]> m_Buffers;

        private readonly int m_BufferSize;

        private readonly Stack<int> m_AvailableBuffers;

        public BufferManager(int bufferSize)
        {
            this.m_BufferSize = bufferSize;
            m_Buffers = new List<byte[]>();
            m_AvailableBuffers = new Stack<int>();
        }

        public void SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_AvailableBuffers.Count > 0)
            {
                int index = m_AvailableBuffers.Pop();

                byte[] buffer;
                lock (m_BufferLock)
                {
                    buffer = m_Buffers[index];
                }

                args.SetBuffer(buffer, 0, buffer.Length);
            }
            else
            {
                byte[] buffer = new byte[m_BufferSize];

                lock (m_BufferLock)
                {
                    m_Buffers.Add(buffer);
                }

                args.SetBuffer(buffer, 0, buffer.Length);
            }
        }

        public void ClearBuffer(SocketAsyncEventArgs args)
        {
            int index;
            lock (m_BufferLock)
            {
                index = m_Buffers.IndexOf(args.Buffer);
            }

            if (index >= 0)
                m_AvailableBuffers.Push(index);

            args.SetBuffer(null, 0, 0);
        }
    }
}
