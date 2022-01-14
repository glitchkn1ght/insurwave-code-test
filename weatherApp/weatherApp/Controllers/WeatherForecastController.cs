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
    using System.Collections.Generic;
    using System.Linq;

    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> Logger;
        private readonly IWeatherService WeatherService;
        private readonly IForecastMapper ForecastMapper;
        private readonly IErrorMapper ErrorMapper;

        private CurrentAstronomy CurrentAstronomy;
        private CurrentForecastSummary CurrentForecastSummary;
        private CurrentConditionsAndAstronomy CurrentConditionsAndAstronomy;
        private List<ErrorDetails> ErrorList;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecastMapper forecastMapper , IWeatherService weatherService, IErrorMapper errorMapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.ForecastMapper = forecastMapper ?? throw new ArgumentNullException(nameof(forecastMapper));
            this.WeatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.ErrorMapper = errorMapper ?? throw new ArgumentNullException(nameof(errorMapper));
        }

        /// <summary> A summary of the weather forecast for a given location. Also provides the astronomy data for a given location and dateTime </summary>
        /// <param name="locationName"> The location you want to receive the weather and astronomy data for </param>
        /// <param name="locationDateTime"> The datetime you want for the astronomy in the format yyyy-MM-dd. Note this does not affect the weather forecast data.</param>
        /// <response code="200">Returns a forecast summary for the location specified.</response>
        /// <response code="207">If there are multiple failure status codes from different parts of the request, returns a list of the errors.</response>
        /// <response code="400">If the request url is invalid or parameters are incorrect or no matching location found.</response>
        /// <response code="401">If the request is unauthorized e.g. apiKey is missing or invalid.</response>  
        /// <response code="403">If the apiKey has has exceeeded usage limit or has been disabled.</response>  
        /// <response code="500">Interanl application error.</response>  
        [HttpGet("{locationName}/{locationDateTime}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentConditionsAndAstronomy))]
        [ProducesResponseType(StatusCodes.Status207MultiStatus, Type = typeof(List<ErrorDetails>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Get(string locationName, string locationDateTime)
        {
            try
            {
                this.ErrorList = new List<ErrorDetails>();

                bool retrivedCurrentConditionsSuccess = await GetCurrentConditions(locationName);
                bool retrievedAstronomySuccess = await this.GetAstronomy(locationName, locationDateTime);
                
                if (retrivedCurrentConditionsSuccess && retrievedAstronomySuccess)
                {
                    this.CurrentConditionsAndAstronomy = new CurrentConditionsAndAstronomy
                    {
                        CurrentForecastSummary = this.CurrentForecastSummary,
                        CurrentAstronomy = this.CurrentAstronomy
                    };

                    return new OkObjectResult(this.CurrentConditionsAndAstronomy);
                }

                if(!retrivedCurrentConditionsSuccess && !retrievedAstronomySuccess)
                {
                    return new ObjectResult(this.ErrorList) { StatusCode = 207 };
                }
                    
                else
                {
                    return new ObjectResult(this.ErrorList) { StatusCode = this.ErrorList.FirstOrDefault().error.HttpStatusCode };
                }
                
            }

            catch (Exception ex)
            {
                this.Logger.LogError($"[Operation=Get(WeatherForecast)], Status=Failed, Message=Exeception thrown: {ex.Message}");

                return new ObjectResult(ex) {StatusCode = 500 };
            }
        }

        private async Task<bool> GetCurrentConditions(string locationName)
        {
            HttpResponseMessage currentConditionsResponse = await this.WeatherService.GetCurrentConditions(locationName);

            if (currentConditionsResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=GetCurrentConditions(WeatherForecast)], locationName={locationName}, Status=Success, Message= Successfully retrieved data from Current forecast endpoint");

                this.CurrentForecastSummary = this.ForecastMapper.mapWeatherAPIResponse(await currentConditionsResponse.Content.ReadAsStringAsync());

                return true;
            }
            else
            {
                ErrorDetails errorDetails = this.ErrorMapper.MapErrorDetails(await currentConditionsResponse.Content.ReadAsStringAsync(),"current");
                    
                this.ErrorList.Add(errorDetails);

                this.Logger.LogWarning($"[Operation=GetCurrentConditions(WeatherForecast)], locationName={locationName}, Status=Failed, Message=data retrieval from Current endpoint failed {errorDetails.error.HttpStatusCode}, { errorDetails.error.apiMessage}");

                return false;
            }
        }

        private async Task<bool> GetAstronomy(string locationName, string locationDateTime)
        {
            HttpResponseMessage currentAstronomyResponse = await this.WeatherService.GetAstronomyConditions(locationName, locationDateTime);

            if (currentAstronomyResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=GetAstronomy(WeatherForecast)], locationName={locationName} locationDT={locationDateTime}, Status=Success, Message= Successfully retrieved data from Astronomy endpoint");

                this.CurrentAstronomy = JsonConvert.DeserializeObject<CurrentAstronomy>(await currentAstronomyResponse.Content.ReadAsStringAsync());

                return true;
            }
            else
            {
                ErrorDetails errorDetails = this.ErrorMapper.MapErrorDetails(await currentAstronomyResponse.Content.ReadAsStringAsync(), "astronomy");

                this.ErrorList.Add(errorDetails);

                this.Logger.LogWarning($"[Operation=GetAstronomy(WeatherForecast)], ocationName={locationName} locationDT={locationDateTime}, Status=Failed, Message=data retrieval from Astronomy endpoint failed {errorDetails.error.HttpStatusCode}, { errorDetails.error.apiMessage}");

                return false;
            }
        }
    }
}
