//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// 12/01/2022 Ticket1 JS Team darkSaber - Refactored to map entire errordetails class.  
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using weatherApp.Models.Response;
    using System.Net.Http;
    
    public interface IErrorMapper
    {
        public Task<ErrorResponse> MapError(HttpResponseMessage payload, string resource);
    }
    
    public class StandardErrorMapper : IErrorMapper
    {

        public async Task<ErrorResponse> MapError(HttpResponseMessage payload, string resource)
        {
            string y = await payload.Content.ReadAsStringAsync();
            
            ErrorResponse error = JsonConvert.DeserializeObject<ErrorResponse>(await payload.Content.ReadAsStringAsync());

            error.Error.HttpStatusCode = this.MapApiErrorCode(error.Error.ApiCode);
            
            error.Error.Resource = resource;

            return error;
        }

        private int MapApiErrorCode(int apiErrorCode)
        {
            int HttpStatusCode;

                switch (apiErrorCode)
                {
                    case 1002:
                    case 2006:
                        HttpStatusCode = 401;
                        break;
                    case 1003:
                    case 1005:
                    case 1006:
                    case 9999:
                        HttpStatusCode = 400;
                        break;
                    case 2007:
                    case 2008:
                        HttpStatusCode = 403;
                        break;
                    default:
                        HttpStatusCode = 500;
                        break;

                }
            return HttpStatusCode;
        }
    }
}
