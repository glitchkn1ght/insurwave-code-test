//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 14/01/2022 Ticket-3 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using Newtonsoft.Json;
    using System;
    using weatherApp.Models.Weather;

    public interface IAstronomyMapper
    {
        CurrentAstronomySummary mapAstronomyAPIResponse(string payload);
    }

    public class StandardAstronomyMapper : IAstronomyMapper
    {
        private IAstronomySummaryMapper AstronomySummaryMapper;

        public StandardAstronomyMapper(IAstronomySummaryMapper astronomySummaryMapper)
        {
            this.AstronomySummaryMapper = astronomySummaryMapper ?? throw new ArgumentNullException(nameof(astronomySummaryMapper));
        }

        public CurrentAstronomySummary mapAstronomyAPIResponse(string payload)
        {
            CurrentAstronomy fullAstronomy = JsonConvert.DeserializeObject<CurrentAstronomy>(payload);

            CurrentAstronomySummary astronomySummary = this.AstronomySummaryMapper.mapAstronomyAPIResponse(fullAstronomy);

            return astronomySummary;
        }

    }
}
