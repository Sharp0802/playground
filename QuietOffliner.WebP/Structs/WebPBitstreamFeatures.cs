using System.Runtime.InteropServices;

namespace QuietOffliner.WebP.Structs
{
    /// <summary>Features gathered from the bitstream</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPBitstreamFeatures
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        
        public int Width;
        public int Height;
        public int Has_alpha;
        public int Has_animation;
        /// <summary>0 = undefined (/mixed), 1 = lossy, 2 = lossless</summary>
        public int Format;
        
        /// <summary>Padding for later use.</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        private readonly uint[] pad;
        
        // ReSharper restore FieldCanBeMadeReadOnly.Global
    }
}