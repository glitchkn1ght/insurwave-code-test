using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weatherApp.Models.Weather;
using weatherApp.Models.Response;

namespace weatherApp.Models.Response
{
    public class ForecastResponse : BaseResponse
    {
        public CurrentForecastSummary forecastSummary { get; set; }
    }
}
