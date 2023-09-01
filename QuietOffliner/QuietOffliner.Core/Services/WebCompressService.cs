using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Brotli;

namespace QuietOffliner.Core.Services
{
    public static class WebCompressService
    {
        public static async Task<byte[]> WebDecompress(this byte[] content, IEnumerable<string> encodings)
        {
            using var inStream = new MemoryStream(content);

            var encodingArray = encodings as string[] ?? encodings.ToArray();
            using Stream? decompressor = encodings switch
            {
                _ when encodingArray.Any(e => e == "br")
                    => new BrotliStream(inStream, CompressionMode.Decompress),
                _ when encodingArray.Any(e => e == "deflate")
                    => new DeflateStream(inStream, CompressionMode.Decompress),
                _ when encodingArray.Any(e => e == "gzip")
                    => new GZipStream(inStream, CompressionMode.Decompress),
                _   => null
            };

            if (decompressor is null)
                return content;

            using var outStream = new MemoryStream();
            await decompressor.CopyToAsync(outStream);
            outStream.Seek(0, SeekOrigin.Begin);
            
            return outStream.ToArray();
        }
    }
}