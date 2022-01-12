using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApp.Models.Weather
{
    public class SummaryForecast
    {
        public string LocationName { get; set; }

        public DateTime LastUpdated { get; set; }

        public double TempCelcius { get; set; }

        public double Precipitation_mm { get; set; }
    }
}
