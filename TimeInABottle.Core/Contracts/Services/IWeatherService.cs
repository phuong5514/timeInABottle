using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Core.Contracts.Services;
public interface IWeatherService
{
    WeatherInfo GetNextHourWeatherInfo();
    void LoadWeatherData();

    public WeatherInfoWrapper GetCurrentWeather();
    public WeatherInfoWrapper GetNextHourWeather();
}
