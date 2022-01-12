using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace weatherApp.Models.Configuration
{
    public class configSettingsWeatherAPI
    {
        public string APIKey { get; set; }

        public string BaseURL { get; set; }
        
        public string ResourceURL { get; set; }

        public string ContentType { get; set; }

        public string GetAirQualityData { get; set; }
    }
}
