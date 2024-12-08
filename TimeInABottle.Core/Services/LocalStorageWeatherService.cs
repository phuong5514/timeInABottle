using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Core.Services;
internal class LocalStorageWeatherService : IWeatherService
{
    public WeatherTimeline WeatherTimeline
    {
        get; set;
    }

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
