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
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly IAstronomyMapper AstronomyMapper;
        private readonly IErrorMapper ErrorMapper;

        private CurrentAstronomySummary CurrentAstronomySummary;
        private CurrentForecastSummary CurrentForecastSummary;
        private CurrentForecastAndAstronomySummary CurrentConditionsAndAstronomy;
        private List<ErrorDetails> ErrorList;

        public WeatherForecastController
            (                           
                ILogger<WeatherForecastController> logger, 
                IForecastMapper forecastMapper,
                IAstronomyMapper astronomyMapper,
                IWeatherService weatherService,
                IErrorMapper errorMapper
            )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.ForecastMapper = forecastMapper ?? throw new ArgumentNullException(nameof(forecastMapper));
            this.AstronomyMapper = astronomyMapper ?? throw new ArgumentNullException(nameof(astronomyMapper));
            this.WeatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.ErrorMapper = errorMapper ?? throw new ArgumentNullException(nameof(errorMapper));
            this.ErrorList = new List<ErrorDetails>();
        }

        /// <summary> A summary of the weather forecast for a given location. Also provides the astronomy data for a given location and dateTime </summary>
        /// <param name="locationName"> The location you want to receive the weather and astronomy data for </param>
        /// <param name="locationDT"> The datetime you want for the astronomy in the format yyyy-MM-dd. Note this does not affect the weather forecast data.</param>
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
        [ProducesResponseType(StatusCodes.Status207MultiStatus, Type = typeof(List<ErrorDetails>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Get(string locationName, string locationDT , bool? tempInCelcius)
        {
            try
            {
                if (!tempInCelcius.HasValue) //Set to true if null to avoid breaking existing functionality
                {
                    tempInCelcius = true;
                }

                bool retrievedCurrentConditionsSuccess = await GetCurrentConditions(locationName, tempInCelcius.GetValueOrDefault());

                if (string.IsNullOrWhiteSpace(locationDT)) //Return old model if new param not supplied to breaking existing functionality
                {
                    this.Logger.LogInformation($"[Operation=Get(WeatherForecast)], Message= Null Value Passed for locationDT retrieving weather summary only");

                    if (retrievedCurrentConditionsSuccess)
                    {
                        return new OkObjectResult(this.CurrentForecastSummary);
                    }

                    return new ObjectResult(this.ErrorList) { StatusCode = this.ErrorList.FirstOrDefault().Error.HttpStatusCode };

                }

                bool retrievedAstronomySuccess = await this.GetAstronomy(locationName, locationDT);
                
                if (retrievedCurrentConditionsSuccess && retrievedAstronomySuccess)
                {
                    this.CurrentConditionsAndAstronomy = new CurrentForecastAndAstronomySummary
                    {
                        CurrentForecastSummary = this.CurrentForecastSummary,
                        CurrentAstronomySummary = this.CurrentAstronomySummary
                    };

                    return new OkObjectResult(this.CurrentConditionsAndAstronomy);
                }

                if(!retrievedCurrentConditionsSuccess && !retrievedAstronomySuccess)
                {
                    return new ObjectResult(this.ErrorList) { StatusCode = 207 };
                }
                    
                else
                {
                    return new ObjectResult(this.ErrorList) { StatusCode = this.ErrorList.FirstOrDefault().Error.HttpStatusCode };
                }
            }

            catch (Exception ex)
            {
                this.Logger.LogError($"[Operation=Get(WeatherForecast)], Status=Failed, Message=Exeception thrown: {ex.Message}");

                return new ObjectResult(ex) {StatusCode = 500 };
            }
        }

        private async Task<bool> GetCurrentConditions(string locationName, bool tempInCelcius)
        {
            HttpResponseMessage currentConditionsResponse = await this.WeatherService.GetCurrentConditions(locationName);

            if (currentConditionsResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=GetCurrentConditions(WeatherForecast)], locationName={locationName}, Status=Success, Message= Successfully retrieved data from Current forecast endpoint");

                this.CurrentForecastSummary = this.ForecastMapper.mapWeatherAPIResponse(await currentConditionsResponse.Content.ReadAsStringAsync(), tempInCelcius);

                return true;
            }
            else
            {
                ErrorDetails errorDetails = this.ErrorMapper.MapErrorDetails(await currentConditionsResponse.Content.ReadAsStringAsync(),"current");
                    
                this.ErrorList.Add(errorDetails);

                this.Logger.LogWarning($"[Operation=GetCurrentConditions(WeatherForecast)], locationName={locationName}, Status=Failed, Message=data retrieval from Current endpoint failed {errorDetails.Error.HttpStatusCode}, { errorDetails.Error.ApiMessage}");

                return false;
            }
        }

        private async Task<bool> GetAstronomy(string locationName, string locationDateTime)
        {
            HttpResponseMessage currentAstronomyResponse = await this.WeatherService.GetAstronomyConditions(locationName, locationDateTime);

            if (currentAstronomyResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=GetAstronomy(WeatherForecast)], locationName={locationName} locationDT={locationDateTime}, Status=Success, Message= Successfully retrieved data from Astronomy endpoint");

                this.CurrentAstronomySummary =  this.AstronomyMapper.mapAstronomyAPIResponse(await currentAstronomyResponse.Content.ReadAsStringAsync());

                return true;
            }
            else
            {
                ErrorDetails errorDetails = this.ErrorMapper.MapErrorDetails(await currentAstronomyResponse.Content.ReadAsStringAsync(), "Astronomy");

                this.ErrorList.Add(errorDetails);

                this.Logger.LogWarning($"[Operation=GetAstronomy(WeatherForecast)], ocationName={locationName} locationDT={locationDateTime}, Status=Failed, Message=data retrieval from Astronomy endpoint failed {errorDetails.Error.HttpStatusCode}, { errorDetails.Error.ApiMessage}");

                return false;
            }
        }
    }
}
