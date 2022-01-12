using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace weatherApp.Models
{
    public class ErrorDetails
    {
        public Error error { get; set; }
}

    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
    }

}
