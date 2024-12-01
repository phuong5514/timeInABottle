using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Storage;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;
using TimeInABottle.Helpers;

namespace TimeInABottle.Services;
public class ApiWeatherServiceBehaviorController : IBehaviorController
{
    public bool CanRun()
    {
        return (IsTodayNotRun() || IsLocationChangeSignificant());
    }

    private bool IsTodayNotRun()
    {
        var key = "LastRunDate";
        var storage = App.GetService<IStorageService>();
        var lastRun = storage.Read<string>(key);
        //var settings = ApplicationData.GetDefault().LocalSettings;
        //var lastRun = settings.Values["LastRunDate"] as string;

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

    private bool IsLocationChangeSignificant()
    {
        var (newLatitude, newLongtitude) = App.GetService<ILocationService>().GetCoordinates();
        //var settings = ApplicationData.GetDefault().LocalSettings;
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
        else {
            if (GeoUtils.CalculateDistance((double)lastLongtitude, (double)lastLatitude, newLatitude, newLongtitude) > 1)
            {
                UpdateLocation(newLatitude, newLongtitude);
                return true;

            }
            return false; // Only run if the distance is greater than 1 km
        }

    }

    public bool CanStop() {
        return true;
    }

    public void Update() {
        
    }


    public void UpdateLocation(double newLongtitude, double newLatitude)
    {
        var storage = App.GetService<IStorageService>();
        storage.Write("LastLongtitude", newLongtitude);
        storage.Write("LastLatitude", newLatitude);
        //var settings = ApplicationData.GetDefault().LocalSettings;
        //settings.Values["LastLongtitude"] = newLongtitude.ToString();
        //settings.Values["LastLatitude"] = newLatitude.ToString();
    }

    public void UpdateLastRunDate()
    {
        var storage = App.GetService<IStorageService>();
        storage.Write("LastRunDate", DateTime.Now.ToString("yyyy-MM-dd"));
        //var settings = ApplicationData.GetDefault().LocalSettings;
        //settings.Values["LastRunDate"] = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void Run() {
        var weatherService = App.GetService<IWeatherService>();
        var storage = App.GetService<IStorageService>();

        var key = "Timeline";

        if (CanRun())
        {
            weatherService.LoadWeatherData();
            storage.Write(key, weatherService.WeatherTimeline);
        }
        else
        {
            weatherService.WeatherTimeline = storage.Read<WeatherTimeline>(key);
        }
    }
}
