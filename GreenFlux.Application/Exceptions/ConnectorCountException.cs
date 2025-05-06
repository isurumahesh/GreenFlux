using Newtonsoft.Json;
using System.Net;

namespace GreenFlux.Application.Exceptions
{
    public class ConnectorCountException : Exception, ICustomException
    {
        public HttpStatusCode? HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new
            {
                StatusCode = HttpStatusCode,
                ErrorCode = HttpStatusCode,
                ErrorMessage = ErrorMessage
            });
        }
    }
}