using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Used to allocate or unload chunks.
    /// </summary>
    public struct ChunkPreamblePacket : IPacket
    {
        public byte Id => 0x32;

        public ChunkPreamblePacket(uint x, uint z, bool load = true)
        {
            X = x;
            Z = z;
            Load = load;
        }

        public uint X, Z;
        /// <summary>
        /// If false, free the chunk. If true, allocate it.
        /// </summary>
        public bool Load;

        public void ReadPacket(IWeltStream stream)
        {
            X = stream.ReadUInt32();
            Z = stream.ReadUInt32();
            Load = stream.ReadBoolean();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteUInt32(X);
            stream.WriteUInt32(Z);
            stream.WriteBoolean(Load);
        }
    }
}