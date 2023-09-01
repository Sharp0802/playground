using System;

namespace QuietOffliner.Core.Resources
{
	public static class HttpHeadersExtension
	{
		public static (string, string)[] GetHeader(this AcceptType type, string referer)
			=> type switch {
					AcceptType.Image => new[]
					{
						("accept",						HttpHeaders.AcceptImage),
						("accept-encoding",				HttpHeaders.AcceptEncoding),
						("accept-language",				HttpHeaders.AcceptLanguage),

						("referer",						referer),

						("sec-fetch-dest",				HttpHeaders.AcceptImage),
						("sec-fetch-mode",				HttpHeaders.SecFetchModeNoCors),
						("sec-fetch-site",				HttpHeaders.SecFetchSiteCross),

						("upgrade-insecure-requests",	HttpHeaders.UpgradeInsecureRequests),
						
						("user-agent",					HttpHeaders.UserAgent)
					},
					AcceptType.Docs => new[]
					{
						("accept",						HttpHeaders.AcceptDocs),
						("accept-encoding",				HttpHeaders.AcceptEncoding),
						("accept-language",				HttpHeaders.AcceptLanguage),
					
						("referer",						referer),
					
						("sec-ch-ua",					HttpHeaders.SecChUa),
						("sec-ch-ua-mobile",			HttpHeaders.SecChUaMobile),
						("sec-fetch-dest",				HttpHeaders.AcceptImage),
						("sec-fetch-mode",				HttpHeaders.SecFetchModeNoCors),
						("sec-fetch-site",				HttpHeaders.SecFetchSiteCross),
						("sec-fetch-user",				HttpHeaders.SecFetchUser),
					
						("upgrade-insecure-requests",	HttpHeaders.UpgradeInsecureRequests),
					
						("user-agent",					HttpHeaders.UserAgent)
					},
					_ => throw new ArgumentOutOfRangeException(nameof(type))
				};
	}

	public enum AcceptType
	{
		Image,
		Docs
	}
}