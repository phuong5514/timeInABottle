using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Services;
using TimeInABottle.Core.Helpers;

namespace TimeInABottle.Background;
public sealed class WeatherDataFetchingBackgroundTask : IBackgroundTask
{
    private BackgroundTaskDeferral? _taskDeferral;
    private IWeatherService _apiWeatherService;

    public WeatherDataFetchingBackgroundTask()
    {
        _apiWeatherService = ApiWeatherService.Instance;
    }

    public void Run(IBackgroundTaskInstance taskInstance)
    {
        _taskDeferral = taskInstance.GetDeferral();
        _apiWeatherService.LoadWeatherData();
        _taskDeferral.Complete();
    }
}
