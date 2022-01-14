//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// 12/01/2022 Ticket1 JS Team darkSaber - Refactored to map entire errordetails class.  
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Utility
{
    using Newtonsoft.Json;
    using weatherApp.Models.Response;
    
    public interface IErrorMapper
    {
        public ErrorDetails MapErrorDetails(string payload, string resource);
    }
    
    public class StandardErrorMapper : IErrorMapper
    {

        public ErrorDetails MapErrorDetails(string payload, string resource)
        {
            ErrorDetails errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(payload);

            errorDetails.error.HttpStatusCode = this.MapApiErrorCode(errorDetails.error.apiCode);
            
            errorDetails.error.Resource = resource;

            return errorDetails;
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
