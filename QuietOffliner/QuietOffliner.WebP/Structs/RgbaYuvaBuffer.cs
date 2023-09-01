using System.Runtime.InteropServices;

namespace QuietOffliner.WebP.Structs
{
    /// <summary>Union of buffer parameters</summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct RgbaYuvaBuffer
    {
        // ReSharper disable MemberCanBePrivate.Global
        
        [FieldOffset(0)]
        public WebPRgbaBuffer RGBA;

        [FieldOffset(0)]
        public WebPYuvaBuffer YUVA;
        
        // ReSharper restore MemberCanBePrivate.Global
    }
}