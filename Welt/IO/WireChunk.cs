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
        private static MemoryStream m_dataStream = new MemoryStream();
        private static BinaryWriter m_writer = new BinaryWriter(m_dataStream);
        private static BinaryReader m_reader = new BinaryReader(m_dataStream);

        public IEnumerable<ushort> Blocks;

        public WireChunk(Chunk chunk)
        {
            Blocks = chunk.Blocks.Select(b => b.Id);
        }

        public byte[] ToArray()
        {
            m_dataStream = new MemoryStream();
            m_writer = new BinaryWriter(m_dataStream);
            foreach (var block in Blocks)
            {
                m_writer.Write(block);
            }
            return m_dataStream.GetBuffer();
        }

        public static WireChunk FromArray(byte[] data)
        {
            var blocks = new List<ushort>(data.Length/2);
            m_dataStream = new MemoryStream(data);
            m_reader = new BinaryReader(m_dataStream);
            while (m_dataStream.CanRead)
            {
                blocks.Add(m_reader.ReadUInt16());
            }
            return new WireChunk {Blocks = blocks};
        }
    }
}