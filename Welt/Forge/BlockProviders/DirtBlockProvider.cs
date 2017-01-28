using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Forge.BlockProviders
{
    public class DirtBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.DIRT;

        public override string BlockName => "dirt";

        public override string BlockTitle { get; set; } = "Dirt";
    }
}
