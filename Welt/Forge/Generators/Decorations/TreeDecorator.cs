using Welt.Types;

namespace Welt.Forge.Generators.Decorations
{
    public class TreeDecorator : IDecorGenerator
    {
        public Block[] GenerateDecoration(Chunk chunk, Vector3I anchor, params string[] args)
        {
            var trees = new Block[5*5*8];
            trees[WorldHelpers.GetIndexFromPosition(3, 0, 3, 4, 7, 4)] = new Block(BlockType.LOG);
            trees[WorldHelpers.GetIndexFromPosition(3, 1, 3, 4, 7, 4)] = new Block(BlockType.LOG);
            trees[WorldHelpers.GetIndexFromPosition(3, 2, 3, 4, 7, 4)] = new Block(BlockType.LOG);
            trees[WorldHelpers.GetIndexFromPosition(3, 3, 3, 4, 7, 4)] = new Block(BlockType.LOG);
            return trees;
        }
    }
}