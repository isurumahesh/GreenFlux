using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GreenFlux.Application.Exceptions
{
    public class CustomException : Exception, ICustomException
    {
        public HttpStatusCode? HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
       
        public string ToJson()
        {
            return JsonConvert.SerializeObject(new
            {
                 StatusCode = HttpStatusCode,
                 ErrorCode = HttpStatusCode,
                 Message = Message
            });
        }
    }
}
