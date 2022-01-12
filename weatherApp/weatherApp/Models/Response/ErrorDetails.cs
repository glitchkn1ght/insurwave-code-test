//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System.Net;

namespace weatherApp.Models.Response
{
    public class ErrorDetails
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public Error()
        {
            switch (this.code)
            {
                case 1002:
                case 2006:
                    HttpStatusCode = HttpStatusCode.Unauthorized;
                    break;
                case 1003:
                case 1005:
                case 1006:
                case 9999:
                    HttpStatusCode = HttpStatusCode.BadRequest;
                    break;
                case 2007:
                case 2008:
                    HttpStatusCode = HttpStatusCode.Forbidden;
                    break;
                default:
                    HttpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }
        }


        public HttpStatusCode HttpStatusCode { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }

}
