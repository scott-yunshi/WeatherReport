using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CurrentWeather.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class WeatherResponse
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string WeatherDescription { get; set; }
    }
}
