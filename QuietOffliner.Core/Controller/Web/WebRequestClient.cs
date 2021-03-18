using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using QuietOffliner.Core.Services;

namespace QuietOffliner.Core.Controller.Web
{
	public sealed class WebRequestClient : IDisposable
	{
		private TextWriter LogStream { get; }
		
		private HttpClientHandler Handler { get; }
		private CookieContainer CookieList { get; }
		private HttpClient Client { get; }

		public WebRequestClient(TextWriter? logStream = null)
		{
			LogStream = logStream ?? Console.Out;

			CookieList = new CookieContainer();
			Handler = new HttpClientHandler
			{
				UseCookies = true,
				CookieContainer = CookieList
			};

			Client = new HttpClient(Handler);
		}

		public Task<HttpResponseMessage> SendAsync(Func<HttpRequestMessage> request, int maximalRetries)
		{
			return Policy
				.Handle<HttpRequestException>()
				.OrResult<HttpResponseMessage>(response => !response.StatusCode.IsSuccesses())
				.WaitAndRetryAsync(
					maximalRetries,
					_ => TimeSpan.FromSeconds(5),
					(exception, _, retryCount, _) =>
					{
						LogStream.WriteLineAsync($"Retry| {retryCount.ToString()}\n" +
						                         $"Cause| {exception}.");
					})
				.ExecuteAsync(() => Client.SendAsync(request()));
		}

		
		public Task<CookieCollection> GetCookies(Uri url)
		{
			return Task.FromResult(CookieList.GetCookies(url));
		}
		
		public Task SetCookies(Uri url, Cookie cookie)
		{
			CookieList.Add(url, cookie);
			
			return Task.CompletedTask;
		}
		
		public Task SetCookies(Uri url, CookieCollection cookies)
		{
			CookieList.Add(url, cookies);
			
			return Task.CompletedTask;
		}
		
		~WebRequestClient()
		{
			Dispose();
		}

		public void Dispose()
		{
			LogStream.Dispose();
			Handler.Dispose();
			Client.Dispose();
			
			GC.SuppressFinalize(this);
		}
	}
}