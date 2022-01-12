using System;
using Newtonsoft.Json;

namespace AngularApp.Models.Weather
{
    public class WeatherForecast
    {
        [JsonProperty("location")]
        public Location WeatherLocation { get; set; }

        [JsonProperty("current")]
        public Current CurrentConditions { get; set; }
    }
}
