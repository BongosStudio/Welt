﻿using System;
using Welt.API.Net;
using Welt.API;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by the server to allow the player to spawn, with information about the world being spawned into.
    /// </summary>
    public struct LoginResponsePacket : IPacket
    {
        public byte Id => 0x01;

        public LoginResponsePacket(int entityID, long seed, string worldName)
        {
            EntityID = entityID;
            Seed = seed;
            WorldName = worldName;
        }

        public int EntityID;
        public long Seed;
        public string WorldName;

        public void ReadPacket(IWeltStream stream)
        {
            EntityID = stream.ReadInt32();
            Seed = stream.ReadInt64();
            WorldName = stream.ReadString();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteInt32(EntityID);
            stream.WriteInt64(Seed);
            stream.WriteString(WorldName);
        }
    }
}