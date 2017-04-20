using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sends actual blocks to populate chunks with.
    /// </summary>
    public struct ChunkDataPacket : IPacket
    {
        public byte Id => 0x33;

        public ChunkDataPacket(uint x, uint z, byte[] compressedData)
        {
            X = x;
            Z = z;
            CompressedData = compressedData;
        }

        public uint X;
        public uint Z;
        public byte[] CompressedData;

        public void ReadPacket(NetIncomingMessage stream)
        {
            X = stream.ReadUInt32();
            Z = stream.ReadUInt32();
            int len = stream.ReadInt32();
            CompressedData = stream.ReadBytes(len);
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(X);
            stream.Write(Z);
            stream.Write(CompressedData.Length);
            stream.Write(CompressedData);
        }
    }
}