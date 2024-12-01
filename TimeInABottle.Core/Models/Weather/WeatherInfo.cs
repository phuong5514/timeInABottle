using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Weather;
public class WeatherInfo
{
    [JsonPropertyName("startTime")]
    public string StartTime
    {
        get; set;
    }

    [JsonPropertyName("values")]
    public WeatherValues Values { get; set; }
}

public class WeatherValues
{
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("weatherCode")]
    public int WeatherCode { get; set; }
}

public class WeatherTimeline
{
    [JsonPropertyName("timestep")]
    public string TimeStep { get; set; }

    [JsonPropertyName("endTime")]
    public string EndTime { get; set; }

    [JsonPropertyName("startTime")]
    public string StartTime { get; set; }

    [JsonPropertyName("intervals")]
    public List<WeatherInfo> Intervals { get; set; }
}


public class WeatherData
{
    [JsonPropertyName("timelines")]
    public List<WeatherTimeline> Timelines { get; set; }
}


public class WeatherApiResponse
{
    [JsonPropertyName("data")]
    public WeatherData Data
    {
        get; set;
    }
}

