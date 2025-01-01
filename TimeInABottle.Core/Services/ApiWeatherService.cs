using System.Xml.Linq;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;
using RestSharp;
using Newtonsoft.Json;

namespace TimeInABottle.Core.Services;
/// <summary>
/// Service for fetching weather data from an API.
/// </summary>
public class ApiWeatherService : IWeatherService
{
    private string _url;
    private string _apiKey;
    private string _startTime;
    private string _endTime;
    private string _timeZone;

    private double _latitude;
    private double _longitude;

    /// <summary>
    /// Gets or sets the weather timeline.
    /// </summary>
    public WeatherTimeline WeatherTimeline
    {
        get; set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiWeatherService"/> class.
    /// </summary>
    public ApiWeatherService()
    {
        // read secret.config file to get the API key and base URL
        ReadApiConfig();
    }

    /// <summary>
    /// Asynchronously loads weather data from the API.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the data was loaded successfully.</returns>
    public async Task<bool> LoadWeatherDataAsync()
    {
        try
        {
            var options = new RestClientOptions($"{_url}/timelines?apikey={_apiKey}");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Accept-Encoding", "gzip");

            request.AddJsonBody($"{{\"location\":\"{_latitude}, {_longitude}\",\"fields\":[\"temperature\",\"weatherCode\"],\"units\":\"metric\",\"timesteps\":[\"1h\"],\"startTime\":\"{_startTime}\",\"endTime\":\"{_endTime}\",\"timezone\":\"auto\"}}", false);
            var response = await client.PostAsync(request);

            var weatherApiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(response.Content);
            var data = weatherApiResponse.Data;
            WeatherTimeline = data.Timelines.First();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Reads the API configuration from the secret.config file.
    /// </summary>
    /// <exception cref="FileNotFoundException">Thrown when the configuration file is not found.</exception>
    /// <exception cref="Exception">Thrown when there is an error reading the configuration file.</exception>
    private void ReadApiConfig()
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "secret.config");

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Configuration file not found.");
        }

        try
        {
            // get today string in the format of "yyyy-MM-dd"
            var todayString = DateTime.Now.ToString("yyyy-MM-dd");

            // Load the XML file
            var config = XElement.Load(configPath);

            // Read the API key
            _apiKey = config.Element("WeatherApi")?.Element("ApiKey")?.Value;
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API Key not found in configuration file.");
            }

            // Read the BaseAddress
            _url = config.Element("WeatherApi")?.Element("BaseAddress")?.Value;
            if (string.IsNullOrEmpty(_url))
            {
                throw new Exception("BaseAddress not found in configuration file.");
            }

            // Read the TimeZone
            _timeZone = config.Element("WeatherApi")?.Element("TimeZone")?.Value;
            if (string.IsNullOrEmpty(_timeZone))
            {
                throw new Exception("TimeZone not found in configuration file.");
            }

            // Read the StartTime
            var startTimeString = config.Element("WeatherApi")?.Element("StartTime")?.Value;
            if (string.IsNullOrEmpty(startTimeString))
            {
                throw new Exception("StartTime not found in configuration file.");
            }
            _startTime = $"{todayString}T{startTimeString}{_timeZone}";

            // Read the EndTime
            var endTimeString = config.Element("WeatherApi")?.Element("EndTime")?.Value;
            if (string.IsNullOrEmpty(endTimeString))
            {
                throw new Exception("EndTime not found in configuration file.");
            }
            _endTime = $"{todayString}T{endTimeString}{_timeZone}";

            // Read the Latitude
            var latitudeString = config.Element("WeatherApi")?.Element("Latitude")?.Value;
            if (string.IsNullOrEmpty(latitudeString))
            {
                throw new Exception("Latitude not found in configuration file.");
            }
            _latitude = double.Parse(latitudeString);

            // Read the Longitude
            var longitudeString = config.Element("WeatherApi")?.Element("Longitude")?.Value;
            if (string.IsNullOrEmpty(longitudeString))
            {
                throw new Exception("Longitude not found in configuration file.");
            }
            _longitude = double.Parse(longitudeString);
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading configuration file.", ex);
        }
    }
}
