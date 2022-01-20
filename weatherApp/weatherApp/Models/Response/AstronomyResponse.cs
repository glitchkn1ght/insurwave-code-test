using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weatherApp.Models.Weather;
using weatherApp.Models.Response;

namespace weatherApp.Models.Response
{
    public class AstronomyResponse : BaseResponse
    {
        public CurrentAstronomySummary astronomySummary { get; set; }
    }
}
