#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge.Weather;

namespace Welt.API.Forge.Generators
{
    /// <summary>
    ///     The default interface that all biome generators inherit. Gives access to weather patterns, generated decorations that
    ///     are biome-specific, the cave generator, elevation patterns, water patterns, and floral life.
    /// </summary>
    public interface IBiomeGenerator
    {
        /// <summary>
        ///     The max elevation the generator will position a surface block.
        /// </summary>
        int MaxElevation { get; }
        /// <summary>
        ///     The min elevation the generator will position a surface block.
        /// </summary>
        int MinElevation { get; }
        /// <summary>
        ///     The <see cref="Welt.API.Forge.Weather.IWeatherEngine"/> the world will use per generator type.
        /// </summary>
        IWeatherEngine Weather { get; }
        /// <summary>
        ///     A collection <see cref="Welt.API.Forge.Generator"/>
        /// </summary>
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