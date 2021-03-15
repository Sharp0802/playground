using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace QuietOffliner.Core.Services
{
	public static class WebRequestService
	{
		public const string ImageAccept = "image/*";
		public const string HtmlAccept = "text/html";
		public const string XmlAccept = "application/xml";
		public const string NoCache = "no-cache";
		public const string AcceptEncoding = "gzip, deflate, br";
		public const string SecChUa = "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"91\", \"Google Chrome\";v=\"91\"";

		public const string UserAgent =
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4442.4 Safari/537.36";

		private static HttpClientHandler Handler { get; }
		private static HttpClient Client { get; }

		static WebRequestService()
		{
			Handler = new HttpClientHandler
			{
			};
			Client = new HttpClient(Handler);
		}

		public static HttpRequestMessage CreateRequest(
			string url,
			HttpMethod method,
			IEnumerable<(string key, string value)> headers)
		{
			var msg = new HttpRequestMessage(method, url);

			foreach (var (name, value) in headers)
			{
				msg.Headers.Add(name, value);
			}

			return msg;
		}

		private static async Task<(HttpResponseMessage msg, byte[] content)> Load(this HttpRequestMessage req)
		{
			var res = await Client.SendAsync(req);

			var con = await (await res.Content
					.ReadAsByteArrayAsync())
				.WebDecompress(res.Content.Headers.ContentEncoding);

			return (res, con);
		}

		public static Task<(HttpResponseMessage msg, byte[] content)> SafeLoad(
			this HttpRequestMessage req,
			uint maxRequestCount = 5,
			Action<Exception>? unexpectedExceptionHandle = null,
			Action<Exception>? onFailedRequestHandle = null)
		{
			unexpectedExceptionHandle ??= e => throw e;
			onFailedRequestHandle ??= e => throw e;

			Exception failedException = null!;
			for (var i = 0U; i < maxRequestCount; i++)
			{
				try
				{
					var (msg, content) = req.Load().Result;

					return Task.FromResult((msg, content));
				}
				catch (Exception e) when (e is AggregateException or HttpRequestException or WebException)
				{
					failedException = e;
				}
				catch (Exception e)
				{
					unexpectedExceptionHandle(e);
					failedException = e;
				}
			}

			onFailedRequestHandle(failedException);

			throw failedException;
		}

		public static async Task<(string result, HttpStatusCode code)> LoadText(this HttpRequestMessage req,
			uint maxRequestCount = 5) //TODO
		{
			var (msg, con) = await req.SafeLoad(maxRequestCount);

			Encoding encode = Encoding.UTF8;
			if (msg.Content.Headers.ContentType?.CharSet is null)
				return (encode.GetString(con), msg.StatusCode);

			try
			{
				encode = Encoding.GetEncoding(msg.Content.Headers.ContentType.CharSet);
			}
			catch (ArgumentException)
			{
				encode = Encoding.UTF8;
			}

			return ((string) encode.GetString(con).Clone(), msg.StatusCode);
		}
	}
}