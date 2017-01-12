namespace Welt.API.Forge.Weather
{
    /// <summary>
    ///     Types of weather an engine can create.
    /// </summary>
    public enum WeatherType
    {
        /// <summary>
        ///     Means there is no overcast and no precipitation.
        /// </summary>
        Clear,
        /// <summary>
        ///     A slight overcast with no precipitation.
        /// </summary>
        Cloudy,
        /// <summary>
        ///     A slight overcast with little precipitation.
        /// </summary>
        Light,
        /// <summary>
        ///     An average overcast with average precipitation.
        /// </summary>
        Average,
        /// <summary>
        ///     Thicker and darker overcast with heavier precipitation, but no major events.
        /// </summary>
        Heavy,
        /// <summary>
        ///     Thicker and darker overcast with heavy precipitation and extreme events; ie:
        ///     thunderstorms, floods, fires, blizzards, etc.
        /// </summary>
        Extreme,
        /// <summary>
        ///     Absolute destruction of the environment. This will not be common at all. The only usage 
        ///     will be on extreme planets.
        /// </summary>
        Apocalyptic
    }
}