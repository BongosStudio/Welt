using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API
{
    /// <summary>
    ///     Contains a collection of objects within a space of 2D vectors.
    /// </summary>
    public sealed class OccupiedSpacialMap<T> : IDisposable where T : class
    {
        private byte[,] m_SpacialMap;
        private List<(T Mass, byte X, byte Y)> m_Objects;

        public OccupiedSpacialMap(int size)
        {
            if (size > byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size), "Map size must be between 0 and 255");
            m_SpacialMap = new byte[size, size];
            for (var x = 0; x < m_SpacialMap.Length; x++)
            {
                for (var y = 0; y < m_SpacialMap.Length; y++)
                {
                    m_SpacialMap[x, y] = 0;
                }
            }
            m_Objects = new List<(T, byte, byte)>(size * (size / 2))
            {
                (default(T), 0, 0)
            };
        }

        public bool TrySetPosition(int x, int y, T value, int size)
        {
            if (value == null) return false;
            for (var ix = x - size; ix <= x + size; ix++)
            {
                for (var iy = y - size; iy <= y + size; iy++)
                {
                    if (m_SpacialMap[ix, iy] != 0)
                        return false;
                }
            }

            m_Objects.Add((value, (byte)x, (byte)y));

            for (var ix = x - size; ix <= x + size; ix++)
            {
                for (var iy = y - size; iy <= y + size; iy++)
                {
                    m_SpacialMap[ix, iy] = (byte)(m_Objects.Count - 1);
                }
            }

            return true;
        }

        public T GetObject(int x, int y, out int centerX, out int centerY)
        {
            if (m_SpacialMap[x, y] == 0)
            {
                centerX = 0; centerY = 0;
                return null;
            }
            var o = m_Objects[m_SpacialMap[x, y]];
            centerX = o.X;
            centerY = o.Y;
            return o.Mass;
        }

        public void RemoveObject(T value)
        {
            var index = m_Objects.FindIndex(o => o.Mass == value);
            for (var x = 0; x < m_SpacialMap.Length; x++)
            {
                for (var y = 0; y < m_SpacialMap.Length; y++)
                {
                    if (m_SpacialMap[x, y] == index)
                        m_SpacialMap[x, y] = 0;
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_Objects.Clear();
                    m_SpacialMap = null;
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OccupiedSpacialMap() {
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
