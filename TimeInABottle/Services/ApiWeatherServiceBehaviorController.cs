using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Storage;
using Newtonsoft.Json;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;
using TimeInABottle.Helpers;

namespace TimeInABottle.Services;
public class ApiWeatherServiceBehaviorController : IBehaviorController
{
    public async Task<bool> CanRunAsync()
    {
        return IsTodayNotRun() || await IsLocationChangeSignificantAsync() || IsTimeLineEmpty();
    }

    private bool IsTimeLineEmpty()
    {
        var storage = App.GetService<IStorageService>();
        var key = "Timeline";
        var value = storage.Read<WeatherTimeline>(key);
        if (value == null || value.Intervals == null || value.StartTime == null || value.EndTime == null || value.TimeStep == null) {
            return true;
        }
        return false;
    }

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
        catch (Exception) {
            return false;
        }
        
        
    }

    public bool CanStop()
    {
        return true;
    }

    public void Update()
    {
        // write to file so that the background task can access it
        var filename = "weather.json";
        var path = Path.Combine(AppContext.BaseDirectory, filename);
        var weatherService = App.GetService<IWeatherService>();
        File.WriteAllText(path, JsonConvert.SerializeObject(weatherService.WeatherTimeline));
    }

    public void UpdateLocation(double newLongtitude, double newLatitude)
    {
        var storage = App.GetService<IStorageService>();
        storage.Write("LastLongtitude", newLongtitude);
        storage.Write("LastLatitude", newLatitude);
    }

    public void UpdateLastRunDate()
    {
        var storage = App.GetService<IStorageService>();
        storage.Write("LastRunDate", DateTime.Now.ToString("yyyy-MM-dd"));
    }

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
            else {

                weatherService.WeatherTimeline = storage.Read<WeatherTimeline>(key);
                // debug only
                //Update();
            }


        }
        else
        {
            weatherService.WeatherTimeline = storage.Read<WeatherTimeline>(key);
            // debug only
            //Update();

        }
    }
}
