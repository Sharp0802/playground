using System;
using QuietOffliner.WebP.Enums;
using QuietOffliner.WebP.Structs;

namespace QuietOffliner.WebP.Natives
{
    internal static partial class NativeMethods
    {
        /// <summary>This function will initialize the configuration according to a predefined set of parameters (referred to by 'preset') and a given quality factor.</summary>
        /// <param name="config">The WebPConfig struct</param>
        /// <param name="preset">Type of image</param>
        /// <param name="quality">Quality of compression</param>
        /// <returns>0 if error</returns>
        public static int WebPConfigInit(ref WebPConfig config, WebPPreset preset, float quality)
        {
            return IntPtr.Size switch
            {
                4 => WebPConfigInitInternal_x86(ref config, preset, quality, WebpDecoderAbiVersion),
                8 => WebPConfigInitInternal_x64(ref config, preset, quality, WebpDecoderAbiVersion),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Get info of WepP image</summary>
        /// <param name="rawWebP">Bytes[] of webp image</param>
        /// <param name="dataSize">Size of rawWebP</param>
        /// <param name="features">Features of WebP image</param>
        /// <returns>VP8StatusCode</returns>
        public static Vp8StatusCode WebPGetFeatures(IntPtr rawWebP, int dataSize, ref WebPBitstreamFeatures features)
        {
            return IntPtr.Size switch
            {
                4 => WebPGetFeaturesInternal_x86(rawWebP, (UIntPtr) dataSize, ref features, WebpDecoderAbiVersion),
                8 => WebPGetFeaturesInternal_x64(rawWebP, (UIntPtr) dataSize, ref features, WebpDecoderAbiVersion),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Activate the lossless compression mode with the desired efficiency.</summary>
        /// <param name="config">The WebPConfig struct</param>
        /// <param name="level">between 0 (fastest, lowest compression) and 9 (slower, best compression)</param>
        /// <returns>0 in case of parameter error</returns>
        public static int WebPConfigLosslessPreset(ref WebPConfig config, int level)
        {
            return IntPtr.Size switch
            {
                4 => WebPConfigLosslessPreset_x86(ref config, level),
                8 => WebPConfigLosslessPreset_x64(ref config, level),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }
        
        /// <summary>Check that 'config' is non-NULL and all configuration parameters are within their valid ranges.</summary>
        public static int WebPValidateConfig(ref WebPConfig config)
        {
            return IntPtr.Size switch
            {
                4 => WebPValidateConfig_x86(ref config),
                8 => WebPValidateConfig_x64(ref config),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Init the struct WebPPicture checking the dll version</summary>
        public static int WebPPictureInitInternal(ref WebPPicture pic)
        {
            return IntPtr.Size switch
            {
                4 => WebPPictureInitInternal_x86(ref pic, WebpDecoderAbiVersion),
                8 => WebPPictureInitInternal_x64(ref pic, WebpDecoderAbiVersion),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Colorspace conversion function to import RGB samples.</summary>
        /// <param name="pic">The WebPPicture struct</param>
        /// <param name="bgr">Point to BGR data</param>
        /// <param name="stride">stride of BGR data</param>
        /// <returns>Returns 0 in case of memory error.</returns>
        public static int WebPPictureImportBgr(ref WebPPicture pic, IntPtr bgr, int stride)
        {
            return IntPtr.Size switch
            {
                4 => WebPPictureImportBGR_x86(ref pic, bgr, stride),
                8 => WebPPictureImportBGR_x64(ref pic, bgr, stride),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Colorspace conversion function to import RGB samples.</summary>
        /// <param name="pic">The WebPPicture struct</param>
        /// <param name="bgra">Point to BGRA data</param>
        /// <param name="stride">stride of BGRA data</param>
        /// <returns>Returns 0 in case of memory error.</returns>
        public static int WebPPictureImportBgra(ref WebPPicture pic, IntPtr bgra, int stride)
        {
            return IntPtr.Size switch
            {
                4 => WebPPictureImportBGRA_x86(ref pic, bgra, stride),
                8 => WebPPictureImportBGRA_x64(ref pic, bgra, stride),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Colorspace conversion function to import RGB samples.</summary>
        /// <param name="pic">The WebPPicture struct</param>
        /// <param name="bgr">Point to BGR data</param>
        /// <param name="stride">stride of BGR data</param>
        /// <returns>Returns 0 in case of memory error.</returns>
        public static int WebPPictureImportBgrx(ref WebPPicture pic, IntPtr bgr, int stride)
        {
            return IntPtr.Size switch
            {
                4 => WebPPictureImportBGRX_x86(ref pic, bgr, stride),
                8 => WebPPictureImportBGRX_x64(ref pic, bgr, stride),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Compress to webp format</summary>
        /// <param name="config">The config struct for compresion parameters</param>
        /// <param name="picture">'picture' hold the source samples in both YUV(A) or ARGB input</param>
        /// <returns>Returns 0 in case of error, 1 otherwise. In case of error, picture->error_code is updated accordingly.</returns>
        public static int WebPEncode(ref WebPConfig config, ref WebPPicture picture)
        {
            return IntPtr.Size switch
            {
                4 => WebPEncode_x86(ref config, ref picture),
                8 => WebPEncode_x64(ref config, ref picture),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Release the memory allocated by WebPPictureAlloc() or WebPPictureImport*()
        /// Note that this function does _not_ free the memory used by the 'picture' object itself.
        /// Besides memory (which is reclaimed) all other fields of 'picture' are preserved.</summary>
        /// <param name="picture">Picture struct</param>
        public static void WebPPictureFree(ref WebPPicture picture)
        {
            switch (IntPtr.Size)
            {
                case 4:     WebPPictureFree_x86(ref picture); break;
                case 8:     WebPPictureFree_x64(ref picture); break;
                default:    throw new InvalidOperationException("Invalid platform. Can not find proper function");
            }
        }

        /// <summary>Validate the WebP image header and retrieve the image height and width. Pointers *width and *height can be passed NULL if deemed irrelevant</summary>
        /// <param name="data">Pointer to WebP image data</param>
        /// <param name="dataSize">This is the size of the memory block pointed to by data containing the image data</param>
        /// <param name="width">The range is limited currently from 1 to 16383</param>
        /// <param name="height">The range is limited currently from 1 to 16383</param>
        /// <returns>1 if success, otherwise error code returned in the case of (a) formatting error(s).</returns>
        public static int WebPGetInfo(IntPtr data, int dataSize, out int width, out int height)
        {
            return IntPtr.Size switch
            {
                4 => WebPGetInfo_x86(data, (UIntPtr) dataSize, out width, out height),
                8 => WebPGetInfo_x64(data, (UIntPtr) dataSize, out width, out height),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Decode WEBP image pointed to by *data and returns BGR samples into a pre-allocated buffer</summary>
        /// <param name="data">Pointer to WebP image data</param>
        /// <param name="dataSize">This is the size of the memory block pointed to by data containing the image data</param>
        /// <param name="outputBuffer">Pointer to decoded WebP image</param>
        /// <param name="outputBufferSize">Size of allocated buffer</param>
        /// <param name="outputStride">Specifies the distance between scanlines</param>
        /// <returns>output_buffer if function succeeds; NULL otherwise</returns>
        public static int WebPDecodeBgrInto(IntPtr data, int dataSize, IntPtr outputBuffer, int outputBufferSize, int outputStride)
        {
            return IntPtr.Size switch
            {
                4 => WebPDecodeBGRInto_x86(data, (UIntPtr) dataSize, outputBuffer, outputBufferSize, outputStride),
                8 => WebPDecodeBGRInto_x64(data, (UIntPtr) dataSize, outputBuffer, outputBufferSize, outputStride),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Decode WEBP image pointed to by *data and returns BGR samples into a pre-allocated buffer</summary>
        /// <param name="data">Pointer to WebP image data</param>
        /// <param name="dataSize">This is the size of the memory block pointed to by data containing the image data</param>
        /// <param name="outputBuffer">Pointer to decoded WebP image</param>
        /// <param name="outputBufferSize">Size of allocated buffer</param>
        /// <param name="outputStride">Specifies the distance between scanlines</param>
        /// <returns>output_buffer if function succeeds; NULL otherwise</returns>
        public static int WebPDecodeBgraInto(IntPtr data, int dataSize, IntPtr outputBuffer, int outputBufferSize, int outputStride)
        {
            return IntPtr.Size switch
            {
                4 => WebPDecodeBGRAInto_x86(data, (UIntPtr) dataSize, outputBuffer, outputBufferSize, outputStride),
                8 => WebPDecodeBGRAInto_x64(data, (UIntPtr) dataSize, outputBuffer, outputBufferSize, outputStride),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Initialize the configuration as empty. This function must always be called first, unless WebPGetFeatures() is to be called.</summary>
        /// <param name="webPDecoderConfig">Configuration struct</param>
        /// <returns>False in case of mismatched version.</returns>
        public static int WebPInitDecoderConfig(ref WebPDecoderConfig webPDecoderConfig)
        {
            return IntPtr.Size switch
            {
                4 => WebPInitDecoderConfigInternal_x86(ref webPDecoderConfig, WebpDecoderAbiVersion),
                8 => WebPInitDecoderConfigInternal_x64(ref webPDecoderConfig, WebpDecoderAbiVersion),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Decodes the full data at once, taking 'config' into account.</summary>
        /// <param name="data">WebP raw data to decode</param>
        /// <param name="dataSize">Size of WebP data </param>
        /// <param name="webPDecoderConfig">Configuration struct</param>
        /// <returns>VP8_STATUS_OK if the decoding was successful</returns>
        public static Vp8StatusCode WebPDecode(IntPtr data, int dataSize, ref WebPDecoderConfig webPDecoderConfig)
        {
            return IntPtr.Size switch
            {
                4 => WebPDecode_x86(data, (UIntPtr) dataSize, ref webPDecoderConfig),
                8 => WebPDecode_x64(data, (UIntPtr) dataSize, ref webPDecoderConfig),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Free any memory associated with the buffer. Must always be called last. Doesn't free the 'buffer' structure itself.</summary>
        /// <param name="buffer">WebPDecBuffer</param>
        public static void WebPFreeDecBuffer(ref WebPDecBuffer buffer)
        {
            switch (IntPtr.Size)
            {
                case 4:     WebPFreeDecBuffer_x86(ref buffer); break;
                case 8:     WebPFreeDecBuffer_x64(ref buffer); break;
                default:    throw new InvalidOperationException("Invalid platform. Can not find proper function");
            }
        }

        /// <summary>Lossy encoding images</summary>
        /// <param name="bgr">Pointer to BGR image data</param>
        /// <param name="width">The range is limited currently from 1 to 16383</param>
        /// <param name="height">The range is limited currently from 1 to 16383</param>
        /// <param name="stride">Specifies the distance between scanlines</param>
        /// <param name="qualityFactor">Ranges from 0 (lower quality) to 100 (highest quality). Controls the loss and quality during compression</param>
        /// <param name="output">output_buffer with WebP image</param>
        /// <returns>Size of WebP Image or 0 if an error occurred</returns>
        public static int WebPEncodeBgr(IntPtr bgr, int width, int height, int stride, float qualityFactor, out IntPtr output)
        {
            return IntPtr.Size switch
            {
                4 => WebPEncodeBGR_x86(bgr, width, height, stride, qualityFactor, out output),
                8 => WebPEncodeBGR_x64(bgr, width, height, stride, qualityFactor, out output),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Lossy encoding images</summary>
        /// <param name="bgra">Pointer to BGRA image data</param>
        /// <param name="width">The range is limited currently from 1 to 16383</param>
        /// <param name="height">The range is limited currently from 1 to 16383</param>
        /// <param name="stride">Specifies the distance between scanlines</param>
        /// <param name="qualityFactor">Ranges from 0 (lower quality) to 100 (highest quality). Controls the loss and quality during compression</param>
        /// <param name="output">output_buffer with WebP image</param>
        /// <returns>Size of WebP Image or 0 if an error occurred</returns>
        public static int WebPEncodeBgra(IntPtr bgra, int width, int height, int stride, float qualityFactor, out IntPtr output)
        {
            return IntPtr.Size switch
            {
                4 => WebPEncodeBGRA_x86(bgra, width, height, stride, qualityFactor, out output),
                8 => WebPEncodeBGRA_x64(bgra, width, height, stride, qualityFactor, out output),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Lossless encoding images pointed to by *data in WebP format</summary>
        /// <param name="bgr">Pointer to BGR image data</param>
        /// <param name="width">The range is limited currently from 1 to 16383</param>
        /// <param name="height">The range is limited currently from 1 to 16383</param>
        /// <param name="stride">Specifies the distance between scanlines</param>
        /// <param name="output">output_buffer with WebP image</param>
        /// <returns>Size of WebP Image or 0 if an error occurred</returns>
        public static int WebPEncodeLosslessBgr(IntPtr bgr, int width, int height, int stride, out IntPtr output)
        {
            return IntPtr.Size switch
            {
                4 => WebPEncodeLosslessBGR_x86(bgr, width, height, stride, out output),
                8 => WebPEncodeLosslessBGR_x64(bgr, width, height, stride, out output),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Lossless encoding images pointed to by *data in WebP format</summary>
        /// <param name="bgra">Pointer to BGR image data</param>
        /// <param name="width">The range is limited currently from 1 to 16383</param>
        /// <param name="height">The range is limited currently from 1 to 16383</param>
        /// <param name="stride">Specifies the distance between scanlines</param>
        /// <param name="output">output_buffer with WebP image</param>
        /// <returns>Size of WebP Image or 0 if an error occurred</returns>
        public static int WebPEncodeLosslessBgra(IntPtr bgra, int width, int height, int stride, out IntPtr output)
        {
            return IntPtr.Size switch
            {
                4 => WebPEncodeLosslessBGRA_x86(bgra, width, height, stride, out output),
                8 => WebPEncodeLosslessBGRA_x64(bgra, width, height, stride, out output),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Releases memory returned by the WebPEncode</summary>
        /// <param name="p">Pointer to memory</param>
        public static void WebPFree(IntPtr p)
        {
            switch (IntPtr.Size)
            {
                case 4:     WebPFree_x86(p); break;
                case 8:     WebPFree_x64(p); break;
                default:    throw new InvalidOperationException("Invalid platform. Can not find proper function");
            }
        }

        /// <summary>Get the webp version library</summary>
        /// <returns>8bits for each of major/minor/revision packet in integer. E.g: v2.5.7 is 0x020507</returns>
        public static int WebPGetDecoderVersion()
        {
            return IntPtr.Size switch
            {
                4 => WebPGetDecoderVersion_x86(),
                8 => WebPGetDecoderVersion_x64(),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }

        /// <summary>Compute PSNR, SSIM or LSIM distortion metric between two pictures.</summary>
        /// <param name="srcPicture">Picture to measure</param>
        /// <param name="refPicture">Reference picture</param>
        /// <param name="metricType">0 = PSNR, 1 = SSIM, 2 = LSIM</param>
        /// <param name="pResult">dB in the Y/U/V/Alpha/All order</param>
        /// <returns>False in case of error (src and ref don't have same dimension, ...)</returns>
        public static int WebPPictureDistortion(ref WebPPicture srcPicture, ref WebPPicture refPicture, int metricType, IntPtr pResult)
        {
            return IntPtr.Size switch
            {
                4 => WebPPictureDistortion_x86(ref srcPicture, ref refPicture, metricType, pResult),
                8 => WebPPictureDistortion_x64(ref srcPicture, ref refPicture, metricType, pResult),
                _ => throw new InvalidOperationException("Invalid platform. Can not find proper function")
            };
        }
    }
}