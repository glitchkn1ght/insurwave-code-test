//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 14/01/2022 Ticket-3 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Mappers
{
    using weatherApp.Models.Weather;

    public interface IAstronomySummaryMapper
    {
       public CurrentAstronomySummary mapAstronomyAPIResponse(CurrentAstronomy fullAstronomy);
    }

    public class StandardAstronomySummaryMapper : IAstronomySummaryMapper
    {
       public CurrentAstronomySummary mapAstronomyAPIResponse(CurrentAstronomy fullAstronomy) 
        {
            CurrentAstronomySummary currentAstronomySummary = new CurrentAstronomySummary
            {
                SunsetTime = fullAstronomy.Astronomy.Astro.SunsetTime,
                SunriseTime = fullAstronomy.Astronomy.Astro.SunriseTime
            };

            return currentAstronomySummary;
        }
    }
}
