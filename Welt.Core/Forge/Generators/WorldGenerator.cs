using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge.Noise;

namespace Welt.Core.Forge.Generators
{
    public class WorldGenerator : IWorldGenerator
    {
        public World World { get; set; }
        public IBiomeSystem BiomeSystem { get; private set; }
        public IWorldDecorationGenerator[] DecorationGenerators { get; private set; }

        public WorldGenerator(World world)
        {
            World = world;
        }

        public IChunk GenerateChunk(Vector3I index)
        {
            return BiomeSystem.GenerateChunk(index);
        }

        public void RegisterBiomeSystem(IBiomeSystem biomeSystem)
        {
            BiomeSystem = biomeSystem;
        }

        public void RegisterDecorator(IWorldDecorationGenerator decorator)
        {

        }
    }
}
