using Welt.Types;

namespace Welt.Forge.Generators.Decorations
{
    public interface IDecorGenerator
    {
        Block[] GenerateDecoration(Chunk chunk, Vector3I anchor, params string[] args);
    }
}