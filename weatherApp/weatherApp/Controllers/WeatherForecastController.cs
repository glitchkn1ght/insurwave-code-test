//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Controllers
{
    using System.Threading.Tasks;
    using weatherApp.Models.Weather;
    using weatherApp.Models;
    using weatherApp.Models.Response;
    using weatherApp.Service;
    using weatherApp.Utility;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore.Http;

    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> Logger;
        private readonly IWeatherService WeatherService;
        private readonly IForecastMapper ForecastMapper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecastMapper forecastMapper , IWeatherService weatherService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.ForecastMapper = forecastMapper ?? throw new ArgumentNullException(nameof(forecastMapper));
            this.WeatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }

        /// <summary> A weather forecast for a given location </summary>
        /// <response code="200">Returns a forecast summary for the location specified</response>
        /// <response code="401">If the request is unauthorized</response>  
        [HttpGet("{locationName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentForecastSummary))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
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
                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], locationName={locationName}, Status=Success, Message= Successfully retrieved current forecase data from API, transforming payload.");

                    CurrentForecastSummary forecastSummary = this.ForecastMapper.mapWeatherAPIResponse(await response.Content.ReadAsStringAsync());

                    return Ok(forecastSummary);
                }
                else
                {
                    ErrorDetails errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(await response.Content.ReadAsStringAsync());

                    this.Logger.LogWarning($"[Operation=Get(WeatherForecast)], Status=Failed, Message=non success code received from API {errorDetails.error.code}, {errorDetails.error.message}");

                    return StatusCode((int)errorDetails.error.HttpStatusCode, errorDetails.error.message);
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
