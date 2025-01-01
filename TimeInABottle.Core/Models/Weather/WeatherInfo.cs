
using System.Text.Json.Serialization;

namespace TimeInABottle.Core.Models.Weather;
/// <summary>
/// Represents weather information for a specific time interval.
/// </summary>
public class WeatherInfo
{
    /// <summary>
    /// Gets or sets the start time of the weather interval.
    /// </summary>
    [JsonPropertyName("startTime")]
    public string StartTime { get; set; }

    /// <summary>
    /// Gets or sets the weather values for the interval.
    /// </summary>
    [JsonPropertyName("values")]
    public WeatherValues Values { get; set; }
}

/// <summary>
/// Represents the weather values such as temperature and weather code.
/// </summary>
public class WeatherValues
{
    /// <summary>
    /// Gets or sets the temperature value.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    /// <summary>
    /// Gets or sets the weather code.
    /// </summary>
    [JsonPropertyName("weatherCode")]
    public int WeatherCode { get; set; }
}

/// <summary>
/// Represents a timeline of weather data.
/// </summary>
public class WeatherTimeline
{
    /// <summary>
    /// Gets or sets the time step of the timeline.
    /// </summary>
    [JsonPropertyName("timestep")]
    public string TimeStep { get; set; }

    /// <summary>
    /// Gets or sets the end time of the timeline.
    /// </summary>
    [JsonPropertyName("endTime")]
    public string EndTime { get; set; }

    /// <summary>
    /// Gets or sets the start time of the timeline.
    /// </summary>
    [JsonPropertyName("startTime")]
    public string StartTime { get; set; }

    /// <summary>
    /// Gets or sets the list of weather intervals in the timeline.
    /// </summary>
    [JsonPropertyName("intervals")]
    public List<WeatherInfo> Intervals { get; set; }
}


/// <summary>
/// Represents the weather data containing multiple timelines.
/// </summary>
public class WeatherData
{
    /// <summary>
    /// Gets or sets the list of weather timelines.
    /// </summary>
    [JsonPropertyName("timelines")]
    public List<WeatherTimeline> Timelines { get; set; }
}


/// <summary>
/// Represents the response from the weather API.
/// </summary>
public class WeatherApiResponse
{
    /// <summary>
    /// Gets or sets the weather data.
    /// </summary>
    [JsonPropertyName("data")]
    public WeatherData Data { get; set; }
}

