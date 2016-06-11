#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
namespace Welt.Forge.Generators
{
    public interface IChunkGenerator
    {
        void Generate(World world, Chunk chunk);
    }
}