//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using Newtonsoft.Json;
    using System;
    using weatherApp.Models.Weather;

    public interface IForecastMapper
   {
        CurrentForecastSummary mapWeatherAPIResponse(string payload, bool TempInCelcius);
    }

    public class StandardForecastMapper : IForecastMapper
    {
       private IForecastSummaryMapper SummaryMapper { get; set; }
        
        public StandardForecastMapper(IForecastSummaryMapper summaryMapper)
        {
            this.SummaryMapper = summaryMapper ?? throw new ArgumentNullException(nameof(summaryMapper));
        }

        public CurrentForecastSummary mapWeatherAPIResponse(string payload, bool TempInCelcius)
        {
            CurrentForecast fullForecast = JsonConvert.DeserializeObject<CurrentForecast>(payload);

            CurrentForecastSummary forecastSummary = this.SummaryMapper.mapSummaryResponse(fullForecast, TempInCelcius);

            return forecastSummary;
        }
    }
}
