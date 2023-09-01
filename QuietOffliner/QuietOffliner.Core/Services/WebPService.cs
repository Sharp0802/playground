using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace QuietOffliner.Core.Services
{
    public static class WebPService
    {
        public static Task<byte[]> Convert(this Bitmap bitmap)
        {
            using var encoder = new WebP.WebP();
            return Task.FromResult(encoder.EncodeLossless(bitmap));
        }
        public static Task<Bitmap> Convert(this byte[] webp)
        {
            using var decoder = new WebP.WebP();
            return Task.FromResult(decoder.Decode(webp));
        }

        public static async Task<byte[]> LoadWebP(string path)
        {
            using var fStream = File.OpenRead(path);
            using var mStream = new MemoryStream();

            await fStream.CopyToAsync(mStream);
            
            mStream.Seek(0, SeekOrigin.Begin);

            return mStream.ToArray();
        }

        public static Task SaveWebP(byte[] webp, string dir, string name)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir).Create();

            File.WriteAllBytes(Path.Combine(dir, name), webp);

            return Task.CompletedTask;
        }
    }
}