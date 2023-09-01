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
using QuietOffliner.Core.Resources;
using QuietOffliner.Core.Controller;
using QuietOffliner.Core.Extensions;
using QuietOffliner.Core.Model;
using QuietOffliner.Core.Model.Markups;
using QuietOffliner.Core.Model.SearchInfos;
using QuietOffliner.Core.Model.Web;
using QuietOffliner.Core.Services;

namespace QuietOffliner.JMana.Controller
{
	public class JManaProvider : Provider
	{
		public JManaProvider(int siteNum, TextWriter logStream = null) : base(
			"JMana",
			$"https://www.jmana{siteNum.ToString()}.net",
			new ProviderSupportList(
				false,
				false,
				true,
				true,
				true,
				true),
			logStream)
		{
		}

		private IEnumerable<(string name, string value)> ImageRequestHeader => AcceptType.Image.GetHeader(HomePageUri);
		private IEnumerable<(string name, string value)> DocsRequestHeader => AcceptType.Docs.GetHeader(HomePageUri);

		public override async Task<Request<Episode?>> LoadEpisode(string id) //Verified
		{
			using var htmlMsg = await Client.SendAsync(() => ImageRequestHeader.CreateMsg(
					$"{HomePageUri}/bookdetail?bookdetailid={id}",
					HttpMethod.Get));

			var htmlResult = await htmlMsg.DecompressContent()
				.ContinueWith(t => t.Result.ToString(htmlMsg.Content.Headers.ContentType.CharSet.GetEncoding()));

			if (!htmlMsg.IsSuccessStatusCode)
				return Request<Episode?>.Failed(htmlMsg.StatusCode);

			using var docs = await Html.New(htmlResult);
			var title = (await docs.FindBySelector("*.view-top *.tit")).Text();

			var imgList = (await docs.FindAllBySelector("div.pdf-wrap img.comicdetail"))
				.Select(e => e.GetAttribute(e.HasAttribute("src") ? "src" : "data-src"))
				.FilterOutByText(title)
				.Select(async imgSrc =>
				{
					using var imgReq = await Client.SendAsync(() => ImageRequestHeader.CreateMsg(imgSrc, HttpMethod.Get));
					
					var imgResult = await imgReq.DecompressContent();

					using var stream = new MemoryStream(imgResult);
					using var encoder = new WebP.WebP();

					var webpImg = encoder.EncodeLossless(new Bitmap(stream));

					return webpImg;
				}).Select(t => t.Result);

			var episode = await Episode.New(id, title, imgList, null, this);
			
			return Request<Episode?>.Successful(episode, htmlMsg.StatusCode);
		}

