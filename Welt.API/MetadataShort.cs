using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.API
{
    public class MetadataShort : MetadataEntry
    {
        public override byte Identifier { get { return 1; } }
        public override string FriendlyName { get { return "short"; } }

        public short Value;

        public static implicit operator MetadataShort(short value)
        {
            return new MetadataShort(value);
        }

        public MetadataShort()
        {
        }

        public MetadataShort(short value)
        {
            Value = value;
        }

        public override void FromStream(NetIncomingMessage stream)
        {
            Value = stream.ReadInt16();
        }

        public override void WriteTo(NetOutgoingMessage stream, byte index)
        {
            stream.Write(GetKey(index));
            stream.Write(Value);
        }
    }
}
