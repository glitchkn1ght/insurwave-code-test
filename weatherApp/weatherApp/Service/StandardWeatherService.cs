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

    public interface IWeatherService
    { 
        public Task<HttpResponseMessage> GetCurrentConditions(string locationName);

        public Task<HttpResponseMessage> GetAstronomyConditions(string locationName, string locationDateTime);
    }

    public class StandardWeatherService : IWeatherService
    {
        private readonly HttpClient Client;
        private readonly ConfigSettingsWeatherAPI configSettings;

        public StandardWeatherService(HttpClient httpClient, IOptions<ConfigSettingsWeatherAPI> configWeatherSettings)
        {
            this.Client = httpClient;
            this.configSettings = configWeatherSettings.Value;
            this.Client.BaseAddress = new Uri(configSettings.BaseURL);
        }

        public async Task<HttpResponseMessage> GetCurrentConditions(string locationName)
        {
            var resource = $"{configSettings.CurrentResourceURL}.{configSettings.ContentType}?key={configSettings.APIKey}&q={locationName}&aqi={configSettings.GetAirQualityData}";

            var response = await this.Client.GetAsync(resource);

            return response;
        }

        public async Task<HttpResponseMessage> GetAstronomyConditions(string locationName, string locationDateTime)
        {
            var resource = $"{configSettings.AstronomyResourceURL}.{configSettings.ContentType}?key={configSettings.APIKey}&q={locationName}&adt={locationDateTime}";

            var response = await this.Client.GetAsync(resource);

            return response;
        }
    }
}
