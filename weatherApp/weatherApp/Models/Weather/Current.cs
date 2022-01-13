//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Weather
{
    using Newtonsoft.Json;
    using System;

    public class Current
    {
        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("last_updated_epoch")]
        public int Last_Updated_Epoch { get; set; }

        [JsonProperty("last_updated")]
        public DateTime Last_Updated { get; set; }

        [JsonProperty("temp_c")]
        public decimal Temperature_Celcius { get; set; }

        [JsonProperty("temp_f")]
        public decimal Temperature_Fahrenheit { get; set; }

        [JsonProperty("is_day")]
        public int Is_day { get; set; }

        [JsonProperty("wind_mph")]
        public decimal Wind_Mph { get; set; }

        [JsonProperty("wind_kph")]
        public decimal Wind_Kph { get; set; }

        [JsonProperty("wind_degree")]
        public int Wind_Degree { get; set; }

        [JsonProperty("wind_dir")]
        public string Wind_Dir { get; set; }

        [JsonProperty("pressure_mb")]
        public decimal Pressure_mb { get; set; }

        [JsonProperty("pressure_in")]
        public decimal Pressure_in { get; set; }

        [JsonProperty("precip_mm")]
        public decimal Precipitation_mm { get; set; }

        [JsonProperty("precip_in")]
        public decimal Precipitation_in { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("cloud")]
        public int Cloud { get; set; }

        [JsonProperty("feelslike_c")]
        public decimal Feelslike_Celcius { get; set; }

        [JsonProperty("Feelslike_f")]
        public decimal Feelslike_Fahrenheit { get; set; }

        [JsonProperty("vis_km")]
        public decimal Visibility_Km { get; set; }

        [JsonProperty("vis_miles")]
        public decimal Visibility_Miles { get; set; }

        [JsonProperty("uv")]
        public decimal UV { get; set; }

        [JsonProperty("gust_mph")]
        public decimal Gust_Mph { get; set; }

        [JsonProperty("gust_kph")]
        public decimal Gust_Kph { get; set; }

    }
}
