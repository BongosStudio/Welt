using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Welt.Core.Net
{
    public class SocketAsyncEventArgsPool : IDisposable
    {
        private readonly BlockingCollection<SocketAsyncEventArgs> m_ArgsPool;

        private readonly int m_MaxPoolSize;

        private BufferManager m_BufferManager;

        public SocketAsyncEventArgsPool(int poolSize, int maxSize, int bufferSize)
        {
            m_MaxPoolSize = maxSize;
            m_ArgsPool = new BlockingCollection<SocketAsyncEventArgs>(new ConcurrentQueue<SocketAsyncEventArgs>());
            m_BufferManager = new BufferManager(bufferSize);

            Init(poolSize);
        }

        private void Init(int size)
        {
            for (int i = 0; i < size; i++)
            {
                m_ArgsPool.Add(CreateEventArgs());
            }
        }

        public SocketAsyncEventArgs Get()
        {
            if (!m_ArgsPool.TryTake(out var args, 1000))
            {
                args = CreateEventArgs();
            }
            
            if (m_ArgsPool.Count > m_MaxPoolSize)
            {
                Trim(m_ArgsPool.Count - m_MaxPoolSize);
            }

            return args;
        }

        public void Add(SocketAsyncEventArgs args)
        {
            if (!m_ArgsPool.IsAddingCompleted)
                m_ArgsPool.Add(args);
        }

        protected SocketAsyncEventArgs CreateEventArgs()
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            m_BufferManager.SetBuffer(args);

            return args;
        }

        public void Trim(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (m_ArgsPool.TryTake(out var args))
                {
                    m_BufferManager.ClearBuffer(args);
                    args.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_ArgsPool.CompleteAdding();

                while (m_ArgsPool.Count > 0)
                {
                    SocketAsyncEventArgs arg = m_ArgsPool.Take();

                    m_BufferManager.ClearBuffer(arg);
                    arg.Dispose();
                }
            }

            m_BufferManager = null;
        }

        ~SocketAsyncEventArgsPool()
        {
            Dispose(false);
        }
    }
}
