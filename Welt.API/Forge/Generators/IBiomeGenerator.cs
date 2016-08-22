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
        ///     Gets the max elevation the generator will position a surface block.
        /// </summary>
        int MaxElevation { get; }
        /// <summary>
        ///     Gets the min elevation the generator will position a surface block.
        /// </summary>
        int MinElevation { get; }
        /// <summary>
        ///     Gets the <see cref="IWeatherEngine"/> the world will use per generator type.
        /// </summary>
        IWeatherEngine Weather { get; }
        /// <summary>
        ///     Gets a collection <see cref="IDecorationGenerator"/> that will be used in the biome.
        /// </summary>
        IDecorationGenerator[] Decorations { get; }
        /// <summary>
        ///     Gets the block used as water in the generator.
        /// </summary>
        Block Water { get; }
        /// <summary>
        ///     Gets whether or not the biome makes a pass to build water structures.
        /// </summary>
        bool CanHaveWater { get; }
        /// <summary>
        ///     Gets the median waterline value. Any water above ground will have a 5+/- block tolerance.
        /// </summary>
        int WaterLine { get; }
        /// <summary>
        ///     Gets the <see cref="ICaveGenerator"/>.
        /// </summary>
        ICaveGenerator Caves { get; }
        /// <summary>
        ///     Gets the blocks used for flora.
        /// </summary>
        Block[] Flora { get; }
        /// <summary>
        ///     Gets whether or not the biome can have flora.
        /// </summary>
        bool CanHaveFlora { get; }

        /// <summary>
        ///     Generates a randomized chunk of biome within the <see cref="IWorld"/>.
        /// </summary>
        /// <param name="world"></param>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        IChunk GenerateChunk(IWorld world, uint x, uint z);
    }
}