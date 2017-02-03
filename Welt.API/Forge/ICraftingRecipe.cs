using Welt.API;

namespace Welt.API.Forge
{
    public interface ICraftingRecipe
    {
        ItemStack[,] Pattern { get; }
        ItemStack Output { get; }
        bool SignificantMetadata { get; }
    }
}