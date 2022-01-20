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
        private List<Error> ErrorList;

        public WeatherForecastController
            (                           
                ILogger<WeatherForecastController> logger, 
                IWeatherService weatherService
            )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.WeatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.ErrorList = new List<Error>();
        }

        /// <summary> A summary of the weather forecast for a given location. Also provides the astronomy data for a given location and dateTime </summary>
        /// <param name="locationName"> The location you want to receive the weather and astronomy data for </param>
        /// <param name="includeAstronomy"> Set to true to include astronomy data in response</param>
        /// <param name="tempInCelcius"> A boolean to detemine the temperature format.  True or null = Celcius, False = Fahrenheit</param>
        /// <response code="200">Returns a forecast summary of weather and astronomy data for the location specified.</response>
        /// <response code="206">Returns a forecast summary of weather for the location specified in order to be comptible with users of previous versions of this application</response>
        /// <response code="207">If there are multiple failure status codes from different parts of the request, returns a list of the errors.</response>
        /// <response code="400">If the request url is invalid or parameters are incorrect or no matching location found.</response>
        /// <response code="401">If the request is unauthorized e.g. apiKey is missing or invalid.</response>  
        /// <response code="403">If the apiKey has has exceeeded usage limit or has been disabled.</response>  
        /// <response code="500">Interanl application Error.</response>  
        [HttpGet("{locationName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentForecastAndAstronomySummary))]
        [ProducesResponseType(StatusCodes.Status206PartialContent, Type = typeof(CurrentForecastSummary))]
        [ProducesResponseType(StatusCodes.Status207MultiStatus, Type = typeof(List<Error>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Get(string locationName, bool? tempInCelcius, bool? includeAstronomy)
        {
            try
            {
                if (!tempInCelcius.HasValue) //Set to true if null to avoid breaking existing functionality
                {
                    tempInCelcius = true;
                }

                 ForecastResponse forecastResponse = await this.WeatherService.GetCurrentConditions(locationName, tempInCelcius.GetValueOrDefault());

                if (!includeAstronomy.HasValue) //Set to true if null to avoid breaking existing functionality
                {
                    includeAstronomy = false;
                }

                if (!includeAstronomy.Value) //Return old model if new param not supplied to breaking existing functionality
                {
                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Status=Success, Message= Null Value Passed for includeAstronomy retrieving weather summary only");

                    if (forecastResponse.IsSuccess)
                    {
                        return new OkObjectResult(forecastResponse.forecastSummary);
                    }

                    return new ObjectResult(forecastResponse.Error) { StatusCode = forecastResponse.Error.ErrorDetails.HttpStatusCode };
                }

                AstronomyResponse astronomyResponse = await this.WeatherService.GetAstronomyConditions(locationName);

                if (forecastResponse.IsSuccess && astronomyResponse.IsSuccess)
                {
                    CurrentForecastAndAstronomySummary summary = new CurrentForecastAndAstronomySummary
                    {
                        CurrentForecastSummary = forecastResponse.forecastSummary,
                        CurrentAstronomySummary = astronomyResponse.AstronomySummary
                    };

                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Status=Success, Message= Succesfully retrieved weather and astronomy data from weatherService");

                    return new OkObjectResult(summary);
                }

                if (!forecastResponse.IsSuccess)
                {
                    this.ErrorList.Add(forecastResponse.Error);

                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Status=Failure, Message= Error retrieving weather data from weatherService");
                }

                if (!astronomyResponse.IsSuccess)
                {
                    this.ErrorList.Add(astronomyResponse.Error);

                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Status=Failure, Message= Error retrieving astronomy data from weatherService");
                }

                return new ObjectResult(this.ErrorList) { StatusCode = 207 };

            }

            catch (Exception ex)
            {
                this.Logger.LogError($"[Operation=Get(WeatherForecast)], Status=Failed, Message=Exeception thrown: {ex.Message}");

                return new ObjectResult(ex) {StatusCode = 500 };
            }
        }   
    }
}
