#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace Welt.API.Forge.Generators
{
    public interface IForgeGenerator
    {
        string LevelType { get; }
        string GeneratorOptions { get; }
        int SpawnX { get; set; }
        int SpawnY { get; set; }
        int SpawnZ { get; set; }

        void Initialize(IWorld world);
        void GenerateChunk(IWorld world, IChunk chunk);

    }
}