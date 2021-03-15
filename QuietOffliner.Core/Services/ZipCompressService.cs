using System.IO.Compression;
using System.Threading.Tasks;

namespace QuietOffliner.Core.Services
{
    public static class ZipCompressService
    {
        public static async Task Compress(
            this string fromDir,
            string toZip,
            CompressionLevel compressLevel)
            => await Task.Factory.StartNew(() =>
                ZipFile.CreateFromDirectory(
                    fromDir,
                    toZip,
                    compressLevel,
                    true));
        
        public static async Task Decompress(
            this string fromZip,
            string toDir)
            => await Task.Factory.StartNew(() =>
                ZipFile.ExtractToDirectory(
                    fromZip,
                    toDir));
    }
}