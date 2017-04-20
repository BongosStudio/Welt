using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by servers to set the position and look of the player. Can be used to teleport players.
    /// </summary>
    public struct SetPlayerPositionPacket : IPacket
    {
        public byte Id => 0x0E;

        public SetPlayerPositionPacket(float x, float y, double stance, float z, float yaw, float pitch, bool onGround)
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
            Stance = stream.ReadDouble();
            Y = stream.ReadSingle();
            Z = stream.ReadSingle();
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(X);
            stream.Write(Stance);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write(Yaw);
            stream.Write(Pitch);
            stream.Write(OnGround);
        }
    }
}