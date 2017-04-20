using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by servers to inform players of changes to the velocity of entities.
    /// Not sure exactly how all of that works, but I know it's going to be a pain in the ass.
    /// </summary>
    public struct EntityVelocityPacket : IPacket
    {
        public byte Id => 0x1C;

        public int EntityID;
        public short XVelocity;
        public short YVelocity;
        public short ZVelocity;

        public void ReadPacket(NetIncomingMessage stream)
        {
            EntityID = stream.ReadInt32();
            XVelocity = stream.ReadInt16();
            YVelocity = stream.ReadInt16();
            ZVelocity = stream.ReadInt16();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(EntityID);
            stream.Write(XVelocity);
            stream.Write(YVelocity);
            stream.Write(ZVelocity);
        }
    }
}