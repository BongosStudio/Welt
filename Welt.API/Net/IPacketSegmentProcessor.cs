using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Welt.API.Net
{
    public interface IPacketSegmentProcessor
    {
        bool ProcessNextSegment(byte[] nextSegment, int offset, int len, out IPacket packet);
    }
}
