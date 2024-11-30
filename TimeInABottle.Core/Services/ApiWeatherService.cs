using System.Xml.Linq;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;
using RestSharp;
using Newtonsoft.Json;
using TimeInABottle.Core.Helpers;

namespace TimeInABottle.Core.Services;
public class ApiWeatherService : IWeatherService
{
    private string _url;
    private string _apiKey;
    private string _startTime;
    private string _endTime;
    private string _timeZone;


    private WeatherTimeline _weatherTimeline;

    // implement singleton pattern
    private static readonly Lazy<ApiWeatherService> instance = new(() => new ApiWeatherService());

    private ApiWeatherService() {
        // read secret.config file to get the API key and base URL
        ReadApiConfig();
    }

    public static ApiWeatherService Instance => instance.Value;

    public WeatherInfo TodayWeatherInfos
    {
        get; set;
    }

    private async Task FetchTodayWeatherInfosAsync()
    {
        var (latitude, longitude) = await GeoLocationFetcher.GetCoordinatesAsync();

        var options = new RestClientOptions($"{_url}/timelines?apikey={_apiKey}");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        request.AddHeader("Accept-Encoding", "gzip");

        request.AddJsonBody($"{{\"location\":\"{latitude}, {longitude}\",\"fields\":[\"temperature\",\"weatherCode\"],\"units\":\"metric\",\"timesteps\":[\"1h\"],\"startTime\":\"{_startTime}\",\"endTime\":\"{_endTime}\",\"timezone\":\"auto\"}}", false);
        var response = await client.PostAsync(request);

        var weatherApiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(response.Content);
        _weatherTimeline = weatherApiResponse.Data;
    }

    private void ReadApiConfig()
    {
        var configPath = "secret.config";

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
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading configuration file.", ex);
        }
    }

    public WeatherInfo GetNextHourWeatherInfo()
    {
        DateTime now = DateTime.Now;
        foreach (var weather in _weatherTimeline.Intervals) {
            DateTime weatherDateTime = DateTime.Parse(weather.Time);
            if (weatherDateTime > now) {
                return weather;
            }
        }

        return null;
    }

    public async void LoadWeatherData() {
        await FetchTodayWeatherInfosAsync();
    }

    public WeatherInfoWrapper GetCurrentWeather() { 
        DateTime now = DateTime.Now;
        foreach (var weather in _weatherTimeline.Intervals)
        {
            DateTime weatherDateTime = DateTime.Parse(weather.Time);
            // check if now and weatherDateTime is in the same hour
            if (now.Hour == weatherDateTime.Hour)
            {
                return new WeatherInfoWrapper(weather);
            }
        }
        return null;
    }

    public WeatherInfoWrapper GetNextHourWeather() {
        DateTime now = DateTime.Now;
        foreach (var weather in _weatherTimeline.Intervals)
        {
            DateTime weatherDateTime = DateTime.Parse(weather.Time);
            if (weatherDateTime > now)
            {
                return new WeatherInfoWrapper(weather);
            }
        }
        return null;
    }

    // tomorrow.io doesn't provide an api endpoint to fetch weathercode mapping


}
