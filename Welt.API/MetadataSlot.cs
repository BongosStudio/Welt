using System;
using System.IO;
using Welt.API.Net;

namespace Welt.API
{
    public class MetadataSlot : MetadataEntry
    {
        public override byte Identifier { get { return 5; } }
        public override string FriendlyName { get { return "slot"; } }

        public ItemStack Value;

        public static implicit operator MetadataSlot(ItemStack value)
        {
            return new MetadataSlot(value);
        }

        public MetadataSlot()
        {
        }

        public MetadataSlot(ItemStack value)
        {
            Value = value;
        }

        public override void FromStream(IWeltStream stream)
        {
            
        }

        public override void WriteTo(IWeltStream stream, byte index)
        {
            
        }
    }
}
