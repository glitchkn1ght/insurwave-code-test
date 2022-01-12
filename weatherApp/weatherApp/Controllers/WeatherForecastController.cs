using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using weatherApp.Service;
using System.Threading.Tasks;
using weatherApp.Models.Weather;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace AngularApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> Logger;
        private readonly IWeatherService WeatherService;
        private readonly IConfiguration Config;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config, IWeatherService weatherService)
        {
            this.Logger = logger;
            this.Config = config;
            this.WeatherService = weatherService;
        }

        [HttpGet("{locationName}")]
        public async Task<CurrentForecast> Get(string locationName)
        {
            try
            {
                HttpResponseMessage response = await this.WeatherService.GetCurrentConditions(locationName);

                if (response.IsSuccessStatusCode)
                {
                    WeatherForecast fullForecast = JsonConvert.DeserializeObject<WeatherForecast>(await response.Content.ReadAsStringAsync());

                    CurrentForecast forecast = new CurrentForecast
                    {
                        City = fullForecast.WeatherLocation.LocationName,
                        Region = fullForecast.WeatherLocation.Region,
                        Country = fullForecast.WeatherLocation.Country,
                        LocalTime = fullForecast.WeatherLocation.LocalTime,
                        Temperature = fullForecast.CurrentConditions.Temperature_Celcius
                    };

                    return forecast;
                }
                else
                {
                    return new CurrentForecast();
                }
                
            }

            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message.ToString());

                return new CurrentForecast();
            }
        }
    }
}
