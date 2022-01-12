using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using weatherApp.Models.Weather;
using weatherApp.Models;
using weatherApp.Service;

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
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.Config = config ?? throw new ArgumentNullException(nameof(config));
            this.WeatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }

        [HttpGet("{locationName}")]
        public async Task<IActionResult> Get(string locationName)
        {
            if (string.IsNullOrEmpty(locationName))
            {
                this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Status=Failed, Message=locationName is null or empty");

                return BadRequest();
            }

            try
            {
                HttpResponseMessage response = await this.WeatherService.GetCurrentConditions(locationName);

                if (response.IsSuccessStatusCode)
                {
                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], locationName={locationName}, Status=Success, Message= Successfully retrieved current forecase data from API");

                    WeatherForecast fullForecast = JsonConvert.DeserializeObject<WeatherForecast>(await response.Content.ReadAsStringAsync());

                    CurrentForecast forecast = new CurrentForecast
                    {
                        City = fullForecast.WeatherLocation.LocationName,
                        Region = fullForecast.WeatherLocation.Region,
                        Country = fullForecast.WeatherLocation.Country,
                        LocalTime = fullForecast.WeatherLocation.LocalTime,
                        Temperature = fullForecast.CurrentConditions.Temperature_Celcius
                    };

                    return Ok(forecast);
                }
                else
                {
                    ErrorDetails errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(await response.Content.ReadAsStringAsync());

                    this.Logger.LogWarning($"[Operation=Get(WeatherForecast)], Status=Failed, Message=non success code received from API {errorDetails.error.code}, {errorDetails.error.message}");

                    return StatusCode((int)response.StatusCode);
                }

            }

            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message.ToString());

                return StatusCode(500);
            }
        }
    }
}
