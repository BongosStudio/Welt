using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Welt.Forge.Renderers
{
    public abstract class Renderer<TItem, TVertex> : IDisposable where TVertex : struct, IVertexType 
    {
        private readonly object m_SyncLock = new object();

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<RendererEventArgs<TItem, TVertex>> MeshCompleted;

        private volatile bool m_IsRunning;
        private Thread[] m_RendererThreads;
        private volatile bool m_IsDisposed;
        protected ConcurrentQueue<TItem> m_Items, m_PriorityItems;
        private HashSet<TItem> m_Pending;

        /// <summary>
        /// Gets whether this renderer is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (m_IsDisposed)
                    throw new ObjectDisposedException(GetType().Name);
                return m_IsRunning;
            }
        }

        /// <summary>
        /// Gets whether this renderer is disposed of.
        /// </summary>
        public bool IsDisposed => m_IsDisposed;

        /// <summary>
        /// 
        /// </summary>
        protected Renderer()
        {
            lock (m_SyncLock)
            {
                m_IsRunning = false;
                var threads = Environment.ProcessorCount - 2;
                if (threads < 1)
                    threads = 1;
                m_RendererThreads = new Thread[threads];
                for (int i = 0; i < threads; i++)
                {
                    m_RendererThreads[i] = new Thread(DoRendering) { IsBackground = true, Name = $"{GetType().Name}_{i}" };
                }
                m_Items = new ConcurrentQueue<TItem>();
                m_PriorityItems = new ConcurrentQueue<TItem>();
                m_Pending = new HashSet<TItem>();
                m_IsDisposed = false;
            }
        }

        /// <summary>
        /// Starts this renderer.
        /// </summary>
        public void Start()
        {
            if (m_IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (m_IsRunning) return;
            lock (m_SyncLock)
            {
                m_IsRunning = true;
                for (int i = 0; i < m_RendererThreads.Length; i++)
                    m_RendererThreads[i].Start(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void DoRendering(object obj)
        {
            while (m_IsRunning)
            {
                var item = default(TItem);
                var result = default(Mesh<TVertex>);

                lock (m_SyncLock)
                {
                    if (m_PriorityItems.TryDequeue(out item) && m_Pending.Remove(item) && TryRender(item, out result))
                    {
                        var args = new RendererEventArgs<TItem, TVertex>(item, result, true);
                        MeshCompleted?.Invoke(this, args);
                    }
                    else if (m_Items.TryDequeue(out item) && m_Pending.Remove(item) && TryRender(item, out result))
                    {
                        var args = new RendererEventArgs<TItem, TVertex>(item, result, false);
                        MeshCompleted?.Invoke(this, args);
                    }
                }

                if (item == null) // We don't have any work, so sleep for a bit.
                    Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected abstract bool TryRender(TItem item, out Mesh<TVertex> result);

        /// <summary>
        /// Stops this renderer.
        /// </summary>
        public void Stop()
        {
            if (m_IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (!m_IsRunning) return;
            lock (m_SyncLock)
            {
                m_IsRunning = false;
                for (int i = 0; i < m_RendererThreads.Length; i++)
                    m_RendererThreads[i].Join();
            }
        }

        /// <summary>
        /// Enqueues an item to this renderer for rendering.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="hasPriority"></param>
        public bool Enqueue(TItem item, bool hasPriority = false)
        {
            if (m_IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (m_Pending.Contains(item))
                return false;
            m_Pending.Add(item);

            if (!m_IsRunning) return false;
            if (hasPriority)
                m_PriorityItems.Enqueue(item);
            else
                m_Items.Enqueue(item);
            return true;
        }

        /// <summary>
        /// Disposes of this renderer.
        /// </summary>
        public void Dispose()
        {
            if (m_IsDisposed)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of this renderer.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            Stop();
            lock (m_SyncLock)
            {
                m_RendererThreads = null;
                m_Items = null; m_PriorityItems = null;
                m_IsDisposed = true;
            }
        }

        /// <summary>
        /// Finalizes this renderer.
        /// </summary>
        ~Renderer()
        {
            Dispose(false);
        }
    }
}
