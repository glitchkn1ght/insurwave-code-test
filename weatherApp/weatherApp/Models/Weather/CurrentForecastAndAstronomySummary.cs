//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 13/01/2022 Ticket3 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Weather
{
    public class CurrentForecastAndAstronomySummary
    {
        public CurrentForecastSummary CurrentForecastSummary { get; set; }

        public CurrentAstronomySummary CurrentAstronomySummary { get; set; }
    }
}
