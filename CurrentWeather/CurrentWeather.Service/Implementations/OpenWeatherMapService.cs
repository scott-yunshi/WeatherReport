using CurrentWeather.Models.ApiConfigModels;
using CurrentWeather.Models.ViewModels;
using CurrentWeather.Service.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrentWeather.Service.Implementations
{
    public class OpenWeatherMapService : IOpenWeatherMapService
    {
        private readonly IOptions<WeatherConfig> _config;
        private readonly ILogger<OpenWeatherMapService> _logger;
        private HttpClient _httpClient;

        public OpenWeatherMapService(IOptions<WeatherConfig> config, ILogger<OpenWeatherMapService> logger, HttpClient httpClient)
        {
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
        }
        public async Task<WeatherResponse> GetOpenWeatherMapInformation(string city, string country)
        {
            _logger.LogInformation($"Calling GetOpenWeatherMapInformation with city: {city}, country: {country}");

            //Create Http Request Query
            var endpoint = _config.Value.APIBaseUrl + String.Format(_config.Value.Query, city, country, _config.Value.APIKey);

            //Create Http Request Message
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            //Send Http Request Message
            using (var response = await _httpClient.SendAsync(request))
            { 

                var body = await response.Content.ReadAsStringAsync();

                // Dynamically get weather.description
                dynamic obj = JsonConvert.DeserializeObject(body);
                var description = new List<string>();

                _logger.LogInformation($"GetOpenWeatherMapInformation response Status: {response.StatusCode}, body: {body}");

                // When OpenWeatherMap API call NOT success
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error: GetOpenWeatherMapInformation response Status: {response.StatusCode}, body: {body}");

                    throw new Exception(obj.message == null ? "3rd party API not working." : obj.message.ToString());
                }

                foreach (var item in obj.weather)
                {
                    description.Add(item.description.ToString());
                }

                return new WeatherResponse() { 
                    City = city, 
                    Country = country, 
                    WeatherDescription = String.Join(", ", description.ToArray()) };
            }
        }
    }
}
