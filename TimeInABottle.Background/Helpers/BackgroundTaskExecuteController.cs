using System;
using Windows.Storage;

namespace TimeInABottle.Background.Helpers;
public static class BackgroundTaskExecuteController
{
    public static bool ShouldRunToday()
    {
        var settings = ApplicationData.Current.LocalSettings;
        var lastRun = settings.Values["LastRunDate"] as string;

        if (!string.IsNullOrEmpty(lastRun) && DateTime.TryParse(lastRun, out var lastRunDate))
        {
            return lastRunDate.Date != DateTime.Now.Date; // Only run if it's a new day
        }

        return true; // Run if no record exists
    }

    public static bool ShouldUpdateLocation(double newLongtitude, double newLatitude)
    {
        var settings = ApplicationData.Current.LocalSettings;

        var lastLongtitude = settings.Values["LastLongtitude"] as string;
        var lastLatitude = settings.Values["LastLatitude"] as string;

        if (!string.IsNullOrEmpty(lastLongtitude) && !string.IsNullOrEmpty(lastLatitude) &&
            double.TryParse(lastLongtitude, out var lastLong) && double.TryParse(lastLatitude, out var lastLat))
        {
            return GeoUtils.CalculateDistance(lastLat, lastLong, newLatitude, newLongtitude) > 1; // Only run if the distance is greater than 1 km
        }

        return true; // Run if no record exists
    }

    public static void UpdateLocation(double newLongtitude, double newLatitude)
    {
        var settings = ApplicationData.Current.LocalSettings;
        settings.Values["LastLongtitude"] = newLongtitude.ToString();
        settings.Values["LastLatitude"] = newLatitude.ToString();
    }

    public static void UpdateLastRunDate()
    {
        var settings = ApplicationData.Current.LocalSettings;
        settings.Values["LastRunDate"] = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
