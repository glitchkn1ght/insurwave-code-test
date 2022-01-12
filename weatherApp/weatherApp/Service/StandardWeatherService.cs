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
    }

    public class StandardWeatherService : IWeatherService
    {
        private readonly HttpClient Client;
        private readonly configSettingsWeatherAPI configSettings;

        public StandardWeatherService(HttpClient httpClient, IOptions<configSettingsWeatherAPI> configWeatherSettings)
        {
            this.Client = httpClient;
            this.configSettings = configWeatherSettings.Value;
        }

        public async Task<HttpResponseMessage> GetCurrentConditions(string locationName)
        {
            this.Client.BaseAddress = new Uri(configSettings.BaseURL);

            var resource = $"{configSettings.ResourceURL}.{configSettings.ContentType}?key={configSettings.APIKey}&q={locationName}&aqi={configSettings.GetAirQualityData}";

            var response = await this.Client.GetAsync(resource);

            return response;
        }
    }
}
