using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Weather;
public class WeatherInfo
{
    public string Time
    {
        get; set;
    }

    public WeatherValues Values
    {
        get; set;
    }
}

public class WeatherValues
{
    public double Temperature
    {
        get; set;
    }

    public int WeatherCode
    {
        get; set;
    }
}

public class WeatherTimeline
{
    public string TimeStep
    {
        get; set;
    }

    public string EndTime
    {
        get; set;
    }

    public string StartTime
    {
        get; set;
    }

    public List<WeatherInfo> Intervals
    {
        get; set;
    }
}

public class WeatherApiResponse
{
    public WeatherTimeline Data
    {
        get; set;
    }
}