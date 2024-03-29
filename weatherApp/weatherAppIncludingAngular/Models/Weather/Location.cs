﻿//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Weather
{
    using Newtonsoft.Json;
    
    public class Location
    {
        [JsonProperty("name")]
        public string LocationName { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("tz_id")]
        public string TimeZoneId { get; set; }

        [JsonProperty("localtime_epoch")]
        public int LocalTime_Epoch { get; set; }

        [JsonProperty("localtime")]
        public string LocalTime { get; set; }
    }
}
