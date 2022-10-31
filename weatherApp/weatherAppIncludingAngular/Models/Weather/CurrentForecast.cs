//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Weather
{
    using Newtonsoft.Json;
    public class CurrentForecast
    {
        [JsonProperty("Location")]
        public Location WeatherLocation { get; set; }

        [JsonProperty("current")]
        public Current CurrentConditions { get; set; }
    }
}
