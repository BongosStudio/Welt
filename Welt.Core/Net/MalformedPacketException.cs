using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Core.Net
{
    public class MalformedPacketException : Exception 
    {
        public byte PacketId;

        public MalformedPacketException(byte packetId) : base($"Could not read malformed packet with Id {packetId}")
        {
            PacketId = packetId;
        }
    }
}
