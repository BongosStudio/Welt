using System;
using Welt.API.Net;

namespace Welt.API
{
    public class MetadataInt : MetadataEntry
    {
        public override byte Identifier { get { return 2; } }
        public override string FriendlyName { get { return "int"; } }

        public int Value;

        public static implicit operator MetadataInt(int value)
        {
            return new MetadataInt(value);
        }

        public MetadataInt()
        {
        }

        public MetadataInt(int value)
        {
            Value = value;
        }

        public override void FromStream(IWeltStream stream)
        {
            Value = stream.ReadInt32();
        }

        public override void WriteTo(IWeltStream stream, byte index)
        {
            stream.WriteUInt8(GetKey(index));
            stream.WriteInt32(Value);
        }
    }
}
