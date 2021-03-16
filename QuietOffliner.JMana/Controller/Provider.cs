using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using QuietOffliner.Core.Controller;
using QuietOffliner.Core.Model;
using QuietOffliner.Core.Model.Markups;
using QuietOffliner.Core.Model.SearchInfos;
using QuietOffliner.Core.Model.Web;
using QuietOffliner.Core.Services;

namespace QuietOffliner.JMana.Controller
{
	public class Provider : IProvider
	{
		// ReSharper disable once UnusedMember.Local
		private Provider() {}

		private Provider(string siteNum)
		{
			HomePageUri = $"https://www.jmana{siteNum}.net";
		}

		public static Task<Provider> New(int siteNum = 1)
			=> Task.FromResult(new Provider(siteNum.ToString()));

		// ReSharper disable CA1822
		public string Name => "JMana";
		// ReSharper restore CA1822
		public string HomePageUri { get; }

		public async Task<Request<Episode?>> LoadEpisode(string id) //Verified
		{
			var (htmlResult, htmlCode) = await WebRequestService.CreateRequest(
				$"{HomePageUri}/bookdetail?bookdetailid={id}",
				HttpMethod.Get, new[]
				{
					("accept", WebRequestService.HtmlAccept),
					("accept-encoding", WebRequestService.AcceptEncoding),
					("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),

					("cache-control", WebRequestService.NoCache),
					("pragma", WebRequestService.NoCache),

					("referer", HomePageUri),

					("sec-ch-ua", WebRequestService.SecChUa),
					("sec-ch-ua-mobile", "?0"),
					("sec-fetch-dest", "document"),
					("sec-fetch-mode", "navigate"),
					("sec-fetch-site", "same-origin"),
					("sec-fetch-user", "?1"),

					("upgrade-insecure-requests", "1"),

					("user-agent", WebRequestService.UserAgent),
				}).LoadText();

			if (!htmlCode.IsSuccesses())
				return Request<Episode?>.Failed(htmlCode);

			using var docs = await Html.New(htmlResult);

			var title = (await docs.FindBySelector("*.view-top *.tit")).Text();

			var imgList = (await docs.FindAllBySelector("div.pdf-wrap img.comicdetail"))
				.Select(e => e.GetAttribute(e.HasAttribute("src") ? "src" : "data-src"))
				.FilterOutByText(title)
				.Select(async imgSrc =>
				{
					#region Create Image Request

					var imgReq = WebRequestService.CreateRequest(
						imgSrc,
						HttpMethod.Get, new[]
						{
							("accept", WebRequestService.ImageAccept),
							("accept-encoding", WebRequestService.AcceptEncoding),
							("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),
							("cache-control", WebRequestService.NoCache),
							("pragma", WebRequestService.NoCache),
							("referer", HomePageUri),
							("sec-fetch-dest", "image"),
							("sec-fetch-mode", "no-cors"),
							("sec-fetch-site", "same-origin"),
							("upgrade-insecure-requests", "1"),
							("user-agent", WebRequestService.UserAgent)
						});

					#endregion

					var (msg, content) = await imgReq.SafeLoad();
					msg?.Dispose();

					using var stream = new MemoryStream(content);
					using var encoder = new WebP.WebP();

					var webpImg = encoder.EncodeLossless(new Bitmap(stream));

					return webpImg;
				}).Select(t => t.Result);

			var episode = await Episode.New(id, title, imgList, null, this);
			
			return Request<Episode?>.Successful(episode, htmlCode);
		}

		public async Task<Request<ImmutableArray<EpisodeInfo>>> LoadEpisodeInfos(string name, /* ignore */ int page = 0) //Verified
		{
			var htmlReq = WebRequestService.CreateRequest(
				$"{HomePageUri}/comic_list_title?bookname={name.Replace(' ', '+')}",
				HttpMethod.Get, new[]
				{
					("accept", WebRequestService.HtmlAccept),
					("accept-encoding", WebRequestService.AcceptEncoding),
					("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),

					("cache-control", WebRequestService.NoCache),
					("pragma", WebRequestService.NoCache),

					("referer", HomePageUri),

					("sec-ch-ua", WebRequestService.UserAgent),
					("sec-ch-ua-mobile", "?0"),
					("sec-fetch-dest", "document"),
					("sec-fetch-mode", "navigate"),
					("sec-fetch-site", "same-origin"),
					("sec-fetch-user", "?1"),

					("upgrade-insecure-requests", "1"),

					("user-agent", WebRequestService.UserAgent),
				});

			var (htmlResult, htmlCode) = await htmlReq.LoadText();

			if (!htmlCode.IsSuccesses())
				return Request<ImmutableArray<EpisodeInfo>>.Failed(htmlCode);

			using var docs = await Html.New(htmlResult);
			
			var seriesList = (await docs.FindAllBySelector("div.content > div > div.lst-wrap > ul > li"))
				.Select(async e =>
				{
					var titleContainer = e.QuerySelector("a.tit"); // TODO : WTF?
					var title = titleContainer.TextContent;
					var id = titleContainer.Id;

					var strRawDate = e.QuerySelector("p.date").Text();
					var strDateMatch = Regex.Match(strRawDate, "[0-9]+-[0-9]+-[0-9]+");
					if (!(strDateMatch.Success
					      && DateTime.TryParseExact(
						      strDateMatch.Value,
						      "yy-MM-dd", 
						      DateTimeFormatInfo.InvariantInfo,
						      DateTimeStyles.AllowWhiteSpaces,
						      out var date)))
						date = DateTime.MinValue;

					return await EpisodeInfo.New(id, title, date, this);
				}).Select(t => t.Result);

			return Request<ImmutableArray<EpisodeInfo>>.Successful(seriesList.ToImmutableArray(), htmlCode);
		}

		public async Task<Request<ImmutableArray<EpisodeInfo>>> LoadRecentEpisodeInfos(int page = 0)
		{
			var htmlReq = WebRequestService.CreateRequest(
				$"{HomePageUri}/comic_recent&page={page.ToString()}",
				HttpMethod.Get, new[]
				{
					("accept", WebRequestService.HtmlAccept),
					("accept-encoding", WebRequestService.AcceptEncoding),
					("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),

					("cache-control", WebRequestService.NoCache),
					("pragma", WebRequestService.NoCache),

					("referer", HomePageUri),

					("sec-ch-ua", WebRequestService.UserAgent),
					("sec-ch-ua-mobile", "?0"),
					("sec-fetch-dest", "document"),
					("sec-fetch-mode", "navigate"),
					("sec-fetch-site", "cross-site"),
					("sec-fetch-user", "?1"),

					("upgrade-insecure-requests", "1"),

					("user-agent", WebRequestService.UserAgent),
				});

			var (htmlResult, htmlCode) = await htmlReq.LoadText();
			
			if (!htmlCode.IsSuccesses())
				return Request<ImmutableArray<EpisodeInfo>>.Failed(htmlCode);

			using var docs = await Html.New(htmlResult);

			return Request<ImmutableArray<EpisodeInfo>>.Successful(
				(await docs.FindAllBySelector("div.content > div > div.img-lst-wrap > ul > li"))
				// ReSharper disable once StringLiteralTypo
				.Where(e => e.Children.All(c => c.TagName != "ins"))
				.Select(async e =>
				{
					var titleContainer = e.QuerySelector("a.tit");
					var title = titleContainer?.Text() ?? "Unknown";
					var idMatch = Regex.Match(titleContainer?.GetAttribute("href") ?? string.Empty, "(?<=([0-9]+))");
					var id = idMatch.Success ? idMatch.Value : "Unknown";
					var strRawDate = e.QuerySelector(".txt-date").Text();
					var strDateMatch = Regex.Match(strRawDate, "[0-9]+-[0-9]+-[0-9]+");
					if (!(strDateMatch.Success
					    && DateTime.TryParseExact(
						    strDateMatch.Value,
						    "yyyy-MM-dd", 
						    DateTimeFormatInfo.InvariantInfo,
						    DateTimeStyles.AllowWhiteSpaces,
						    out var date)))
						date = DateTime.MinValue;

					return await EpisodeInfo.New(id, title, date, this);
				}).Select(t => t.Result).ToImmutableArray(),
				htmlCode);
		}

		public async Task<Request<SeriesInfo?>> LoadSeriesInfo(string name, int page = 0)
		{
			var htmlReq = WebRequestService.CreateRequest(
				$"{HomePageUri}/comic_list_title?bookname={name.Replace(' ', '+')}",
				HttpMethod.Get, new[]
				{
					("accept", WebRequestService.HtmlAccept),
					("accept-encoding", WebRequestService.AcceptEncoding),
					("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),

					("cache-control", WebRequestService.NoCache),
					("pragma", WebRequestService.NoCache),

					("referer", HomePageUri),

					("sec-ch-ua", WebRequestService.UserAgent),
					("sec-ch-ua-mobile", "?0"),
					("sec-fetch-dest", "document"),
					("sec-fetch-mode", "navigate"),
					("sec-fetch-site", "same-origin"),
					("sec-fetch-user", "?1"),

					("upgrade-insecure-requests", "1"),

					("user-agent", WebRequestService.UserAgent),
				});

			var (htmlResult, htmlCode) = await htmlReq.LoadText();

			if (!htmlCode.IsSuccesses())
				return Request<SeriesInfo?>.Failed(htmlCode);

			using var docs = await Html.New(htmlResult);

			var container = await docs.FindBySelector("div.content > div > div.books-db-detail");
			var rawTitle = container.QuerySelector("a.tit").Text();
			var title = rawTitle.Replace("제목 : ", "");

			var thumbnailSrc = container.QuerySelector("div.books-thumnail > img").GetAttribute("src");
			var (thumbnailMsg, thumbnailBytes) = await WebRequestService.CreateRequest(
				thumbnailSrc.StartsWith("/")
					? $"{HomePageUri}{thumbnailSrc}"
					: thumbnailSrc,
				HttpMethod.Get, 
				new[]
				{
					("accept", WebRequestService.ImageAccept),
					("accept-encoding", WebRequestService.AcceptEncoding),
					("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),
					
					("cache-control", WebRequestService.NoCache),
					("pragma", WebRequestService.NoCache),
					
					("referer", HomePageUri),
					
					("sec-ch-ua", WebRequestService.SecChUa),
					("sec-ch-ua-mobile", "?0"),
					("sec-fetch-dest", "image"),
					("sec-fetch-mode", "no-cors"),
					("sec-fetch-site", "same-origin"),
					
					("user-agent", WebRequestService.UserAgent),
				}).SafeLoad();

			var thumbnail = Array.Empty<byte>();

			if (thumbnailMsg.IsSuccessStatusCode)
			{
				using var webp = new WebP.WebP();
				using var bytesStream = new MemoryStream(thumbnailBytes);
				thumbnail = webp.EncodeLossless(new Bitmap(bytesStream));
			}

			var artist = "Unknown";
			var tagList = new List<string>();
			var infoContainer = container.QuerySelector("div.books-d-wrap");
			foreach (var info in infoContainer.Children.Where(e => e.TagName == "dl"))
			{
				var infoText = info.Text();
				if (infoText.StartsWith("작가"))
					artist = infoText.Replace("작가 : ", "");
				else if (infoText.StartsWith("태그"))
					tagList.Add(infoText.Replace("장르 : ", ""));
			}

			return Request<SeriesInfo?>.Successful(
				await SeriesInfo.New(
					title,
					artist,
					PublishInterval.Unknown,
					tagList,
					thumbnail,
					this),
				htmlCode);
		}

		public async Task<Request<ImmutableArray<SeriesInfo>>> LoadRecentSeriesInfos(int page = 0) //Verified
		{
			return await LoadAllSeriesInfos(await SearchRequest.New(
				string.Empty,
				string.Empty,
				PublishInterval.All,
				Ordering.Recent,
				Array.Empty<string>()),
				page);
		}

		public async Task<Request<ImmutableArray<SeriesInfo>>> LoadAllSeriesInfos(SearchRequest request, int page = 0) //Verified
		{
			#region Enum to String

			var interval = request.Interval switch
			{
				PublishInterval.All => "",
				PublishInterval.Weekly => "주간",
				PublishInterval.EveryOtherWeek => "격주",
				PublishInterval.Monthly => "월간",
				PublishInterval.Book => "단행본",
				PublishInterval.Short => "단편",
				PublishInterval.Complete => "완결",
				PublishInterval.Misc => "기타",

				PublishInterval.Unknown => throw new InvalidEnumArgumentException(
					nameof(request.Interval),
					(int) request.Interval,
					typeof(PublishInterval)),
				_ => throw new ArgumentOutOfRangeException(
					nameof(request.Interval),
					request.Interval,
					"out of range")
			};
			var ordering = request.Ordering switch
			{
				Ordering.Default => "전체",
				Ordering.Recent => "최신순",
				Ordering.Goods => "추천",
				Ordering.Bookmarks => "북마크",

				Ordering.Popular => throw new ProviderNotSupportedException(
					this,
					"Ordering by popular is not supported"),
				_ => throw new ArgumentOutOfRangeException(
					nameof(request.Ordering),
					request.Ordering,
					"out of range")
			};

			#endregion

			#region Create Html Request

			var keywordQ = string.IsNullOrEmpty(request.Name)
				? string.Empty
				: $"keyword={request.Name.Replace(' ', '+')}&";
			var artistQ = string.IsNullOrEmpty(request.Artist)
				? string.Empty
				: $"author={request.Artist.Replace(' ', '+')}&";
			var intervalQ = string.IsNullOrEmpty(interval)
				? string.Empty
				// ReSharper disable once StringLiteralTypo
				: $"gubun={interval}&";
			var tagsQ = !request.Tags.Any()
				? string.Empty
				: $"tag={request.Tags.Aggregate((c, n) => $"{c}::{n}")}&";
			var orderingQ = string.IsNullOrEmpty(ordering)
				? string.Empty
				: $"ordering={ordering}";

			var htmlReq = WebRequestService.CreateRequest(
				$"{HomePageUri}/comic_list?page={page.ToString()}&" + keywordQ + artistQ + intervalQ + tagsQ + orderingQ,
				HttpMethod.Get, new[]
				{
					("accept", WebRequestService.HtmlAccept),
					("accept-encoding", WebRequestService.AcceptEncoding),
					("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),
					("cache-control", WebRequestService.NoCache),
					("pragma", WebRequestService.NoCache),
					("referer", HomePageUri),
					("sec-ch-ua", WebRequestService.SecChUa),
					("sec-ch-ua-mobile", "?0"),
					("sec-fetch-dest", "document"),
					("sec-fetch-mode", "navigate"),
					("sec-fetch-site", "same-origin"),
					("sec-fetch-user", "?1"),
					("upgrade-insecure-requests", "1"),
					("user-agent", WebRequestService.UserAgent)
				});

			#endregion

			var (htmlResult, htmlCode) = await htmlReq.LoadText();

			if (!htmlCode.IsSuccesses())
				return Request<ImmutableArray<SeriesInfo>>.Failed(htmlCode);

			using var docs = await Html.New(htmlResult);

			var seriesList = (await docs.FindAllBySelector("*.search-result-wrap > *.img-lst-wrap > ul > li"))
				.Select(async e =>
				{
					var title = e.QuerySelector("a.tit").Text();
					var artist = e.QuerySelector("p.p_author").Text();
					var publishInterval = e.ClassName switch
					{
						"weekly1" => PublishInterval.Weekly,
						"weekly2" => PublishInterval.EveryOtherWeek,
						"monthly" => PublishInterval.Monthly,
						"one" => PublishInterval.Book,
						"dan" => PublishInterval.Short,
						"finish" => PublishInterval.Complete,
						"etc" => PublishInterval.Misc,

						_ => PublishInterval.Unknown
					};
					var tags = e.QuerySelector(".genre").Text().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					var thumbnailSrc = e.QuerySelector("img.main_img").GetAttribute("src");

					if (thumbnailSrc.StartsWith("/"))
						thumbnailSrc = thumbnailSrc.Insert(0, HomePageUri);

					#region Create Image Request

					var imgReq = WebRequestService.CreateRequest(
						thumbnailSrc,
						HttpMethod.Get,
						new[]
						{
							("accept", WebRequestService.ImageAccept),
							("accept-encoding", WebRequestService.AcceptEncoding),
							("accept-language", "en-GB,en-US;q=0.9,en;q=0.8,ko;q=0.7"),
							("cache-control", WebRequestService.NoCache),
							("pragma", WebRequestService.NoCache),
							("referer", HomePageUri),
							("sec-fetch-dest", "image"),
							("sec-fetch-mode", "no-cors"),
							("sec-fetch-site", "same-origin"),
							("upgrade-insecure-requests", "1"),
							("user-agent", WebRequestService.UserAgent)
						});

					#endregion
					
					var (msg, content) = await imgReq.SafeLoad();
					msg?.Dispose();

					return await SeriesInfo.New(title, artist, publishInterval, tags, content, this);
				}).Select(t => t.Result);

			return Request<ImmutableArray<SeriesInfo>>.Successful(seriesList.ToImmutableArray(), htmlCode);
		}
	}
}