#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System.Collections.Generic;

#endregion

namespace Welt.API
{
    public class Dictionary3<T> : Dictionary<ulong, T>
    {
        const ulong SIZE = uint.MaxValue;
        private const ulong SIZE_SQUARED = (ulong) uint.MaxValue*uint.MaxValue;
        //and get some oolong tea

        public T this[uint x, uint y, uint z]
        {
            get
            {
                var outVal = default(T);
                TryGetValue((x + (y * SIZE) + (z * SIZE_SQUARED)), out outVal);
                return outVal;
            }
            set
            {
                var key = (x + (y * SIZE) + (z * SIZE_SQUARED));

                //T outVal = default(T);
                //if (TryGetValue(key, out outVal))
                //{
                    this[key] = value; 
                //}
                //else
                //{
                //    Add(key, value);
                //}
             }
        }
    }
}
