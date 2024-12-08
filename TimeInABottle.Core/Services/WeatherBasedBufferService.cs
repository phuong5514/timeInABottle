using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;


namespace TimeInABottle.Core.Services;
public class WeatherBasedBufferService : IBufferService
{
    public int BufferSize {
        get; private set;
    }

    public async void LoadBuffer()
    {
        try
        {

            IWeatherService weatherService = new LocalStorageWeatherService();
            await weatherService.LoadWeatherDataAsync();

            var info = weatherService.GetNextHourWeatherInfo();
            if (info == null)
            {
                throw new Exception("No weather info available");
            }

            var code = info.Values.WeatherCode;

            BufferSize = DetermineBufferSize(code);
        }
        catch (Exception)
        {
            BufferSize = 0;
        }
    }

    private int DetermineBufferSize(int weatherCode)
    {
        var type = weatherCode / 1000;

        return type switch
        {
            1 => 5, // Clear/Sunny, Partly Cloudy
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
