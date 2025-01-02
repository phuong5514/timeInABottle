using Newtonsoft.Json;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;
using TimeInABottle.Helpers;

namespace TimeInABottle.Services;
/// <summary>
/// Controller for managing the behavior of the API weather service.
/// </summary>
public class ApiWeatherServiceBehaviorController : IBehaviorController
{
    /// <summary>
    /// Determines whether the weather service can run.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the service can run.</returns>
    public async Task<bool> CanRunAsync()
    {
        return IsTodayNotRun() || await IsLocationChangeSignificantAsync() || IsTimeLineEmpty();
    }

    /// <summary>
    /// Checks if the weather timeline is empty.
    /// </summary>
    /// <returns>True if the timeline is empty; otherwise, false.</returns>
    private bool IsTimeLineEmpty()
    {
        var storage = App.GetService<IStorageService>();
        var key = "Timeline";
        var value = storage.Read<WeatherTimeline>(key);
        if (value == null || value.Intervals == null || value.StartTime == null || value.EndTime == null || value.TimeStep == null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the weather service has not run today.
    /// </summary>
    /// <returns>True if the service has not run today; otherwise, false.</returns>
    private bool IsTodayNotRun()
    {
        var key = "LastRunDate";
        var storage = App.GetService<IStorageService>();
        var lastRun = storage.Read<string>(key);

        if (!string.IsNullOrEmpty(lastRun) && DateTime.TryParse(lastRun, out var lastRunDate))
        {
            if (lastRunDate.Date != DateTime.Now.Date)
            {
                UpdateLastRunDate();
                return true;
            }
            return false; // Only run if it's a new day
        }

        UpdateLastRunDate();
        return true; // Run if no record exists
    }

    /// <summary>
    /// Checks if the location change is significant.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the location change is significant.</returns>
    private async Task<bool> IsLocationChangeSignificantAsync()
    {
        try
        {
            var (newLatitude, newLongtitude) = await App.GetService<ILocationService>().GetCoordinatesAsync();
            var storage = App.GetService<IStorageService>();

            var keyLongtitude = "LastLongtitude";
            var keyLatitude = "LastLatitude";

            var lastLongtitude = storage.Read<double?>(keyLongtitude);
            var lastLatitude = storage.Read<double?>(keyLatitude);

            if (lastLongtitude == null || lastLatitude == null)
            {
                UpdateLocation(newLatitude, newLongtitude);
                return true;
            }
            else
            {
                if (GeoUtils.CalculateDistance((double)lastLongtitude, (double)lastLatitude, newLatitude, newLongtitude) > 1)
                {
                    UpdateLocation(newLatitude, newLongtitude);
                    return true;
                }
                return false; // Only run if the distance is greater than 1 km
            }

        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether the weather service can stop.
    /// </summary>
    /// <returns>True if the service can stop; otherwise, false.</returns>
    public bool CanStop()
    {
        return true;
    }

    /// <summary>
    /// Updates the weather data and writes it to a file.
    /// </summary>
    public void Update()
    {
        // write to file so that the background task can access it
        var filename = "weather.json";
        var path = Path.Combine(AppContext.BaseDirectory, filename);
        var weatherService = App.GetService<IWeatherService>();
        File.WriteAllText(path, JsonConvert.SerializeObject(weatherService.WeatherTimeline));
    }

    /// <summary>
    /// Updates the stored location coordinates.
    /// </summary>
    /// <param name="newLongtitude">The new longitude.</param>
    /// <param name="newLatitude">The new latitude.</param>
    public void UpdateLocation(double newLongtitude, double newLatitude)
    {
        var storage = App.GetService<IStorageService>();
        storage.Write("LastLongtitude", newLongtitude);
        storage.Write("LastLatitude", newLatitude);
    }

    /// <summary>
    /// Updates the last run date to the current date.
    /// </summary>
    public void UpdateLastRunDate()
    {
        var storage = App.GetService<IStorageService>();
        storage.Write("LastRunDate", DateTime.Now.ToString("yyyy-MM-dd"));
    }

    /// <summary>
    /// Runs the weather service.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RunAsync()
    {
        var weatherService = App.GetService<IWeatherService>();
        var storage = App.GetService<IStorageService>();

        var key = "Timeline";

        if (await CanRunAsync())
        {
            if (await weatherService.LoadWeatherDataAsync())
            {
                storage.Write(key, weatherService.WeatherTimeline);
                Update();
            }
            else
            {
                weatherService.WeatherTimeline = storage.Read<WeatherTimeline>(key);
            }
        }
        else
        {
            weatherService.WeatherTimeline = storage.Read<WeatherTimeline>(key);
        }
    }
}
