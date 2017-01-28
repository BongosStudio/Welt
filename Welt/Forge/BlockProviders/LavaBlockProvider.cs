using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Forge.BlockProviders
{
    public class LavaBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.LAVA;

        public override string BlockName => "lava_still";

        public override string BlockTitle { get; set; } = "Lava";
    }
}
