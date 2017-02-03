using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent to update the rotation of the player's head and body.
    /// </summary>
    public struct PlayerLookPacket : IPacket
    {
        public byte Id { get { return 0x0C; } }

        public float Yaw, Pitch;
        public bool OnGround;

        public void ReadPacket(IWeltStream stream)
        {
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteSingle(Yaw);
            stream.WriteSingle(Pitch);
            stream.WriteBoolean(OnGround);
        }
    }
}