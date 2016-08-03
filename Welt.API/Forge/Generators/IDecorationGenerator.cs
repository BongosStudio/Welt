#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace Welt.API.Forge.Generators
{
    public interface IDecorationGenerator
    {
        void Decorate(IChunk chunk);
        void Decorate(IChunk chunk, uint x, uint z);
    }
}