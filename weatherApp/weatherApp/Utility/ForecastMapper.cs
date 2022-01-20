//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using weatherApp.Models.Weather;

    public interface IForecastMapper
   {
        Task<CurrentForecastSummary> mapWeatherAPIResponse(HttpResponseMessage httpResponse, bool TempInCelcius);
    }

    public class StandardForecastMapper : IForecastMapper
    {
       private IForecastSummaryMapper SummaryMapper { get; set; }
        
        public StandardForecastMapper(IForecastSummaryMapper summaryMapper)
        {
            this.SummaryMapper = summaryMapper ?? throw new ArgumentNullException(nameof(summaryMapper));
        }

        public async Task<CurrentForecastSummary> mapWeatherAPIResponse(HttpResponseMessage httpResponse, bool TempInCelcius)
        {
            CurrentForecast fullForecast = JsonConvert.DeserializeObject<CurrentForecast>(await httpResponse.Content.ReadAsStringAsync());

            CurrentForecastSummary forecastSummary = this.SummaryMapper.mapSummaryResponse(fullForecast, TempInCelcius);

            return forecastSummary;
        }
    }
}
