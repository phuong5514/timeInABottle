using System;
using Windows.Storage;

namespace TimeInABottle.Background;
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

    public static void UpdateLastRunDate()
    {
        var settings = ApplicationData.Current.LocalSettings;
        settings.Values["LastRunDate"] = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
