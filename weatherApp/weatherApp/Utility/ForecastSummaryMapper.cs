using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using weatherApp.Models.Weather;
using Newtonsoft.Json;

namespace weatherApp.Utility
{
    public interface IForecastSummaryMapper 
    {
        public CurrentForecastSummary mapSummaryResponse(CurrentForecast fullForecast);
    }

    public class standardSummaryMapper : IForecastSummaryMapper
    {
        public CurrentForecastSummary mapSummaryResponse(CurrentForecast fullForecast)
        {
            CurrentForecastSummary forecastSummary = new CurrentForecastSummary
            {
                City = fullForecast.WeatherLocation.LocationName,
                Region = fullForecast.WeatherLocation.Region,
                Country = fullForecast.WeatherLocation.Country,
                LocalTime = fullForecast.WeatherLocation.LocalTime,
                Temperature = fullForecast.CurrentConditions.Temperature_Celcius
            };

            return forecastSummary;
        }
    }
}
