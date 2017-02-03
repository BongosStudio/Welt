#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
namespace Welt.API.Forge
{
    public interface IChunkGenerator
    {
        void Generate(IWorld world, IChunk chunk);
    }
}