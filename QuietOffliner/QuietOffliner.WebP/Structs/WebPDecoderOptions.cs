using System.Runtime.InteropServices;

namespace QuietOffliner.WebP.Structs
{
    /// <summary>Decoding options</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPDecoderOptions
    {
        /// <summary>if true, skip the in-loop filtering.</summary>
        public int bypass_filtering;
        /// <summary>if true, use faster pointwise upsampler.</summary>
        public int no_fancy_upsampling;
        /// <summary>if true, cropping is applied _first_</summary>
        public int use_cropping;
        /// <summary>left position for cropping. Will be snapped to even values.</summary>
        public int crop_left;
        /// <summary>top position for cropping. Will be snapped to even values.</summary>
        public int crop_top;
        /// <summary>width of the cropping area</summary>
        public int crop_width;
        /// <summary>height of the cropping area</summary>
        public int crop_height;
        /// <summary>if true, scaling is applied _afterward_</summary>
        public int use_scaling;
        /// <summary>final width</summary>
        public int scaled_width;
        /// <summary>final height</summary>
        public int scaled_height;
        /// <summary>if true, use multi-threaded decoding</summary>
        public int use_threads;
        /// <summary>dithering strength (0=Off, 100=full)</summary>
        public int dithering_strength;
        /// <summary>flip output vertically</summary>
        public int flip;
        /// <summary>alpha dithering strength in [0..100]</summary>
        public int alpha_dithering_strength;
        /// <summary>padding for later use.</summary>
        private readonly uint pad1;
        /// <summary>padding for later use.</summary>
        private readonly uint pad2;
        /// <summary>padding for later use.</summary>
        private readonly uint pad3;
        /// <summary>padding for later use.</summary>
        private readonly uint pad4;
        /// <summary>padding for later use.</summary>
        private readonly uint pad5;
    }
}