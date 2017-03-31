using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Forge
{
    /// <summary>
    ///     Contains set blocks that are to be added once a chunk is generated.
    /// </summary>
    public class BlockBucket : IDisposable
    {
        // Key = chunk index; Value = block data
        private Dictionary<Vector3I, Queue<BlockDescriptor>> m_Store;

        /// <summary>
        ///     Creates a new instance of the bucket.
        /// </summary>
        public BlockBucket()
        {
            m_Store = new Dictionary<Vector3I, Queue<BlockDescriptor>>();
        }

        /// <summary>
        ///     Enqueues a block to be added to the store.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="block"></param>
        public void Enqueue(ReadOnlyChunk chunk, BlockDescriptor block)
        {
            if (m_Store.ContainsKey(chunk.GetIndex()))
            {
                m_Store.Add(chunk.GetIndex(), new Queue<BlockDescriptor>());
            }
            m_Store[chunk.GetIndex()].Enqueue(block);
        }

        /// <summary>
        ///     Gets all blocks enqueued for the specified chunk.
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public IEnumerable<BlockDescriptor> GetChunkStore(ReadOnlyChunk chunk)
        {
            var index = chunk.GetIndex();
            if (!m_Store.ContainsKey(index)) yield break;
            while (m_Store[index].Count > 0)
                yield return m_Store[index].Dequeue();
            m_Store.Remove(chunk.GetIndex());
        }

        /// <summary>
        ///     Returns the next available block for the specified chunk.
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public BlockDescriptor Dequeue(ReadOnlyChunk chunk)
        {
            return m_Store[chunk.GetIndex()].Dequeue();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_Store.Clear();
                }

                m_Store = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BlockBucket() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