		public override async Task<Request<ImmutableArray<EpisodeInfo>>> LoadEpisodeInfos(string name, int page = 0) //Verified
		{
			using var htmlMsg = await Client.SendAsync(() => DocsRequestHeader.CreateMsg(
				$"{HomePageUri}/comic_list_title?bookname={name.Replace(' ', '+')}",
				HttpMethod.Get));

			var htmlResult = await htmlMsg.DecompressContent()
				.ContinueWith(t => t.Result.ToString(htmlMsg.Content.Headers.ContentType.CharSet.GetEncoding()));
			
			if (!htmlMsg.IsSuccessStatusCode)
				return Request<ImmutableArray<EpisodeInfo>>.Failed(htmlMsg.StatusCode);
			using var docs = await Html.New(htmlResult);

			var seriesList = (await docs.FindAllBySelector("div.content > div > div.lst-wrap > ul > li"))
				.Select(async e =>
				{
					var titleContainer = e.QuerySelector("a.tit");
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
			
			return Request<ImmutableArray<EpisodeInfo>>.Successful(seriesList.ToImmutableArray(), htmlMsg.StatusCode);
		}

		public override async Task<Request<ImmutableArray<EpisodeInfo>>> LoadRecentEpisodeInfos(int page = 0) //Verified
		{
			using var htmlMsg = await Client.SendAsync(() => DocsRequestHeader.CreateMsg(
				$"{HomePageUri}/comic_recent?page={(page + 1).ToString()}",
				HttpMethod.Get));

			var htmlResult = await htmlMsg.DecompressContent()
				.ContinueWith(t => t.Result.ToString(htmlMsg.Content.Headers.ContentType.CharSet.GetEncoding()));
			
			if (!htmlMsg.IsSuccessStatusCode)
				return Request<ImmutableArray<EpisodeInfo>>.Failed(htmlMsg.StatusCode);
			
			using var docs = await Html.New(htmlResult);
			
			return Request<ImmutableArray<EpisodeInfo>>.Successful(
				(await docs.FindAllBySelector("div.content > div > div.img-lst-wrap > ul > li"))
				// ReSharper disable once StringLiteralTypo
				.Where(e => e.Children.Length > 1)
				.Select(async e =>
				{
					var titleContainer = e.QuerySelector("a.tit");
					var title = titleContainer?.Text() ?? "Unknown";

					var idMatch = Regex.Match(titleContainer?.GetAttribute("href") ?? string.Empty, "(?<==)[0-9]+");
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
				htmlMsg.StatusCode);
		}

		public override async Task<Request<SeriesInfo?>> LoadSeriesInfo(string name) //Verified
		{
			using var htmlReq = await Client.SendAsync(() => DocsRequestHeader.CreateMsg(
				$"{HomePageUri}/comic_list_title?bookname={name.Replace(' ', '+')}",
				HttpMethod.Get));

			var htmlResult = await htmlReq
				.DecompressContent()
				.ContinueWith(t => t.Result.ToString(htmlReq.Content.Headers.ContentType.CharSet.GetEncoding()));
			
			if (!htmlReq.IsSuccessStatusCode)
				return Request<SeriesInfo?>.Failed(htmlReq.StatusCode);
			
			using var docs = await Html.New(htmlResult);
			
			var container = await docs.FindBySelector("div.content > div > div.books-db-detail");
			var rawTitle = container.QuerySelector("a.tit").Text();
			var title = rawTitle.Replace("제목 : ", "");

			// ReSharper disable once StringLiteralTypo
			var thumbnailSrc = container.QuerySelector("div.books-thumnail > img").GetAttribute("src");
			
			using var thumbnailMsg = await Client.SendAsync(() => ImageRequestHeader.CreateMsg(
				thumbnailSrc.StartsWith("/")
					? $"{HomePageUri}{thumbnailSrc}"
					: thumbnailSrc,
				HttpMethod.Get));

			var thumbnailBytes = await thumbnailMsg.DecompressContent();

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
			foreach (var info in infoContainer?.Children
				.Where(e => e.TagName is "dl" or "DL")
				.Select(e => e.FirstElementChild) ?? Array.Empty<IElement>())
			{
				var infoText = info.Text();
				if (infoText.Contains("작가"))
					artist = infoText.Replace("작가 : ", "").Replace(" ", "");
				else if (infoText.Contains("장르"))
					tagList.Add(infoText.Replace("장르 : ", "").Replace(" ", ""));
			}

			return Request<SeriesInfo?>.Successful(
				await SeriesInfo.New(
					title,
					artist,
					PublishInterval.Unknown,
					tagList,
					thumbnail,
					this),
				htmlReq.StatusCode);
		}

		public override async Task<Request<ImmutableArray<SeriesInfo>>> LoadRecentSeriesInfos(int page = 0) //Verified
		{
			return await LoadAllSeriesInfos(await SearchRequest.New(
					string.Empty,
					string.Empty,
					PublishInterval.All,
					Ordering.Recent,
					Array.Empty<string>()),
				page);
		}

		public override async Task<Request<ImmutableArray<SeriesInfo>>> LoadAllSeriesInfos(SearchRequest request, int page = 0) //Verified
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

				PublishInterval.Unknown => throw new ArgumentOutOfRangeException(nameof(request.Interval)),
				_ => throw new ArgumentOutOfRangeException(nameof(request.Interval))
			};
			var ordering = request.Ordering switch
			{
				Ordering.Default => "전체",
				Ordering.Recent => "최신순",
				Ordering.Goods => "추천",
				Ordering.Bookmarks => "북마크",

				Ordering.Popular => throw new ProviderNotSupportedException(this, "Ordering by popular is not supported"),
				_ => throw new ArgumentOutOfRangeException(nameof(request.Ordering))
			};

			#endregion

			#region Request to String

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

			#endregion

			using var htmlMsg = await Client.SendAsync(() => DocsRequestHeader.CreateMsg(
				$"{HomePageUri}/comic_list?page={page.ToString()}&" + keywordQ + artistQ + intervalQ + tagsQ +
				orderingQ,
				HttpMethod.Get));

			var htmlResult = await htmlMsg
				.DecompressContent()
				.ContinueWith(t => t.Result.ToString(htmlMsg.Content.Headers.ContentType.CharSet.GetEncoding()));

			if (!htmlMsg.StatusCode.IsSuccesses())
				return Request<ImmutableArray<SeriesInfo>>.Failed(htmlMsg.StatusCode);

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
					var tags = e.QuerySelector(".genre").Text()
						.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
					var thumbnailSrc = e.QuerySelector("img.main_img").GetAttribute("src");

					if (thumbnailSrc.StartsWith("/"))
						thumbnailSrc = thumbnailSrc.Insert(0, HomePageUri);

					using var imgReq = await Client.SendAsync(() => ImageRequestHeader.CreateMsg(thumbnailSrc, HttpMethod.Get));
					
					var content = await imgReq.DecompressContent();

					return await SeriesInfo.New(title, artist, publishInterval, tags, content, this);
				}).Select(t => t.Result);

			return Request<ImmutableArray<SeriesInfo>>.Successful(seriesList.ToImmutableArray(), htmlMsg.StatusCode);
		}
	}
}