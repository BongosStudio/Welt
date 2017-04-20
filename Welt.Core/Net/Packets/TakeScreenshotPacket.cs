using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct ScreenshotResultPacket : IPacket
    {
        public byte Id => 0xD0;
        public byte[] Data;

        public void ReadPacket(NetIncomingMessage stream)
        {
            var length = stream.ReadInt32();
            Data = stream.ReadBytes(length);
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(Data.Length);
            stream.Write(Data);
        }
    }
}
