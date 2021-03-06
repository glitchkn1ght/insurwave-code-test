//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Configuration
{
    public class ConfigSettingsWeatherAPI
    {
        public string APIKey { get; set; }

        public string BaseURL { get; set; }
        
        public string CurrentResourceURL { get; set; }

        public string ContentType { get; set; }

        public string GetAirQualityData { get; set; }
    }
}
