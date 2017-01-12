#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace Welt.API.Forge
{
    public interface IFloor
    {
        byte Level { get; }
        IBlockPalette Blocks { get; }
    }
}