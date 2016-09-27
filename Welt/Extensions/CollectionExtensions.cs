using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Selects the requested indexes from the array and returns them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T[] Pick<T>(this T[] collection, params byte[] items)
        {
            var data = new T[items.Length];
            for (var i = 0; i < items.Length; ++i)
            {
                data[i] = collection[items[i]];
            }
            return data;
        }

        /// <summary>
        ///     Selects the requested indexes from the array and returns them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T[] Pick<T>(this T[] collection, params int[] items)
        {
            var data = new T[items.Length];
            for (var i = 0; i < items.Length; ++i)
            {
                data[i] = collection[items[i]];
            }
            return data;
        }
    }
}
