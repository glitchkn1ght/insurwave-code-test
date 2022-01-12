using System;
using Newtonsoft.Json;

namespace weatherApp.Models.Weather
{
    public class CurrentForecast
    {
        [JsonProperty("location")]
        public Location WeatherLocation { get; set; }

        [JsonProperty("current")]
        public Current CurrentConditions { get; set; }
    }
}
