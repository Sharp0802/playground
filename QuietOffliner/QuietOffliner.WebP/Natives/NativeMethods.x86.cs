using System;
using System.Runtime.InteropServices;
using System.Security;
using QuietOffliner.WebP.Enums;
using QuietOffliner.WebP.Structs;

namespace QuietOffliner.WebP.Natives
{
    [SuppressUnmanagedCodeSecurity]
    internal static partial class NativeMethods
    {

        [DllImport("kernel32.dll",
            EntryPoint = "CopyMemory",
            SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private const int WebpDecoderAbiVersion = 0x0208;
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPConfigInitInternal")]
        private static extern int WebPConfigInitInternal_x86(
            ref WebPConfig config,
            WebPPreset preset,
            float quality,
            int webpDecoderAbiVersion);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPGetFeaturesInternal")]
        private static extern Vp8StatusCode WebPGetFeaturesInternal_x86(
            [In] IntPtr rawWebP,
            UIntPtr dataSize,
            ref WebPBitstreamFeatures features,
            int webpDecoderAbiVersion);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPConfigLosslessPreset")]
        private static extern int WebPConfigLosslessPreset_x86(
            ref WebPConfig config,
            int level);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPValidateConfig")]
        private static extern int WebPValidateConfig_x86(
            ref WebPConfig config);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPPictureInitInternal")]
        private static extern int WebPPictureInitInternal_x86(
            ref WebPPicture pic,
            int webpDecoderAbiVersion);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPPictureImportBGR")]
        private static extern int WebPPictureImportBGR_x86(
            ref WebPPicture pic,
            IntPtr bgr,
            int stride);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPPictureImportBGRA")]
        private static extern int WebPPictureImportBGRA_x86(
            ref WebPPicture pic,
            IntPtr bgra,
            int stride);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPPictureImportBGRX")]
        private static extern int WebPPictureImportBGRX_x86(
            ref WebPPicture pic,
            IntPtr bgr,
            int stride);

        /// <summary>The writer type for output compress data</summary>
        /// <param name="data">Data returned</param>
        /// <param name="dataSize">Size of data returned</param>
        /// <param name="pic">Picture struct</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int WebPMemoryWrite([In] IntPtr data, UIntPtr dataSize, ref WebPPicture pic);
        
        public static WebPMemoryWrite OnCallback;
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPEncode")]
        private static extern int WebPEncode_x86(
            ref WebPConfig config,
            ref WebPPicture picture);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPPictureFree")]
        private static extern void WebPPictureFree_x86(
            ref WebPPicture pic);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPGetInfo")]
        private static extern int WebPGetInfo_x86(
            [In] IntPtr data,
            UIntPtr dataSize,
            out int width,
            out int height);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPDecodeBGRInto")]
        private static extern int WebPDecodeBGRInto_x86(
            [In] IntPtr data,
            UIntPtr dataSize,
            IntPtr outputBuffer,
            int outputBufferSize, 
            int outputStride);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPDecodeBGRAInto")]
        private static extern int WebPDecodeBGRAInto_x86(
            [In] IntPtr data,
            UIntPtr dataSize,
            IntPtr outputBuffer,
            int outputBufferSize,
            int outputStride);

        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPInitDecoderConfigInternal")]
        private static extern int WebPInitDecoderConfigInternal_x86(
            ref WebPDecoderConfig webPDecoderConfig,
            int webpDecoderAbiVersion);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPDecode")]
        private static extern Vp8StatusCode WebPDecode_x86(
            IntPtr data,
            UIntPtr dataSize,
            ref WebPDecoderConfig config);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPFreeDecBuffer")]
        private static extern void WebPFreeDecBuffer_x86(
            ref WebPDecBuffer buffer);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPEncodeBGR")]
        private static extern int WebPEncodeBGR_x86(
            [In] IntPtr bgr,
            int width,
            int height,
            int stride,
            float qualityFactor,
            out IntPtr output);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPEncodeBGRA")]
        private static extern int WebPEncodeBGRA_x86(
            [In] IntPtr bgra,
            int width,
            int height,
            int stride,
            float qualityFactor,
            out IntPtr output);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPEncodeLosslessBGR")]
        private static extern int WebPEncodeLosslessBGR_x86(
            [In] IntPtr bgr,
            int width,
            int height,
            int stride,
            out IntPtr output);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPEncodeLosslessBGRA")]
        private static extern int WebPEncodeLosslessBGRA_x86(
            [In] IntPtr bgra,
            int width,
            int height,
            int stride,
            out IntPtr output);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPFree")]
        private static extern void WebPFree_x86(
            IntPtr p);
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPGetDecoderVersion")]
        private static extern int WebPGetDecoderVersion_x86();
        
        [DllImport("libwebp_x86.dll",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "WebPPictureDistortion")]
        private static extern int WebPPictureDistortion_x86(
            ref WebPPicture srcPicture,
            ref WebPPicture refPicture, 
            int metricType, 
            IntPtr pResult);
    }
}