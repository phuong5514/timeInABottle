using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Core.Contracts.Services;
public interface IWeatherService
{

    public WeatherTimeline WeatherTimeline
    {
        get; set;
    }

    Task<bool> LoadWeatherDataAsync();

    public WeatherInfo GetCurrentWeatherInfo()
    {
        var now = DateTime.Now;

        if (WeatherTimeline == null)
        {
            return null;
        }

        if (WeatherTimeline.Intervals == null)
        {
            return null;
        }

        foreach (var weather in WeatherTimeline.Intervals)
        {
            DateTime weatherDateTime = DateTime.Parse(weather.StartTime);
            // check if now and weatherDateTime is in the same hour
            if (now.Hour == weatherDateTime.Hour)
            {
                return weather;
            }
        }
        return null;

    }

    public WeatherInfo GetNextHourWeatherInfo()
    {
        DateTime now = DateTime.Now;
        foreach (var weather in WeatherTimeline.Intervals)
        {
            DateTime weatherDateTime = DateTime.Parse(weather.StartTime);
            if (weatherDateTime > now)
            {
                return weather;
            }
        }

        return null;
    }

    public WeatherInfoWrapper GetCurrentWeather()
    {
        DateTime now = DateTime.Now;

        if (WeatherTimeline == null)
        {
            return null;
        }

        if (WeatherTimeline.Intervals == null)
        {
            return null;
        }

        foreach (var weather in WeatherTimeline.Intervals)
        {
            var weatherDateTime = DateTime.Parse(weather.StartTime);
            // check if now and weatherDateTime is in the same hour
            if (now.Hour == weatherDateTime.Hour)
            {
                return new WeatherInfoWrapper(weather);
            }
        }
        return null;
    }

    public WeatherInfoWrapper GetNextHourWeather()
    {
        var now = DateTime.Now;

        if (WeatherTimeline == null)
        {
            return null;
        }

        if (WeatherTimeline.Intervals == null)
        {
            return null;
        }

        foreach (var weather in WeatherTimeline.Intervals)
        {
            var weatherDateTime = DateTime.Parse(weather.StartTime);
            if (weatherDateTime > now)
            {
                return new WeatherInfoWrapper(weather);
            }
        }
        return null;
    }


}
