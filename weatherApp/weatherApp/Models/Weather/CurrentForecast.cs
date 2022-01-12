using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApp.Models.Weather
{
    public class CurrentForecast
    {
        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public DateTime LocalTime { get; set; }

        public double Temperature { get; set; }
    }
}
