#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge.Weather;

namespace Welt.API.Forge.Generators
{
    public interface IBiomeGenerator
    {
        int MaxElevation { get; }
        int MinElevation { get; }
        IWeatherEngine Weather { get; }
        IDecorationGenerator[] Decorations { get; }
        ushort Water { get; }
        bool CanHaveWater { get; }
        int WaterLine { get; }
        ICaveGenerator Caves { get; }
        ushort Grass { get; }
        bool CanHaveGrass { get; }
        int GrassLine { get; }

        IChunk GenerateChunk(IWorld world, uint x, uint z);
    }
}