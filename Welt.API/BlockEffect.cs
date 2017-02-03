namespace Welt.API
{
    public enum BlockEffect
    {
        /// <summary>
        ///     There is no visual effect applied.
        /// </summary>
        None,
        /// <summary>
        ///     Fast current ripples and refracting is applied.
        /// </summary>
        LightLiquidEffect,
        /// <summary>
        ///     Slow current ripples and distance shortening is applied.
        /// </summary>
        HeavyLiquidEffect,
        /// <summary>
        ///     Horizontal "grass in the breeze" effect is applied.
        /// </summary>
        VegetationWindEffect,
        /// <summary>
        ///     Heights will blend when applied
        /// </summary>
        HeightBlendEffect,

    }
}
