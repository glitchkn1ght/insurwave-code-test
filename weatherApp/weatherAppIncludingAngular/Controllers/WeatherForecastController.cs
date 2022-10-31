//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using weatherApp.Models.Response;
    using weatherApp.Models.Weather;
    using weatherApp.Service;

    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> Logger;
        private readonly IWeatherService WeatherService;
        private List<ErrorResponse> ErrorList;

        public WeatherForecastController
            (                           
                ILogger<WeatherForecastController> logger, 
                IWeatherService weatherService
            )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.WeatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.ErrorList = new List<ErrorResponse>();
        }

        /// <summary> A summary of the weather forecast for a given location. Also provides the astronomy data for a given location and dateTime </summary>
        /// <param name="locationName"> The location you want to receive the weather and astronomy data for </param>
        /// <param name="includeAstronomy"> Set to true to include astronomy data in response</param>
        /// <param name="tempInCelcius"> A boolean to detemine the temperature format.  True or null = Celcius, False = Fahrenheit</param>
        /// <response code="200">Returns a forecast summary of weather and astronomy data for the location specified.</response>
        /// <response code="400">If the request url is invalid or parameters are incorrect or no matching location found.</response>
        /// <response code="401">If the request is unauthorized e.g. apiKey is missing or invalid.</response>  
        /// <response code="403">If the apiKey has has exceeeded usage limit or has been disabled.</response>  
        /// <response code="500">Interanl application ErrorResponse.</response>  
        [HttpGet("{locationName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentWeatherAndAstronomySummary))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Get(string locationName, bool tempInCelcius = true, bool includeAstronomy = false)
        {
            try
            {
                CurrentWeatherAndAstronomySummary summary = new CurrentWeatherAndAstronomySummary();

                ForecastResponse forecastResponse = await this.WeatherService.GetCurrentConditions(locationName, tempInCelcius);

                if (!forecastResponse.IsSuccess)
                {
                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Status=Failure, Message= Error retrieving weather data from weatherService");

                    return new ObjectResult(forecastResponse.Error) { StatusCode = forecastResponse.Error.Error.HttpStatusCode };
                }

                summary.CurrentForecastSummary = forecastResponse.forecastSummary;

                if (includeAstronomy)
                {
                    AstronomyResponse astronomyResponse = await this.WeatherService.GetAstronomyConditions(locationName);

                    if (!astronomyResponse.IsSuccess)
                    {
                        this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Status=Failure, Message= Error retrieving astronomy data from weatherService");

                        return new ObjectResult(astronomyResponse.Error) { StatusCode = forecastResponse.Error.Error.HttpStatusCode };
                    }

                    summary.CurrentAstronomySummary = astronomyResponse.AstronomySummary;
                }

                return new OkObjectResult(summary);
            }

            catch (Exception ex)
            {
                this.Logger.LogError($"[Operation=Get(WeatherForecast)], Status=Failed, Message=Exeception thrown: {ex.Message}");

                return new ObjectResult(ex) {StatusCode = 500 };
            }
        }   
    }
}
