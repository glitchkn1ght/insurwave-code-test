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
        public int HttpStatusCode { get; set; }

        [JsonProperty("code")]
        public int apiCode { get; set; }

        [JsonProperty("message")]
        public string apiMessage { get; set; }
    }

}
