//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using weatherApp.Models.Weather;

    public interface IForecastSummaryMapper 
    {
        public CurrentForecastSummary mapSummaryResponse(CurrentForecast fullForecast);
    }

    public class standardSummaryMapper : IForecastSummaryMapper
    {
        public CurrentForecastSummary mapSummaryResponse(CurrentForecast fullForecast)
        {
            CurrentForecastSummary forecastSummary = new CurrentForecastSummary
            {
                City = fullForecast.WeatherLocation.LocationName,
                Region = fullForecast.WeatherLocation.Region,
                Country = fullForecast.WeatherLocation.Country,
                LocalTime = fullForecast.WeatherLocation.LocalTime,
                Temperature = fullForecast.CurrentConditions.Temperature_Celcius
            };

            return forecastSummary;
        }
    }
}
