using System;
using System.Runtime.InteropServices;

namespace QuietOffliner.WebP.Structs
{
    /// <summary>Generic structure for describing the output sample buffer.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPRgbaBuffer
    {
        /// <summary>pointer to RGBA samples.</summary>
        public IntPtr rgba;
        /// <summary>stride in bytes from one scanline to the next.</summary>
        public int stride;
        /// <summary>total size of the rgba buffer.</summary>
        public UIntPtr size;
    }
}