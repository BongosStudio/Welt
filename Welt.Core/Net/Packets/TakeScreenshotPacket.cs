using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct TakeScreenshotPacket : IPacket
    {
        public byte Id => 0xD0;
        public byte[] Data;

        public void ReadPacket(IWeltStream stream)
        {
            var length = stream.ReadInt32();
            Data = stream.ReadUInt8Array(length);
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteInt32(Data.Length);
            stream.WriteUInt8Array(Data);
        }
    }
}
