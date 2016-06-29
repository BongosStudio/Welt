#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge;
using Welt.API.Forge.Generators;

namespace Welt.Core.Forge.Generators
{
    public class StandardGenerator : IForgeGenerator
    {
        public string LevelType => "DEFAULT";
        public string GeneratorOptions { get; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public int SpawnZ { get; set; }

        public void Initialize(IWorld world)
        {
            SpawnX = FastMath.NextRandom(world.Size*16);
            SpawnZ = FastMath.NextRandom(world.Size*16);
            SpawnY = 128;
        }

        public IChunk GenerateChunk(IWorld world, uint x, uint z)
        {
            throw new System.NotImplementedException();
        }
    }
}