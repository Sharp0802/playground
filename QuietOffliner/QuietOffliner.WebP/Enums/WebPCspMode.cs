namespace QuietOffliner.WebP.Enums
{
    /// <summary>Describes the byte-ordering of packed samples in memory.</summary>
    public enum WebpCspMode
    {
        // ReSharper disable InconsistentNaming
        
        /// <summary>Byte-order: R,G,B,R,G,B,...</summary>
        Mode_RGB = 0,
        /// <summary>Byte-order: R,G,B,A,R,G,B,A,...</summary>
        Mode_RGBA = 1,
        /// <summary>Byte-order: B,G,R,B,G,R,...</summary>
        Mode_BGR = 2,
        /// <summary>Byte-order: B,G,R,A,B,G,R,A,...</summary>
        Mode_BGRA = 3,
        /// <summary>Byte-order: A,R,G,B,A,R,G,B,...</summary>
        Mode_ARGB = 4,
        /// <summary>Byte-order: RGB-565: [a4 a3 a2 a1 a0 r5 r4 r3], [r2 r1 r0 g4 g3 g2 g1 g0], ...
        /// WEBP_SWAP_16BITS_CSP is defined, 
        /// Byte-order: RGB-565: [a4 a3 a2 a1 a0 b5 b4 b3], [b2 b1 b0 g4 g3 g2 g1 g0], ...</summary>
        Mode_RGBA4444 = 5,
        /// <summary>Byte-order: RGB-565: [r4 r3 r2 r1 r0 g5 g4 g3], [g2 g1 g0 b4 b3 b2 b1 b0], ...
        /// WEBP_SWAP_16BITS_CSP is defined, 
        /// Byte-order: [b3 b2 b1 b0 a3 a2 a1 a0], [r3 r2 r1 r0 g3 g2 g1 g0], ...</summary>
        Mode_RGB565 = 6,
        /// <summary>RGB-premultiplied transparent modes (alpha value is preserved)</summary>
        ModeRgbA = 7,
        /// <summary>RGB-premultiplied transparent modes (alpha value is preserved)</summary>
        ModeBgrA = 8,
        /// <summary>RGB-premultiplied transparent modes (alpha value is preserved)</summary>
        ModeArgb = 9,
        /// <summary>RGB-premultiplied transparent modes (alpha value is preserved)</summary>
        ModeRgbA4444 = 10,
        /// <summary>yuv 4:2:0</summary>
        ModeYuv = 11,
        /// <summary>yuv 4:2:0</summary>
        ModeYuva = 12,
        /// <summary>MODE_LAST -> 13</summary>
        ModeLast = 13,
        
        // ReSharper restore InconsistentNaming
    }
}