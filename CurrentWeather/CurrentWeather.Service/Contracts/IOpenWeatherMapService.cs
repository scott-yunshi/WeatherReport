using CurrentWeather.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrentWeather.Service.Contracts
{
    public interface IOpenWeatherMapService
    {
        Task<WeatherResponse> GetOpenWeatherMapInformation(string city, string country);
    }
}
