using Welt.API.Windows;

namespace Welt.API.Forge
{
    public interface ICraftingRepository
    {
        ICraftingRecipe GetRecipe(IWindowArea craftingArea);
        bool TestRecipe(IWindowArea craftingArea, ICraftingRecipe recipe, int x, int y);
    }
}