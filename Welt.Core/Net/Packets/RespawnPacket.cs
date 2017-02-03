using System;
using Welt.API.Net;
using Welt.API;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by clients when the player clicks "Respawn" after death. Sent by servers to confirm
    /// the respawn, and to respawn players in different dimensions (i.e. when using a portal).
    /// </summary>
    public struct RespawnPacket : IPacket
    {
        public byte Id { get { return 0x09; } }
        
        public void ReadPacket(IWeltStream stream)
        {
            
        }

        public void WritePacket(IWeltStream stream)
        {
            
        }
    }
}