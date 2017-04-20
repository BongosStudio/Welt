using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Plays a sound effect (or special case: generates smoke particles)
    /// </summary>
    public struct SoundEffectPacket : IPacket
    {
        public enum EffectType
        {
            
        }

        public byte Id => 0x3D;

        public EffectType Effect;
        public int X;
        public sbyte Y;
        public int Z;
        /// <summary>
        /// For record play, the record ID. For smoke, the direction. For break block, the block ID.
        /// </summary>
        public int Data;

        public void ReadPacket(NetIncomingMessage stream)
        {
            Effect = (EffectType)stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadSByte();
            Z = stream.ReadInt32();
            Data = stream.ReadInt32();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write((int)Effect);
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write(Data);
        }
    }
}