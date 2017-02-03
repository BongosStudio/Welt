using System;

namespace Welt.API.Net
{
    public interface IPacket
    {
        byte Id { get; }
        void ReadPacket(IWeltStream stream);
        void WritePacket(IWeltStream stream);
    }
}