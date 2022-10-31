//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// 13/01/2022 Ticket3 JS Team darkSaber - Added AstronomyResourceURL
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Configuration
{
    public class ConfigSettingsWeatherAPI
    {
        public string APIKey { get; set; }

        public string BaseURL { get; set; }
        
        public string CurrentResourceURL { get; set; }

        public string AstronomyResourceURL { get; set; }

        public string ContentType { get; set; }

        public string GetAirQualityData { get; set; }
    }
}
