#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
namespace Welt.Core.Forge.Weather
{
    public enum WeatherType : byte
    {
        // Cloud descriptions
        Clear,
        LightClouds,
        HeavyClouds,
        // Percipitation descriptions
        NoPercipitation,
        LightPercipitation,
        HeavyPercipitation,
        // Weather events 
        Tornado,
        Hurricane,
        Flooding,
    }
}