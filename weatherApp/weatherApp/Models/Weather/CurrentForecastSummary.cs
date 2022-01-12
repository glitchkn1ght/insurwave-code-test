//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Weather
{
    using System;

    public class CurrentForecastSummary
    {
        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public DateTime LocalTime { get; set; }

        public decimal Temperature { get; set; }
    }
}
