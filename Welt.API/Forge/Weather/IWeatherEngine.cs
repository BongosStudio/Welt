namespace Welt.API.Forge.Weather
{
    public interface IWeatherEngine
    {
        double Percipitation { get; }
        long MinLength { get; } 
        long MaxLength { get; }
        WeatherType[] AllowedWeather { get; }
        
        void Update(double time);
        void Toggle(WeatherType type);
    }
}