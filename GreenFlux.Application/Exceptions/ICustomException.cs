using System.Net;

namespace GreenFlux.Application.Exceptions
{
    public interface ICustomException
    {
        public HttpStatusCode? HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string ToJson();
    }
}