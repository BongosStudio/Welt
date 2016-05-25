#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System.Collections.Generic;
using System.Linq;

namespace Welt.Types
{
    public class SparseMatrix<T>
    {
        // Master dictionary hold rows of column dictionary
        protected Dictionary<uint, Dictionary<uint, T>> Rows;

        /// <summary>
        /// Constructs a SparseMatrix instance.
        /// </summary>
        public SparseMatrix()
        {
            Rows = new Dictionary<uint, Dictionary<uint, T>>();
        }

        /// <summary>
        /// Gets or sets the value at the specified matrix position.
        /// </summary>
        /// <param name="row">Matrix row</param>
        /// <param name="col">Matrix column</param>
        public T this[uint row, uint col]
        {
            get
            {
                return GetAt(row, col);
            }
            set
            {
                SetAt(row, col, value);
            }
        }

        /// <summary>
        /// Gets the value at the specified matrix position.
        /// </summary>
        /// <param name="row">Matrix row</param>
        /// <param name="col">Matrix column</param>
        /// <returns>Value at the specified position</returns>
        public T GetAt(uint row, uint col)
        {
            Dictionary<uint, T> cols;
            if (Rows.TryGetValue(row, out cols))
            {
                var value = default(T);
                if (cols.TryGetValue(col, out value))
                    return value;
            }
            return default(T);
        }

        /// <summary>
        /// Sets the value at the specified matrix position.
        /// </summary>
        /// <param name="row">Matrix row</param>
        /// <param name="col">Matrix column</param>
        /// <param name="value">New value</param>
        public void SetAt(uint row, uint col, T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                // Remove any existing object if value is default(T)
                RemoveAt(row, col);
            }
            else
            {
                // Set value
                Dictionary<uint, T> cols;
                if (!Rows.TryGetValue(row, out cols))
                {
                    cols = new Dictionary<uint, T>();
                    Rows.Add(row, cols);
                }
                cols[col] = value;
            }
        }

        /// <summary>
        /// Removes the value at the specified matrix position.
        /// </summary>
        /// <param name="row">Matrix row</param>
        /// <param name="col">Matrix column</param>
        public void RemoveAt(uint row, uint col)
        {
            Dictionary<uint, T> cols;
            if (Rows.TryGetValue(row, out cols))
            {
                // Remove column from this row
                cols.Remove(col);
                // Remove entire row if empty
                if (cols.Count == 0)
                    Rows.Remove(row);
            }
        }

        /// <summary>
        /// Returns all items in the specified row.
        /// </summary>
        /// <param name="row">Matrix row</param>
        public IEnumerable<T> GetRowData(uint row)
        {
            Dictionary<uint, T> cols;
            if (Rows.TryGetValue(row, out cols))
            {
                foreach (var pair in cols)
                {
                    yield return pair.Value;
                }
            }
        }

        public void RemoveRow(uint row)
        {
            Rows.Remove(row);
        }

        public void RemoveColumn(uint col)
        {
            foreach (var rowdata in Rows)
            {
                rowdata.Value.Remove(col);
            }
        }

        /// <summary>
        /// Returns the number of items in the specified row.
        /// </summary>
        /// <param name="row">Matrix row</param>
        public int GetRowDataCount(uint row)
        {
            Dictionary<uint, T> cols;
            if (Rows.TryGetValue(row, out cols))
            {
                return cols.Count;
            }
            return 0;
        }

        /// <summary>
        /// Returns all items in the specified column.
        /// This method is less efficent than GetRowData().
        /// </summary>
        /// <param name="col">Matrix column</param>
        /// <returns></returns>
        public IEnumerable<T> GetColumnData(uint col)
        {
            foreach (var rowdata in Rows)
            {
                T result;
                if (rowdata.Value.TryGetValue(col, out result))
                    yield return result;
            }
        }

        /// <summary>
        /// Returns the number of items in the specified column.
        /// This method is less efficent than GetRowDataCount().
        /// </summary>
        /// <param name="col">Matrix column</param>
        public uint GetColumnDataCount(uint col)
        {
            
            return (uint) Rows.Count(cols => cols.Value.ContainsKey(col));
        }
    }
}