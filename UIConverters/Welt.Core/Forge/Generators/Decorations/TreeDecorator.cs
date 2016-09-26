using Welt.API.Forge;
using Welt.API.Forge.Generators;
using Welt.Core.Forge.Botany;

namespace Welt.Core.Forge.Generators.Decorations
{
    public class TreeDecorator : IDecorationGenerator
    {
        public TreeTypes[] ValidTrees { get; }

        public TreeDecorator(params TreeTypes[] trees)
        {
            ValidTrees = trees;
        }

        public void Decorate(IChunk chunk)
        {
            
        }

        public void Decorate(IChunk chunk, uint x, uint z)
        {
            
        }
    }
}