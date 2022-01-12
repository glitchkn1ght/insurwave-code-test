using Newtonsoft.Json;

namespace weatherApp.Models.Weather
{
    public class Condition
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
