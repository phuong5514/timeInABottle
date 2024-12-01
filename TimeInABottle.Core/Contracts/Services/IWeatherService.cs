using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Core.Contracts.Services;
public interface IWeatherService
{

    public WeatherTimeline WeatherTimeline
    {
        get; set;
    }

    WeatherInfo GetNextHourWeatherInfo();
    void LoadWeatherData();

    public WeatherInfoWrapper GetCurrentWeather();
    public WeatherInfoWrapper GetNextHourWeather();
}
