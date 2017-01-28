﻿using System;
using System.Collections.ObjectModel;

namespace Welt.Types
{
    /// <summary>
    /// Represents an array of 4-bit values.
    /// </summary>
    public class NibbleArray
    {
        /// <summary>
        /// The data in the nibble array. Each byte contains
        /// two nibbles, stored in big-endian.
        /// </summary>
        public byte[] Data { get; set; }

        public NibbleArray()
        {
        }

        /// <summary>
        /// Creates a new nibble array with the given number of nibbles.
        /// </summary>
        public NibbleArray(int length)
        {
            Data = new byte[length / 2];
        }

        /// <summary>
        /// Gets the current number of nibbles in this array.
        /// </summary>
        public int Length
        {
            get { return Data.Length * 2; }
        }

        /// <summary>
        /// Gets or sets a nibble at the given index.
        /// </summary>
        public byte this[int index]
        {
            get
            {
                //var data = Data[index / 2];
                //var shift = ((index) % 2 * 4) & 0xF;
                //return (byte)(data >> shift);
                return (byte)(Data[index / 2] >> ((index) % 2 * 4) & 0xF);
            }
            set
            {
                value &= 0xF;
                Data[index / 2] &= (byte)(0xF << ((index + 1) % 2 * 4));
                Data[index / 2] |= (byte)(value << (index % 2 * 4));
            }
        }

        public static (byte, byte) GetData(byte data)
        {
            return ((byte)(data & 0x0F), (byte)(data & 0xF0 >> 4));
        }

        public static byte GetNibble(byte a, byte b)
        {
            return (byte)((b << 4) | a);
        }
    }

    public class ReadOnlyNibbleArray
    {
        private NibbleArray NibbleArray { get; set; }

        public ReadOnlyNibbleArray(NibbleArray array)
        {
            NibbleArray = array;
        }

        public byte this[int index] => NibbleArray[index];

        public ReadOnlyCollection<byte> Data => Array.AsReadOnly(NibbleArray.Data);
    }

}
