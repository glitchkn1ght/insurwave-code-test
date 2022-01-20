//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Service
{
    using Microsoft.Extensions.Options;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using weatherApp.Models.Configuration;
    using weatherApp.Utility;
    using weatherApp.BusinessLogic;
    using weatherApp.Models.Response;

    public interface IWeatherService
    { 
        public Task<ForecastResponse> GetCurrentConditions(string locationName, bool tempInCelcius);

        public Task<AstronomyResponse> GetAstronomyConditions(string locationName);
    }

    public class StandardWeatherService : IWeatherService
    {
        private readonly HttpClient Client;
        private readonly ConfigSettingsWeatherAPI configSettings;
        private readonly IForecastMapper ForecastMapper;
        private readonly IWeatherOrchestrator WeatherOrchestrator;

        public StandardWeatherService(HttpClient httpClient, IOptions<ConfigSettingsWeatherAPI> configWeatherSettings, WeatherOrchestrator weatherOrchestrator)
        {
            this.Client = httpClient;
            this.configSettings = configWeatherSettings.Value;
            this.WeatherOrchestrator = weatherOrchestrator;
            this.Client.BaseAddress = new Uri(configSettings.BaseURL);
        }

        public async Task<ForecastResponse> GetCurrentConditions(string locationName, bool tempInCelcius)
        {
            var resource = $"{configSettings.CurrentResourceURL}.{configSettings.ContentType}?key={configSettings.APIKey}&q={locationName}&aqi={configSettings.GetAirQualityData}";

            ForecastResponse forecastResponse = await this.WeatherOrchestrator.InterpretAPIForecastResponse(await this.Client.GetAsync(resource), tempInCelcius);

            return forecastResponse;
        }

        public async Task<AstronomyResponse> GetAstronomyConditions(string locationName)
        {
            var resource = $"{configSettings.AstronomyResourceURL}.{configSettings.ContentType}?key={configSettings.APIKey}&q={locationName}";

            AstronomyResponse astronomyResponse = await this.WeatherOrchestrator.InterpretAPIAstronomyResponse(await this.Client.GetAsync(resource));

            return astronomyResponse;
        }
    }
}
