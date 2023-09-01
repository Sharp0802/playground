using System;
using System.Runtime.InteropServices;
using QuietOffliner.WebP.Enums;

namespace QuietOffliner.WebP.Structs
{
    /// <summary>Output buffer</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPDecBuffer
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        // ReSharper disable MemberCanBePrivate.Global

        /// <summary>Colorspace.</summary>
        public WebpCspMode colorspace;
        /// <summary>Width of image.</summary>
        public int width;
        /// <summary>Height of image.</summary>
        public int height;
        /// <summary>If non-zero, 'internal_memory' pointer is not used. If value is '2' or more, the external memory is considered 'slow' and multiple read/write will be avoided.</summary>
        public int is_external_memory;
        /// <summary>Output buffer parameters.</summary>
        public RgbaYuvaBuffer u;
        /// <summary>padding for later use.</summary>
        private readonly uint pad1;
        /// <summary>padding for later use.</summary>
        private readonly uint pad2;
        /// <summary>padding for later use.</summary>
        private readonly uint pad3;
        /// <summary>padding for later use.</summary>
        private readonly uint pad4;
        /// <summary>Internally allocated memory (only when is_external_memory is 0). Should not be used externally, but accessed via WebPRgbaBuffer.</summary>
        public IntPtr private_memory;
        
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global
    }
}