using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Forge;
using Welt.Core.Forge.BlockProviders;

namespace Welt.Core.Forge.Biomes
{
    public class MountainBiome : IBiome
    {
        public BiomeType BiomeType => BiomeType.Warm;

        public double PrecipitationChance => 0.2;

        public int RadiusMax => Chunk.Width * 8;

        public int RadiusMin => Chunk.Width * 4;

        public double ShapeVariation => 0.5;

        public ushort SublayerBlockId => new StoneBlockProvider().Id;

        public byte SublayerBlockMetadata => 0;

        public ushort SurfaceBlockId => new DirtBlockProvider().Id;

        public byte SurfaceBlockMetadata => 0;

        public ushort TopBlockId => new GrassBlockProvider().Id;

        public byte TopBlockMetadata => 0;
    }
}
