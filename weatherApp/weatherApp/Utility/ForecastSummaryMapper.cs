//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using Microsoft.Extensions.Options;
    using weatherApp.Models.Configuration;
    using weatherApp.Models.Weather;

    public interface IForecastSummaryMapper 
    {
        public CurrentForecastSummary mapSummaryResponse(CurrentForecast fullForecast);
    }

    public class StandardSummaryMapper : IForecastSummaryMapper
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


    public class ConfigurableSummaryMapper : IForecastSummaryMapper
    {
        private readonly ConfigSettingsForecastSummary configSettings;

        public ConfigurableSummaryMapper(IOptions<ConfigSettingsForecastSummary> ForecastSummarySettings)
        {
            this.configSettings = ForecastSummarySettings.Value;
        }

        public CurrentForecastSummary mapSummaryResponse(CurrentForecast fullForecast)
        {
            CurrentForecastSummary forecastSummary = new CurrentForecastSummary
            {
                City = fullForecast.WeatherLocation.LocationName,
                Region = fullForecast.WeatherLocation.Region,
                Country = fullForecast.WeatherLocation.Country,
                LocalTime = fullForecast.WeatherLocation.LocalTime,
                Temperature =  this.configSettings.TemperatureInCelcius ? fullForecast.CurrentConditions.Temperature_Celcius : fullForecast.CurrentConditions.Temperature_Fahrenheit
            };

            return forecastSummary;
        }
    }
}
