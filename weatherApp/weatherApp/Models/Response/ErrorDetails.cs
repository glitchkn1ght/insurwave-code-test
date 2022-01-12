//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Response
{

    using Newtonsoft.Json;

    public class ErrorDetails
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public void mapErrorCodes()
        {
            switch (this.apiCode)
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
        }

        public int HttpStatusCode { get; set; }

        [JsonProperty("code")]
        public int apiCode { get; set; }

        [JsonProperty("message")]
        public string apiMessage { get; set; }
    }

}
