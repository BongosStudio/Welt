#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Collections.Concurrent;

#endregion

namespace Welt.Types
{
    public class Dictionary2<T> : ConcurrentDictionary<ulong, T>
    {
        private const ulong SIZE = uint.MaxValue;

        public ulong KeyFromCoords(uint x, uint z)
        {
            return (x + (z*SIZE));
        }

        //prefer to let the key calculation inlined here
        public virtual T this[uint x, uint z]
        {
            get
            {
                var outVal = default(T);
                TryGetValue((x + (z*SIZE)), out outVal);
                return outVal;
            }
            set
            {
                var key = (x + (z*SIZE));

                this[key] = value;
            }
        }

        public virtual void Remove(uint x, uint z)
        {
            T removed;
            TryRemove(KeyFromCoords(x, z), out removed);
        }
    }
}
