using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by clients to update whether or not the player is on the ground.
    /// Probably best to just ignore this.
    /// </summary>
    public struct PlayerGroundedPacket : IPacket
    {
        public byte Id => 0x0A;

        public bool OnGround;

        public void ReadPacket(NetIncomingMessage stream)
        {
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(OnGround);
        }
    }
}