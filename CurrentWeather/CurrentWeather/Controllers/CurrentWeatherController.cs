using CurrentWeather.Common;
using CurrentWeather.Common.ValidationHelper;
using CurrentWeather.Models.ViewModels;
using CurrentWeather.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CurrentWeather.Controllers
{
    [Route(EndpointConstants.CurrentWeatherBaseRoute)]
    public class CurrentWeatherController : ControllerBase
    {

        private readonly ILogger<CurrentWeatherController> _logger;
        private readonly IOpenWeatherMapService _OpenWeatherMapService;
        public CurrentWeatherController(ILogger<CurrentWeatherController> logger, IOpenWeatherMapService OpenWeatherMapService)
        {
            _logger = logger;
            _OpenWeatherMapService = OpenWeatherMapService;
        }

        [HttpGet("weather")]
        [ApiKeyAuth]
        public async Task<IActionResult> GetCurrentWeatherDescription( string city, string country)
        {
            _logger.LogInformation($"Calling GetCurrentWeatherDescription endpoint with city: {city}, country: {country}");

            if (!(city.Length > 0 && Regex.IsMatch(city, "^[a-zA-Z]*$") &&
                  country.Length > 0 && Regex.IsMatch(country, "^[a-zA-Z]*$")))
            {
                throw new Exception("Invalid city or country name.");
            }

            var result = await _OpenWeatherMapService.GetOpenWeatherMapInformation(city, country);
            return Ok(result);
        }
    }
}
