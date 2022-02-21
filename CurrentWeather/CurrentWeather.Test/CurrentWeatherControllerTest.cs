using CurrentWeather.Controllers;
using CurrentWeather.Models.ViewModels;
using CurrentWeather.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CurrentWeather.Test
{
    public class CurrentWeatherControllerTest
    {
        private readonly Mock<IOpenWeatherMapService> _openWeatherMapService;
        private readonly CurrentWeatherController _controller;
        private const string _badRequstErrorMessage = "Invalid city or country name.";

        public CurrentWeatherControllerTest()
        {
            var _logger = new Mock<ILogger<CurrentWeatherController>>();
            _openWeatherMapService = new Mock<IOpenWeatherMapService>();
            _controller = new CurrentWeatherController(_logger.Object, _openWeatherMapService.Object);
        }

        [Fact]
        public async Task GetCurrentWeatherDescriptionWithCorrectInputs_ShouldReturnDataAsync()
        {
            // Arrange
            var expected = new WeatherResponse { City = "london", Country = "uk", WeatherDescription = "good weather" };
            _openWeatherMapService.Setup(s => s.GetOpenWeatherMapInformation("london","uk"))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetCurrentWeatherDescription("london", "uk");
            var actual = ((OkObjectResult)result).Value as WeatherResponse;

            // Assert
            _openWeatherMapService.Verify(r => r.GetOpenWeatherMapInformation("london", "uk"));
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected.City, actual.City);
            Assert.Equal(expected.Country, actual.Country);
            Assert.Equal(expected.WeatherDescription, actual.WeatherDescription);
        }

        [Fact]
        public async Task GetCurrentWeatherDescriptionWithInCorrectInputs_ShouldThrowException()
        {
            // Arrange
            _openWeatherMapService.Setup(s => s.GetOpenWeatherMapInformation("", ""))
                .ReturnsAsync((WeatherResponse)null);

            // Act
            // Assert
            var errorMsg = await Assert.ThrowsAsync<Exception>(() => _controller.GetCurrentWeatherDescription("", ""));
            Assert.Equal(_badRequstErrorMessage, errorMsg.Message);
        }
    }
}
