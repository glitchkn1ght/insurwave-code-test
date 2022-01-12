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
        public double Wind_Mph { get; set; }

        [JsonProperty("wind_kph")]
        public double Wind_Kph { get; set; }

        [JsonProperty("wind_degree")]
        public int Wind_Degree { get; set; }

        [JsonProperty("wind_dir")]
        public string Wind_Dir { get; set; }

        [JsonProperty("pressure_mb")]
        public double Pressure_mb { get; set; }

        [JsonProperty("pressure_in")]
        public double Pressure_in { get; set; }

        [JsonProperty("precip_mm")]
        public double Precipitation_mm { get; set; }

        [JsonProperty("precip_in")]
        public double Precipitation_in { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("cloud")]
        public int Cloud { get; set; }

        [JsonProperty("feelslike_c")]
        public double Feelslike_Celcius { get; set; }

        [JsonProperty("Feelslike_f")]
        public double Feelslike_Fahrenheit { get; set; }

        [JsonProperty("vis_km")]
        public double Visibility_Km { get; set; }

        [JsonProperty("vis_miles")]
        public double Visibility_Miles { get; set; }

        [JsonProperty("uv")]
        public double UV { get; set; }

        [JsonProperty("gust_mph")]
        public double Gust_Mph { get; set; }

        [JsonProperty("gust_kph")]
        public double Gust_Kph { get; set; }

    }
}
