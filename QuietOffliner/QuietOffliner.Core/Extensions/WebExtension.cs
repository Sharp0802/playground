using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Brotli;

namespace QuietOffliner.Core.Extensions
{
	public static class WebExtension
	{
		public static HttpRequestMessage CreateMsg(
			this IEnumerable<(string name, string value)> header,
			string url,
			HttpMethod method)
		{
			var msg = new HttpRequestMessage
			{
				RequestUri = new Uri(url),
				Method = method
			};


			foreach (var (name, value) in header)
				msg.Headers.Add(name, value);

			return msg;
		}

		public static async Task<byte[]> DecompressContent(this HttpResponseMessage msg)
		{
			using var rawDataStream = new MemoryStream(await msg.Content.ReadAsByteArrayAsync());
			
			using Stream? decompressor = msg.Content.Headers.ContentEncoding switch
			{
				_ when msg.Content.Headers.ContentEncoding.Any(s => s == "gzip")
					=> new GZipStream(rawDataStream, CompressionMode.Decompress),
				_ when msg.Content.Headers.ContentEncoding.Any(s => s == "deflate")
					=> new DeflateStream(rawDataStream, CompressionMode.Decompress),
				_ when msg.Content.Headers.ContentEncoding.Any(s => s == "br")
					=> new BrotliStream(rawDataStream, CompressionMode.Decompress),
				_ => null
			};

			using var resultStream = new MemoryStream();
			await (decompressor ?? rawDataStream).CopyToAsync(resultStream);
			resultStream.Seek(0, SeekOrigin.Begin);

			return resultStream.ToArray();
		}
	}
}