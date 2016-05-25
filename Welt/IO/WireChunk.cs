#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Welt.Forge;

namespace Welt.IO
{
    public struct WireChunk
    {
        private static MemoryStream _mDataStream = new MemoryStream();
        private static BinaryWriter _mWriter = new BinaryWriter(_mDataStream);
        private static BinaryReader _mReader = new BinaryReader(_mDataStream);

        public IEnumerable<ushort> Blocks;

        public WireChunk(Chunk chunk)
        {
            Blocks = chunk.Blocks.Select(b => b.Id);
        }

        public byte[] ToArray()
        {
            _mDataStream = new MemoryStream();
            _mWriter = new BinaryWriter(_mDataStream);
            foreach (var block in Blocks)
            {
                _mWriter.Write(block);
            }
            return _mDataStream.GetBuffer();
        }

        public static WireChunk FromArray(byte[] data)
        {
            var blocks = new List<ushort>(data.Length/2);
            _mDataStream = new MemoryStream(data);
            _mReader = new BinaryReader(_mDataStream);
            while (_mDataStream.CanRead)
            {
                blocks.Add(_mReader.ReadUInt16());
            }
            return new WireChunk {Blocks = blocks};
        }
    }
}