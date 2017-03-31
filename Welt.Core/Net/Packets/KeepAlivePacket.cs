using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent periodically to confirm that the connection is still active. Send the same packet back
    /// to confirm it. Connection is dropped if no keep alive is received within 10 seconds.
    /// </summary>
    public struct KeepAlivePacket : IPacket
    {
        public byte Id => 0x00;
        public long Data;

        public void ReadPacket(IWeltStream stream)
        {
            // This space intentionally left blank
        }

        public void WritePacket(IWeltStream stream)
        {
            // This space intentionally left blank
        }
    }
}