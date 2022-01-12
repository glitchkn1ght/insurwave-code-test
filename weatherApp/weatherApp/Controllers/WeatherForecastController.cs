using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularApp.Models.Weather;
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
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _config;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
        {
            this._logger = logger;
            this._config = config;
        }

        [HttpGet]
        public async Task<CurrentForecast> Get()
        {
            try
            {
                string baseURL = _config.GetValue<string>("WeatherApi:BaseUrl");
                string apiKey = _config.GetValue<string>("WeatherApi:APIKey");


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync($"current.json?key={apiKey}&q=London&aqi=no");

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
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());

                return new CurrentForecast();
            }
        }
    }
}
