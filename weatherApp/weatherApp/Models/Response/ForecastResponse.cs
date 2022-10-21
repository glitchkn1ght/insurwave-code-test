//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/01/2022 Ticket3 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Response
{
    using weatherApp.Models.Weather;

    public class ForecastResponse : BaseResponse
    {
        public CurrentForecastSummary forecastSummary { get; set; }
    }
}
