using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CurrentWeather.Models.ApiConfigModels
{
    [ExcludeFromCodeCoverage]
    public class WeatherConfig
    {
        public string APIBaseUrl { get; set; }
        public string Query { get; set; }
        public string APIKey { get; set; }
    }
}
