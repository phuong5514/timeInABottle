using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Core.Services;
/// <summary>
/// Service for loading weather data from local storage.
/// </summary>
internal class LocalStorageWeatherService : IWeatherService
{
    /// <summary>
    /// Gets or sets the weather timeline.
    /// </summary>
    public WeatherTimeline WeatherTimeline
    {
        get; set;
    }

    /// <summary>
    /// Asynchronously loads weather data from local storage.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the data was loaded successfully.</returns>
    public Task<bool> LoadWeatherDataAsync()
    {
        var filename = "weather.json";
        // AppContext.BaseDirectory is null in debug mode?
        var path = Path.Combine(AppContext.BaseDirectory, filename);
        var jsonString = File.ReadAllText(path);
        WeatherTimeline = JsonConvert.DeserializeObject<WeatherTimeline>(jsonString);
        return Task.FromResult(true);
    }
}
