using Welt.API.Forge.Weather;

namespace Welt.Core.Forge.Weather
{
    public class WeatherEngine : IWeatherEngine
    {
        public double Percipitation { get; }
        public long MinLength { get; }
        public long MaxLength { get; }
        public WeatherType[] AllowedWeather { get; }
        public void Update(double time)
        {
            
        }

        public void Toggle(WeatherType type)
        {
            
        }
    }
}