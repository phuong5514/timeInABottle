using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Background.Helpers;

namespace TimeInABottle.Background;
public sealed class WeatherDataFetchingBackgroundTask : IBackgroundTask
{
    private BackgroundTaskDeferral? _taskDeferral;
    private readonly IWeatherService _weatherService;

    public WeatherDataFetchingBackgroundTask()
    {
        _weatherService = ApiWeatherService.Instance;
    }

    public void Run(IBackgroundTaskInstance taskInstance)
    {
        _taskDeferral = taskInstance.GetDeferral();
        if (BackgroundTaskExecuteController.ShouldRunToday())
        {
            _weatherService.LoadWeatherData();
            BackgroundTaskExecuteController.UpdateLastRunDate();
        }
        _taskDeferral.Complete();
    }
}
