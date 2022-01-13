//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Astronomy
{
    using Newtonsoft.Json;

    public class Astronomy
    {
        [JsonProperty("sunrise")]
        public string SunriseTime { get; set; }

        [JsonProperty("sunrise")]
        public string SunsetTime { get; set; }

        [JsonProperty("moonrise")]
        public string MoonRiseTime { get; set; }

        [JsonProperty("moonset")]
        public string MoonSetTime { get; set; }

        [JsonProperty("moon_phase")]
        public string MoonPhase { get; set; }

        [JsonProperty("moon_illumination")]
        public string MoonIllumination { get; set; }
    }
}
