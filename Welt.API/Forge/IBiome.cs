using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    public interface IBiome
    {
        int RadiusMin { get; }
        int RadiusMax { get; }

        double ShapeVariation { get; }

        double PrecipitationChance { get; }

        BiomeType BiomeType { get; }

        ushort TopBlockId { get; }
        byte TopBlockMetadata { get; }

        ushort SurfaceBlockId { get; }
        byte SurfaceBlockMetadata { get; }

        ushort SublayerBlockId { get; }
        byte SublayerBlockMetadata { get; }
    }
}
