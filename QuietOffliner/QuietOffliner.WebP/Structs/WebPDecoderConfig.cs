using System.Runtime.InteropServices;

namespace QuietOffliner.WebP.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPDecoderConfig
    {
        /// <summary>Immutable bitstream features (optional)</summary>
        public WebPBitstreamFeatures input;
        /// <summary>Output buffer (can point to external mem)</summary>
        public WebPDecBuffer output;
        /// <summary>Decoding options</summary>
        public WebPDecoderOptions options;
    }
}