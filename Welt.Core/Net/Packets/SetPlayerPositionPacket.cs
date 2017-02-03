using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by servers to set the position and look of the player. Can be used to teleport players.
    /// </summary>
    public struct SetPlayerPositionPacket : IPacket
    {
        public byte Id { get { return 0x0D; } }

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

        public void ReadPacket(IWeltStream stream)
        {
            X = stream.ReadSingle();
            Stance = stream.ReadDouble();
            Y = stream.ReadSingle();
            Z = stream.ReadSingle();
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteSingle(X);
            stream.WriteDouble(Stance);
            stream.WriteSingle(Y);
            stream.WriteSingle(Z);
            stream.WriteSingle(Yaw);
            stream.WriteSingle(Pitch);
            stream.WriteBoolean(OnGround);
        }
    }
}