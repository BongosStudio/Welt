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

        public void ReadPacket(IWeltStream stream)
        {
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteBoolean(OnGround);
        }
    }
}