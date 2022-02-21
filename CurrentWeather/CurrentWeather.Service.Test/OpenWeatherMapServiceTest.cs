using CurrentWeather.Models.ApiConfigModels;
using CurrentWeather.Service.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CurrentWeather.Service.Test
{
    public class OpenWeatherMapServiceTest
    {
        private const string _apiKey = "8b7535b42fe1c551f18028f64e8688f7";
        private const string _query = "?q={0},{1}&appid={2}";
        private const string _apiBaseUrl = "https://api.openweathermap.org/data/2.5/weather";
        private const string _apiResponse = @"{""weather"": [{""id"": 802,""main"": ""Clouds"",""description"": ""good weather"",""icon"": ""03d""}]}";

        [Fact]
        public async void OpenWeatherMapInformationWithCorrectInput_ShouldReturnData()
        {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_apiResponse),
            };

            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var moqHttpClient = new HttpClient(handlerMock.Object);

            var moqConfig = Options.Create(new WeatherConfig() { 
                APIBaseUrl = _apiBaseUrl,
                Query = _query,
                APIKey = _apiKey
            });
            var moqLogger = Mock.Of<ILogger<OpenWeatherMapService>>();

            //Action
            var getWeatherData = new OpenWeatherMapService(moqConfig, moqLogger, moqHttpClient);
            var retrievedPosts = await getWeatherData.GetOpenWeatherMapInformation("london","uk");

            //Assert
            Assert.NotNull(retrievedPosts);
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }
    }
}
