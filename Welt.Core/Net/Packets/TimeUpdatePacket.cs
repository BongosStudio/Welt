using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct TimeUpdatePacket : IPacket
    {
        public byte Id { get { return 0x04; } }

        public TimeUpdatePacket(int time)
        {
            Time = time;
        }

        public int Time;

        public void ReadPacket(IWeltStream stream)
        {
            Time = stream.ReadInt32();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteInt32(Time);
        }
    }
}