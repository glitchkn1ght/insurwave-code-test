//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 14/01/2022 Ticket-3 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using Newtonsoft.Json;
    using System;
    using weatherApp.Models.Weather;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IAstronomyMapper
    {
        Task<CurrentAstronomySummary> mapAstronomyAPIResponse(HttpResponseMessage payload);
    }

    public class StandardAstronomyMapper : IAstronomyMapper
    {
        private IAstronomySummaryMapper AstronomySummaryMapper;

        public StandardAstronomyMapper(IAstronomySummaryMapper astronomySummaryMapper)
        {
            this.AstronomySummaryMapper = astronomySummaryMapper ?? throw new ArgumentNullException(nameof(astronomySummaryMapper));
        }

        public async Task<CurrentAstronomySummary> mapAstronomyAPIResponse(HttpResponseMessage payload)
        {
            CurrentAstronomy fullAstronomy = JsonConvert.DeserializeObject<CurrentAstronomy>(await payload.Content.ReadAsStringAsync());

            CurrentAstronomySummary astronomySummary = this.AstronomySummaryMapper.mapAstronomyAPIResponse(fullAstronomy);

            return astronomySummary;
        }

    }
}
