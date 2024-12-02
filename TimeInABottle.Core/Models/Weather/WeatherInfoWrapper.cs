using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Weather;
public class WeatherInfoWrapper
{
    private readonly WeatherInfo _weatherInfo;
    public WeatherInfoWrapper(WeatherInfo weatherInfo)
    {
        _weatherInfo = weatherInfo;
    }

    public TimeOnly Time => TimeOnly.FromDateTime(DateTime.Parse(_weatherInfo.StartTime));
    public double Temperature => _weatherInfo.Values.Temperature;
    public string WeatherType => WeatherDictionary[_weatherInfo.Values.WeatherCode];

    public static Dictionary<int, string> WeatherDictionary = new()
    {
        { 0, "Unknown" },
        { 1000, "Clear, Sunny" },
        { 1100, "Mostly Clear" },
        { 1101, "Partly Cloudy" },
        { 1102, "Mostly Cloudy" },
        { 1001, "Cloudy" },
        { 2000, "Fog" },
        { 2100, "Light Fog" },
        { 4000, "Drizzle" },
        { 4001, "Rain" },
        { 4200, "Light Rain" },
        { 4201, "Heavy Rain" },
        { 5000, "Snow" },
        { 5001, "Flurries" },
        { 5100, "Light Snow" },
        { 5101, "Heavy Snow" },
        { 6000, "Freezing Drizzle" },
        { 6001, "Freezing Rain" },
        { 6200, "Light Freezing Rain" },
        { 6201, "Heavy Freezing Rain" },
        { 7000, "Ice Pellets" },
        { 7101, "Heavy Ice Pellets" },
        { 7102, "Light Ice Pellets" },
        { 8000, "Thunderstorm" }
    };
}
