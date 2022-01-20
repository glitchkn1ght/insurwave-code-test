//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.Models.Response
{
    using Newtonsoft.Json;

    public class Error
    {
        public ErrorDetails ErrorDetails { get; set; }
    }

    public class ErrorDetails
    {
        public int HttpStatusCode { get; set; }

        public string Resource { get; set; }

        [JsonProperty("code")]
        public int ApiCode { get; set; }

        [JsonProperty("message")]
        public string ApiMessage { get; set; }
    }

}
