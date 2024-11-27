using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Core.Services;
internal class WeatherBasedBufferService : IBufferService
{
    private readonly IWeatherService _weatherService;

    public WeatherBasedBufferService(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public int BufferSize {
        get; private set;
    }

    public void LoadBuffer()
    {
        var info = _weatherService.getNextHourWeatherInfo();
        var code = info.Values.WeatherCode;

        BufferSize = DetermineBufferSize(code);
    }


    private int DetermineBufferSize(int weatherCode)
    {
        var type = weatherCode / 1000;

        return type switch
        {
            1 => weatherCode == 1000 ? 0 : 5, // Clear/Sunny vs Partly Cloudy
            2 => 10, // Fog
            4 => weatherCode == 4000 ? 10 : 20, // Drizzle vs Rain
            5 => weatherCode == 5000 ? 15 : 25, // Light Snow vs Heavy Snow
            6 => 30, // Freezing Rain
            7 => 30, // Ice Pellets
            8 => 50, // Thunderstorm
            _ => 0 // Default for unknown
        };
    }

}
