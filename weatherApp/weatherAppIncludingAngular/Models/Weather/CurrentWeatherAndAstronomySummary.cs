//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 13/01/2022 Ticket3 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Weather
{
    public class CurrentWeatherAndAstronomySummary
    {
        public CurrentWeatherAndAstronomySummary()
        {
            this.CurrentForecastSummary = new CurrentForecastSummary();
            this.CurrentAstronomySummary = new CurrentAstronomySummary();
        }
        
        public CurrentForecastSummary CurrentForecastSummary { get; set; }

        public CurrentAstronomySummary CurrentAstronomySummary { get; set; }
    }
}
