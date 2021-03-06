//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using weatherApp.Models.Response;
    using weatherApp.Models.Weather;
    using weatherApp.Service;
    using weatherApp.Utility;

    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> Logger;
        private readonly IWeatherService WeatherService;
        private readonly IForecastMapper ForecastMapper;
        private readonly IErrorMapper ErrorMapper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecastMapper forecastMapper , IWeatherService weatherService, IErrorMapper errorMapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.ForecastMapper = forecastMapper ?? throw new ArgumentNullException(nameof(forecastMapper));
            this.WeatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.ErrorMapper = errorMapper ?? throw new ArgumentNullException(nameof(errorMapper));
        }

        /// <summary> A summary of the weather forecast for a given location. </summary>
        /// <param name="locationName"> The location you want to receive the weather data for </param>
        /// <response code="200">Returns a forecast summary for the location specified.</response>
        /// <response code="400">If the request url is invalid or parameters are incorrect or no matching location found.</response>
        /// <response code="401">If the request is unauthorized e.g. apiKey is missing or invalid.</response>  
        /// <response code="403">If the apiKey has has exceeeded usage limit or has been disabled.</response>  
        /// <response code="500">Interanl application error.</response>  
        [HttpGet("{locationName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentForecastSummary))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Get(string locationName)
        {
            try
            {
                HttpResponseMessage response = await this.WeatherService.GetCurrentConditions(locationName);

                if (response.IsSuccessStatusCode)
                {
                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], locationName={locationName}, Status=Success, Message= Successfully retrieved current forecase data from API, transforming payload.");

                    CurrentForecastSummary forecastSummary = this.ForecastMapper.mapWeatherAPIResponse(await response.Content.ReadAsStringAsync());

                    return new OkObjectResult(forecastSummary);
                }
                else
                {
                    ErrorDetails errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(await response.Content.ReadAsStringAsync());

                    errorDetails.error.HttpStatusCode = ErrorMapper.MapApiErrorCode(errorDetails.error.apiCode);

                    this.Logger.LogWarning($"[Operation=Get(WeatherForecast)], Status=Failed, Message=non success code received from API {errorDetails.error.HttpStatusCode}, { errorDetails.error.apiMessage}");

                    return new ObjectResult(errorDetails.error) { StatusCode = errorDetails.error.HttpStatusCode};
                }

            }

            catch (Exception ex)
            {
                this.Logger.LogError($"[Operation=Get(WeatherForecast)], Status=Failed, Message=Exeception thrown: {ex.Message}");

                return new ObjectResult(ex) {StatusCode = 500 };
            }
        }
    }
}
