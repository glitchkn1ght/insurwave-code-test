//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Response
{
    using Newtonsoft.Json;

    public class ErrorDetails
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public int HttpStatusCode { get; set; }

        public string Resource { get; set; }

        [JsonProperty("code")]
        public int ApiCode { get; set; }

        [JsonProperty("message")]
        public string ApiMessage { get; set; }
    }

}
