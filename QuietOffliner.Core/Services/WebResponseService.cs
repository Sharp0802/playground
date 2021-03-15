using System.Net;

namespace QuietOffliner.Core.Services
{
    public static class WebResponseService
    {
        public static bool IsSuccesses(this HttpStatusCode res)
            => res is HttpStatusCode.OK or HttpStatusCode.Redirect;
    }
}