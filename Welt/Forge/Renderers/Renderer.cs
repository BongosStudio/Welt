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
    public abstract class Renderer<TItem, TVertex> : IDisposable where TVertex : IVertexType 
    {
        private readonly object _syncLock = new object();

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<RendererEventArgs<TItem, TVertex>> MeshCompleted;

        private volatile bool _isRunning;
        private Thread[] _rendererThreads;
        private volatile bool _isDisposed;
        protected ConcurrentQueue<TItem> _items, _priorityItems;
        private HashSet<TItem> _pending;

        /// <summary>
        /// Gets whether this renderer is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().Name);
                return _isRunning;
            }
        }

        /// <summary>
        /// Gets whether this renderer is disposed of.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Renderer()
        {
            lock (_syncLock)
            {
                _isRunning = false;
                var threads = Environment.ProcessorCount - 2;
                if (threads < 1)
                    threads = 1;
                _rendererThreads = new Thread[threads];
                for (int i = 0; i < _rendererThreads.Length; i++)
                {
                    _rendererThreads[i] = new Thread(DoRendering) { IsBackground = true };
                }
                _items = new ConcurrentQueue<TItem>(); _priorityItems = new ConcurrentQueue<TItem>();
                _pending = new HashSet<TItem>();
                _isDisposed = false;
            }
        }

        /// <summary>
        /// Starts this renderer.
        /// </summary>
        public void Start()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (_isRunning) return;
            lock (_syncLock)
            {
                _isRunning = true;
                for (int i = 0; i < _rendererThreads.Length; i++)
                    _rendererThreads[i].Start(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void DoRendering(object obj)
        {
            while (_isRunning)
            {
                var item = default(TItem);
                var result = default(Mesh<TVertex>);

                lock (_syncLock)
                {
                    if (_priorityItems.TryDequeue(out item) && _pending.Remove(item) && TryRender(item, out result))
                    {
                        var args = new RendererEventArgs<TItem, TVertex>(item, result, true);
                        MeshCompleted?.Invoke(this, args);
                    }
                    else if (_items.TryDequeue(out item) && _pending.Remove(item) && TryRender(item, out result))
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
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (!_isRunning) return;
            lock (_syncLock)
            {
                _isRunning = false;
                for (int i = 0; i < _rendererThreads.Length; i++)
                    _rendererThreads[i].Join();
            }
        }

        /// <summary>
        /// Enqueues an item to this renderer for rendering.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="hasPriority"></param>
        public bool Enqueue(TItem item, bool hasPriority = false)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (_pending.Contains(item))
                return false;
            _pending.Add(item);

            if (!_isRunning) return false;
            if (hasPriority)
                _priorityItems.Enqueue(item);
            else
                _items.Enqueue(item);
            return true;
        }

        /// <summary>
        /// Disposes of this renderer.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
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
            lock (_syncLock)
            {
                _rendererThreads = null;
                _items = null; _priorityItems = null;
                _isDisposed = true;
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
