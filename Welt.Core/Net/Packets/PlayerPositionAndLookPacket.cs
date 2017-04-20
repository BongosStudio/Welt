using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by clients to inform the server of updates to their position and look direction.
    /// A combination of the PlayerPosition and PlayerLook packets.
    /// </summary>
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public byte Id => 0x0D;

        public PlayerPositionAndLookPacket(float x, float y, double stance, float z, float yaw, float pitch, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            Stance = stance;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public float X, Y, Z;
        public double Stance;
        public float Yaw, Pitch;
        public bool OnGround;

        public void ReadPacket(NetIncomingMessage stream)
        {
            X = stream.ReadSingle();
            Y = stream.ReadSingle();
            Stance = stream.ReadDouble();
            Z = stream.ReadSingle();
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Stance);
            stream.Write(Z);
            stream.Write(Yaw);
            stream.Write(Pitch);
            stream.Write(OnGround);
        }
    }
}