using TimeInABottle.Core.Contracts.Services;

namespace TimeInABottle.Core.Services;
/// <summary>
/// Service for determining buffer size based on weather conditions.
/// </summary>
public class WeatherBasedBufferService : IBufferService
{
    /// <summary>
    /// Gets the buffer size based on the weather conditions.
    /// </summary>
    public int BufferSize
    {
        get; private set;
    }

    /// <summary>
    /// Loads the buffer size based on the next hour's weather information.
    /// </summary>
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

    /// <summary>
    /// Determines the buffer size based on the weather code.
    /// </summary>
    /// <param name="weatherCode">The weather code.</param>
    /// <returns>The buffer size.</returns>
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
