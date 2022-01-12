using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using weatherApp.Models.Weather;
using Newtonsoft.Json;

namespace weatherApp.Utility
{
   public interface IForecastMapper
   {
        CurrentForecastSummary mapWeatherAPIResponse(string payload);
    }


    public class standardForecastMapper : IForecastMapper
    {
       private IForecastSummaryMapper SummaryMapper { get; set; }
        
        public standardForecastMapper(IForecastSummaryMapper summaryMapper)
        {
            this.SummaryMapper = summaryMapper ?? throw new ArgumentNullException(nameof(summaryMapper));
        }

        public CurrentForecastSummary mapWeatherAPIResponse(string payload)
        {
            CurrentForecast fullForecast = JsonConvert.DeserializeObject<CurrentForecast>(payload);

            CurrentForecastSummary forecastSummary = this.SummaryMapper.mapSummaryResponse(fullForecast);

            return forecastSummary;
        }
    }
}
