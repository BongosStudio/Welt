﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct ScreenshotRequestPacket : IPacket
    {
        public byte Id => 0xD1;

        public void ReadPacket(NetIncomingMessage stream)
        {
            
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            
        }
    }
}
