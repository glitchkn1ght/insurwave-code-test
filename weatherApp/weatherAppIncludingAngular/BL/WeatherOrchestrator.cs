//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.BusinessLogic
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using weatherApp.Models.Response;
    using weatherApp.Mappers;

    public interface IWeatherOrchestrator
    {
        Task<ForecastResponse> InterpretAPIForecastResponse(HttpResponseMessage currentConditionsResponse, bool tempInCelcius);

        Task<AstronomyResponse> InterpretAPIAstronomyResponse(HttpResponseMessage astronomyConditionsAPIResponse);
    }

    public class StandardWeatherOrchestrator : IWeatherOrchestrator
    {
        private readonly ILogger<StandardWeatherOrchestrator> Logger;
        private readonly IForecastMapper ForecastMapper;
        private readonly IAstronomyMapper AstronomyMapper;
        private readonly IErrorMapper ErrorMapper;

        public StandardWeatherOrchestrator 
            (                           
                ILogger<StandardWeatherOrchestrator> logger, 
                IForecastMapper forecastMapper,
                IAstronomyMapper astronomyMapper,
                IErrorMapper errorMapper
            )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            this.ForecastMapper = forecastMapper ?? throw new ArgumentNullException(nameof(forecastMapper));
            this.AstronomyMapper = astronomyMapper ?? throw new ArgumentNullException(nameof(astronomyMapper));
            this.ErrorMapper = errorMapper ?? throw new ArgumentNullException(nameof(errorMapper));
        }

        public async Task<ForecastResponse> InterpretAPIForecastResponse(HttpResponseMessage currentConditionsResponse, bool tempInCelcius)
        {
            ForecastResponse forecastResponse = new ForecastResponse();

            if (currentConditionsResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=interpretAPIForecastResponse], Status=Success, Message=Success code received from Current forecast endpoint, mapping data.");

                forecastResponse.forecastSummary = await this.ForecastMapper.mapWeatherAPIResponse(currentConditionsResponse, tempInCelcius);

                forecastResponse.IsSuccess = true;
            }
            else
            {

                
                forecastResponse.Error = await this.ErrorMapper.MapError(currentConditionsResponse, "current");

                this.Logger.LogWarning($"[Operation=InterpretAPIForecastResponse], Status=Failed, Message=Non success code received from Current forecast endpoint, details: { forecastResponse.Error.Error.HttpStatusCode}, {forecastResponse.Error.Error.ApiMessage}");

                forecastResponse.IsSuccess = false;
            }

            return forecastResponse;
        }

        public async Task<AstronomyResponse> InterpretAPIAstronomyResponse(HttpResponseMessage astronomyConditionsAPIResponse)
        {
            AstronomyResponse astronomyResponse = new AstronomyResponse();

            if (astronomyConditionsAPIResponse.IsSuccessStatusCode)
            {
                astronomyResponse.IsSuccess = true;

                this.Logger.LogInformation($"[Operation=InterpretAPIAstronomyResponse], Status=Success, Message= Success code received from Astronmy forecast endpoint, mapping data.");

                astronomyResponse.AstronomySummary = await this.AstronomyMapper.mapAstronomyAPIResponse(astronomyConditionsAPIResponse);
            }
            else
            {
                astronomyResponse.Error = await this.ErrorMapper.MapError(astronomyConditionsAPIResponse, "current");

                this.Logger.LogWarning($"[Operation=InterpretAPIAstronomyResponse], Status=Failed, Message=Non success code received from Astronmy forecast endpoint, details: { astronomyResponse.Error.Error.HttpStatusCode}, {astronomyResponse.Error.Error.ApiMessage}");
            }

            return astronomyResponse;
        }
    }
}
