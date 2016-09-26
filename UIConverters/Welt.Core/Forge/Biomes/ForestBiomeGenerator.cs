using System;
using Welt.API.Forge;
using Welt.API.Forge.Generators;
using Welt.API.Forge.Weather;
using Welt.Core.Forge.Botany;
using Welt.Core.Forge.Generators.Decorations;
using Welt.Core.Forge.Weather;
using Welt.Core.Services;

namespace Welt.Core.Forge.Biomes
{
    public class ForestBiomeGenerator : IBiomeGenerator
    {
        public int MaxElevation => 150;
        public int MinElevation => 120;
        public IWeatherEngine Weather => new WeatherEngine();
        public IDecorationGenerator[] Decorations => new IDecorationGenerator[]
        {
            new TreeDecorator(TreeTypes.Oak, TreeTypes.Spruce), 
        };

        public (ushort Id, byte Metadata) Water => BlockService.GetBlockFromName("water");
        public bool CanHaveWater => true;
        public int WaterLine => 130;
        public ICaveGenerator Caves { get; }
        public bool CanHaveFlora => true;

        public (ushort Id, byte Metadata)[] Flora => new[]
        {
            BlockService.GetBlockFromName("rose"),
            BlockService.GetBlockFromName("grass")
        };

        public IChunk GenerateChunk(IWorld world, uint x, uint z)
        {
            var chunk = new Chunk(world, x, z);
            return chunk;
        }
    }
}