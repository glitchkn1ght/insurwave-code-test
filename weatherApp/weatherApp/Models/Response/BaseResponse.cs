using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weatherApp.Models.Weather;

namespace weatherApp.Models.Response
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }

        public Error Error { get; set; }
    }
}
