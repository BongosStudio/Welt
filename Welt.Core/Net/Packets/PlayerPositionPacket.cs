using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by clients to inform the server of updates to their position.
    /// </summary>
    public struct PlayerPositionPacket : IPacket
    {
        public byte Id => 0x0B;

        public float X, Y, Z;
        /// <summary>
        /// The Y position of the player's eyes. This changes when crouching.
        /// </summary>
        public double Stance;
        public bool OnGround;

        public void ReadPacket(NetIncomingMessage stream)
        {
            X = stream.ReadSingle();
            Y = stream.ReadSingle();
            Stance = stream.ReadDouble();
            Z = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Stance);
            stream.Write(Z);
            stream.Write(OnGround);
        }
    }
}