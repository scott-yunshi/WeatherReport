using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CurrentWeather.Test
{
    public class CurrentWeatherAPIRateLimitTest
    {
        [Fact]
        public async Task CurrentWeatherAPIRateLimitTest_ThrowWhenExceedLimit()
        {
            using var factory = new WebApplicationFactory<Startup>();
            var httpClient = factory.CreateClient();
            var statusCode = -1;

            for (int i = 0; i < 6; i++)
            {
                var response = await httpClient.GetAsync($"api/weather?city=london&&country=uk");
                statusCode = (int)response.StatusCode;
            }

            Assert.Equal(429, statusCode);
        }
    }
}
