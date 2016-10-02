using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public sealed class StoneBlock : Block
    {
        public StoneBlock() : base(1, 0, "stone", 0.5f, 1, 1, 1, false, false, true, false, false, 0, 0, 0) { }
    }

    public sealed class LimestoneBlock : Block
    {
        public LimestoneBlock() : base(1, 1, "limestone", 0.5f, 1, 1, 1, false, false, true, false, false, 0, 0, 0) { }
    }

    public sealed class DirtBlock : Block
    {
        public DirtBlock() : base(2, 0, "dirt", 0.3f, 1, 1, 1, false, false, true, false, false, 0, 0, 0) { }
    }

    public sealed class VolcanicSoilBlock : Block
    {
        public VolcanicSoilBlock() : base(2, 1, "volcanic_soil", 0.3f, 1, 1, 1, false, false, true, false, false, 0, 0, 0) { }
    }

    public sealed class GrassBlock : Block
    {
        public GrassBlock() : base(3, 0, "grass_block", 0.3f, 1, 1, 1, false, false, true, false, false, 0, 0, 0) { }
    }

    public sealed class ClayBlock : Block
    {
        public ClayBlock() : base(4, 0, "clay_block", 0.2f, 1, 1, 1, false, false, true, false, false, 0, 0, 0) { }
    }

    public sealed class WaterBlock : Block
    {
        public WaterBlock() : base(5, 0, "water", 0, 1, 1, 1, true, false, false, true, false, 0, 0, 0) { }
    }

    public sealed class LavaBlock : Block
    {
        public LavaBlock() : base(6, 0, "lava", 0, 1, 1, 1, false, false, false, true, false, 10, 3, 0) { }
    }

    public sealed class WoodBlock : Block
    {
        public WoodBlock() : base(7, 0, "wood", 0.4f, 1, 1, 1, false, true, true, false, false, 0, 0, 0) { }
    }

    public sealed class LeavesBlock : Block
    {
        public LeavesBlock() : base(8, 0, "leaves", 0.1f, 1, 1, 1, false, true, true, false, true, 0, 0, 0) { }
    }

    public sealed class SnowBlock : Block
    {
        public SnowBlock() : base(20, 0, "snow", 0.01f, 1, 0.2f, 1, false, false, false, true, false, 0, 0, 0) { }
    }

    public sealed class RosePlantBlock : Block
    {
        public RosePlantBlock() : base(30, 0, "rose", 0.01f, 0.2f, 0.2f, 0.2f, false, true, false, true, false, 0, 0, 0) { }
    }

    public sealed class GrassPlantBlock : Block
    {
        public GrassPlantBlock() : base(31, 0, "grass", 0.01f, 1, 0.4f, 1, false, true, false, true, false, 0, 0, 0) { }
    }

    public sealed class IronOreBlock : Block
    {
        public IronOreBlock() : base(100, 0, "iron_ore", 0.65f, 1, 1, 1, false, false, true, false, false, 0, 0, 0) { }
    }
}
