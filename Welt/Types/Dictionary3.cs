#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Collections.Generic;

#endregion

namespace Welt.Types
{
    public class Dictionary3<T> : Dictionary<ulong, T>
    {
        const ulong SIZE = UInt32.MaxValue;
        const ulong SIZE_SQUARED = (ulong)UInt32.MaxValue * UInt32.MaxValue;
        //and get some oolong tea

        public T this[uint x, uint y, uint z]
        {
            get
            {
                var outVal = default(T);
                TryGetValue((ulong)(x + (y * SIZE) + (z * SIZE_SQUARED)), out outVal);
                return outVal;
            }
            set
            {
                var key = (ulong)(x + (y * SIZE) + (z * SIZE_SQUARED));

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
