using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent to update the rotation of the player's head and body.
    /// </summary>
    public struct PlayerLookPacket : IPacket
    {
        public byte Id => 0x0C;

        public float Yaw, Pitch;
        public bool OnGround;

        public void ReadPacket(NetIncomingMessage stream)
        {
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(Yaw);
            stream.Write(Pitch);
            stream.Write(OnGround);
        }
    }
}