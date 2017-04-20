using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct TimeUpdatePacket : IPacket
    {
        public byte Id => 0x04;

        public TimeUpdatePacket(int time)
        {
            Time = time;
        }

        public int Time;

        public void ReadPacket(NetIncomingMessage stream)
        {
            Time = stream.ReadInt32();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(Time);
        }
    }
}