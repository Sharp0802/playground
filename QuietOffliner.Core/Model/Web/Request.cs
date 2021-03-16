using System.Net;

namespace QuietOffliner.Core.Model.Web
{
    public readonly struct Request<TRequest>
    {
        private Request(TRequest? value, HttpStatusCode code)
        {
            Value = value;
            ResponseCode = code;
        }
        
        public static Request<TRequest> Failed(HttpStatusCode code)
            => new(default, code);
        public static Request<TRequest> Successful(TRequest result, HttpStatusCode code)
            => new(result, code);
        
        public TRequest?        Value { get; }
        public HttpStatusCode   ResponseCode { get; }
    }
}