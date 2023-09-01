using System.Net;

namespace QuietOffliner.Core.Model.Web
{
    public readonly struct Request<TRequest>
    {
        private Request(TRequest? value, HttpStatusCode code, bool isSucceed)
        {
            Value = value;
            ResponseCode = code;
            IsSucceed = isSucceed;
        }
        
        public static Request<TRequest> Failed(HttpStatusCode code)
            => new(default, code, false);
        public static Request<TRequest> Successful(TRequest result, HttpStatusCode code)
            => new(result, code, true);
        
        public TRequest?        Value { get; }
        public HttpStatusCode   ResponseCode { get; }
        public bool             IsSucceed { get; }
    }
}