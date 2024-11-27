using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Weather;
using RestSharp;
using Newtonsoft.Json;
using TimeInABottle.Core.Helpers;

namespace TimeInABottle.Core.Services;
internal class ApiWeatherService : IWeatherService
{
    private string _url;
    private string _apiKey;

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

    public async Task FetchTodayWeatherInfosAsync(double latitude, double longitude, string startTime, string endTime)
    {
        var options = new RestClientOptions($"{_url}/timelines?apikey={_apiKey}");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        request.AddHeader("Accept-Encoding", "gzip");
        request.AddJsonBody($"{{\"location\":\"{latitude}, {longitude}\",\"fields\":[\"temperature\",\"weatherCode\"],\"units\":\"metric\",\"timesteps\":[\"1h\"],\"startTime\":\"{startTime}\",\"endTime\":\"{endTime}\"}}", false);
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
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading configuration file.", ex);
        }
    }

    public WeatherInfo getNextHourWeatherInfo()
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

    // tomorrow.io doesn't provide an api endpoint to fetch weathercode mapping
    public static Dictionary<int, string> WeatherDictionary = new()
    {
        { 0, "Unknown" },
        { 1000, "Clear, Sunny" },
        { 1100, "Mostly Clear" },
        { 1101, "Partly Cloudy" },
        { 1102, "Mostly Cloudy" },
        { 1001, "Cloudy" },
        { 2000, "Fog" },
        { 2100, "Light Fog" },
        { 4000, "Drizzle" },
        { 4001, "Rain" },
        { 4200, "Light Rain" },
        { 4201, "Heavy Rain" },
        { 5000, "Snow" },
        { 5001, "Flurries" },
        { 5100, "Light Snow" },
        { 5101, "Heavy Snow" },
        { 6000, "Freezing Drizzle" },
        { 6001, "Freezing Rain" },
        { 6200, "Light Freezing Rain" },
        { 6201, "Heavy Freezing Rain" },
        { 7000, "Ice Pellets" },
        { 7101, "Heavy Ice Pellets" },
        { 7102, "Light Ice Pellets" },
        { 8000, "Thunderstorm" }
    };

}
