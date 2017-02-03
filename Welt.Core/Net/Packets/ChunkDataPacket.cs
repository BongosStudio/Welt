using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sends actual blocks to populate chunks with.
    /// </summary>
    public struct ChunkDataPacket : IPacket
    {
        public byte Id { get { return 0x33; } }

        public ChunkDataPacket(uint x, short y, uint z, short width, short height, short depth, byte[] compressedData)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;
            Height = height;
            Depth = depth;
            CompressedData = compressedData;
        }

        public uint X;
        public short Y;
        public uint Z;
        public short Width, Height, Depth;
        public byte[] CompressedData;

        public void ReadPacket(IWeltStream stream)
        {
            X = stream.ReadUInt32();
            Y = stream.ReadInt16();
            Z = stream.ReadUInt32();
            Width = (short)(stream.ReadInt8() + 1);
            Height = (short)(stream.ReadInt8() + 1);
            Depth = (short)(stream.ReadInt8() + 1);
            int len = stream.ReadInt32();
            CompressedData = stream.ReadUInt8Array(len);
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteUInt32(X);
            stream.WriteInt16(Y);
            stream.WriteUInt32(Z);
            stream.WriteInt8((sbyte)(Width - 1));
            stream.WriteInt8((sbyte)(Height - 1));
            stream.WriteInt8((sbyte)(Depth - 1));
            stream.WriteInt32(CompressedData.Length);
            stream.WriteUInt8Array(CompressedData);
        }
    }
}