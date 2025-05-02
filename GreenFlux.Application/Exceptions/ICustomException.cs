using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GreenFlux.Application.Exceptions
{
    public interface ICustomException
    {     
        public HttpStatusCode? HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public string ToJson();
    }
}
