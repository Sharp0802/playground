using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using QuietOffliner.WebP.Enums;
using QuietOffliner.WebP.Structs;

namespace QuietOffliner.WebP
{
    public sealed class WebP : IDisposable
    {
        private const int WebpMaxDimension = 16383;
        
        #region | Public Decode Functions |
        
        /// <summary>Read a WebP file</summary>
        /// <param name="pathFileName">WebP file to load</param>
        /// <returns>Bitmap with the WebP image</returns>
        public Bitmap Load(string pathFileName)
        {
            try
            {
                var rawWebP = File.ReadAllBytes(pathFileName);

                return Decode(rawWebP);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.Load");
            }
        }

        /// <summary>Decode a WebP image</summary>
        /// <param name="rawWebP">The data to uncompress</param>
        /// <returns>Bitmap with the WebP image</returns>
        public Bitmap Decode(byte[] rawWebP)
        {
            Bitmap bmp = null;
            BitmapData bmpData = null;
            var pinnedWebP = GCHandle.Alloc(rawWebP, GCHandleType.Pinned);

            try
            {
                //Get image width and height
                GetInfo(
                    rawWebP,
                    out var imgWidth,
                    out var imgHeight,
                    out var hasAlpha,
                    out _,
                    out _);

                //Create a BitmapData and Lock all pixels to be written
                bmp = hasAlpha
                    ? new Bitmap(imgWidth, imgHeight, PixelFormat.Format32bppArgb)
                    : new Bitmap(imgWidth, imgHeight, PixelFormat.Format24bppRgb);
                bmpData = bmp.LockBits(
                    new Rectangle(0, 0, imgWidth, imgHeight),
                    ImageLockMode.WriteOnly,
                    bmp.PixelFormat);

                //Uncompress the image
                var outputSize = bmpData.Stride * imgHeight;
                var ptrData = pinnedWebP.AddrOfPinnedObject();
                var size = bmp.PixelFormat == PixelFormat.Format24bppRgb ?
                    Natives.NativeMethods.WebPDecodeBgrInto(ptrData, rawWebP.Length, bmpData.Scan0, outputSize, bmpData.Stride) :
                    Natives.NativeMethods.WebPDecodeBgraInto(ptrData, rawWebP.Length, bmpData.Scan0, outputSize, bmpData.Stride);
                if (size == 0)
                    throw new Exception("Can´t encode WebP");

                return bmp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.Decode");
            }
            finally
            {
                //Unlock the pixels
                if (bmpData is not null)
                    bmp.UnlockBits(bmpData);

                //Free memory
                if (pinnedWebP.IsAllocated)
                    pinnedWebP.Free();
            }
        }

        /// <summary>Decode a WebP image</summary>
        /// <param name="rawWebP">the data to uncompress</param>
        /// <param name="options">Options for advanced decode</param>
        /// <returns>Bitmap with the WebP image</returns>
        public Bitmap Decode(byte[] rawWebP, WebPDecoderOptions options)
        {
            var pinnedWebP = GCHandle.Alloc(rawWebP, GCHandleType.Pinned);
            Bitmap bmp = null;
            BitmapData bmpData = null;
            try
            {
                var config = new WebPDecoderConfig();
                if (Natives.NativeMethods.WebPInitDecoderConfig(ref config) == 0)
                {
                    throw new Exception("WebPInitDecoderConfig failed. Wrong version?");
                }

                // Read the .webp input file information
                var ptrRawWebP = pinnedWebP.AddrOfPinnedObject();
                Vp8StatusCode result;
                if (options.use_scaling == 0)
                {
                    result = Natives.NativeMethods.WebPGetFeatures(ptrRawWebP, rawWebP.Length, ref config.input);
                    if (result != Vp8StatusCode.Vp8StatusOk)
                        throw new Exception("Failed WebPGetFeatures with error " + result);

                    //Test cropping values
                    if (options.use_cropping == 1)
                    {
                        if (options.crop_left + options.crop_width > config.input.Width ||
                            options.crop_top + options.crop_height > config.input.Height)
                            throw new Exception("Crop options exceeded WebP image dimensions");
                    }
                }

                config.options.bypass_filtering = options.bypass_filtering;
                config.options.no_fancy_upsampling = options.no_fancy_upsampling;
                config.options.use_cropping = options.use_cropping;
                config.options.crop_left = options.crop_left;
                config.options.crop_top = options.crop_top;
                config.options.crop_width = options.crop_width;
                config.options.crop_height = options.crop_height;
                config.options.use_scaling = options.use_scaling;
                config.options.scaled_width = options.scaled_width;
                config.options.scaled_height = options.scaled_height;
                config.options.use_threads = options.use_threads;
                config.options.dithering_strength = options.dithering_strength;
                config.options.flip = options.flip;
                config.options.alpha_dithering_strength = options.alpha_dithering_strength;

                //Create a BitmapData and Lock all pixels to be written
                if (config.input.Has_alpha == 1)
                {
                    config.output.colorspace = WebpCspMode.ModeBgrA;
                    bmp = new Bitmap(config.input.Width, config.input.Height, PixelFormat.Format32bppArgb);
                }
                else
                {
                    config.output.colorspace = WebpCspMode.Mode_BGR;
                    bmp = new Bitmap(config.input.Width, config.input.Height, PixelFormat.Format24bppRgb);
                }

                bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

                // Specify the output format
                config.output.u.RGBA.rgba = bmpData.Scan0;
                config.output.u.RGBA.stride = bmpData.Stride;
                config.output.u.RGBA.size = (UIntPtr) (bmp.Height * bmpData.Stride);
                config.output.height = bmp.Height;
                config.output.width = bmp.Width;
                config.output.is_external_memory = 1;

                // Decode
                result = Natives.NativeMethods.WebPDecode(ptrRawWebP, rawWebP.Length, ref config);
                if (result != Vp8StatusCode.Vp8StatusOk)
                {
                    throw new Exception("Failed WebPDecode with error " + result);
                }

                Natives.NativeMethods.WebPFreeDecBuffer(ref config.output);

                return bmp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.Decode");
            }
            finally
            {
                //Unlock the pixels
                if (bmpData != null)
                    bmp.UnlockBits(bmpData);

                //Free memory
                if (pinnedWebP.IsAllocated)
                    pinnedWebP.Free();
            }
        }

        /// <summary>Get Thumbnail from webP in mode faster/low quality</summary>
        /// <param name="rawWebP">The data to uncompress</param>
        /// <param name="width">Wanted width of thumbnail</param>
        /// <param name="height">Wanted height of thumbnail</param>
        /// <returns>Bitmap with the WebP thumbnail in 24bpp</returns>
        public Bitmap GetThumbnailFast(byte[] rawWebP, int width, int height)
        {
            var pinnedWebP = GCHandle.Alloc(rawWebP, GCHandleType.Pinned);
            Bitmap bmp = null;
            BitmapData bmpData = null;

            try
            {
                var config = new WebPDecoderConfig();
                if (Natives.NativeMethods.WebPInitDecoderConfig(ref config) == 0)
                    throw new Exception("WebPInitDecoderConfig failed. Wrong version?");

                // Set up decode options
                config.options.bypass_filtering = 1;
                config.options.no_fancy_upsampling = 1;
                config.options.use_threads = 1;
                config.options.use_scaling = 1;
                config.options.scaled_width = width;
                config.options.scaled_height = height;

                // Create a BitmapData and Lock all pixels to be written
                bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);

                // Specify the output format
                config.output.colorspace = WebpCspMode.Mode_BGR;
                config.output.u.RGBA.rgba = bmpData.Scan0;
                config.output.u.RGBA.stride = bmpData.Stride;
                config.output.u.RGBA.size = (UIntPtr) (height * bmpData.Stride);
                config.output.height = height;
                config.output.width = width;
                config.output.is_external_memory = 1;

                // Decode
                IntPtr ptrRawWebP = pinnedWebP.AddrOfPinnedObject();
                Vp8StatusCode result = Natives.NativeMethods.WebPDecode(ptrRawWebP, rawWebP.Length, ref config);
                if (result != Vp8StatusCode.Vp8StatusOk)
                    throw new Exception("Failed WebPDecode with error " + result);

                Natives.NativeMethods.WebPFreeDecBuffer(ref config.output);

                return bmp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.Thumbnail");
            }
            finally
            {
                //Unlock the pixels
                if (bmpData != null)
                    bmp.UnlockBits(bmpData);

                //Free memory
                if (pinnedWebP.IsAllocated)
                    pinnedWebP.Free();
            }
        }

        /// <summary>Thumbnail from webP in mode slow/high quality</summary>
        /// <param name="rawWebP">The data to uncompress</param>
        /// <param name="width">Wanted width of thumbnail</param>
        /// <param name="height">Wanted height of thumbnail</param>
        /// <returns>Bitmap with the WebP thumbnail</returns>
        public Bitmap GetThumbnailQuality(byte[] rawWebP, int width, int height)
        {
            var pinnedWebP = GCHandle.Alloc(rawWebP, GCHandleType.Pinned);
            Bitmap bmp = null;
            BitmapData bmpData = null;

            try
            {
                var config = new WebPDecoderConfig();
                if (Natives.NativeMethods.WebPInitDecoderConfig(ref config) == 0)
                    throw new Exception("WebPInitDecoderConfig failed. Wrong version?");

                var ptrRawWebP = pinnedWebP.AddrOfPinnedObject();
                var result = Natives.NativeMethods.WebPGetFeatures(ptrRawWebP, rawWebP.Length, ref config.input);
                if (result != Vp8StatusCode.Vp8StatusOk)
                    throw new Exception("Failed WebPGetFeatures with error " + result);

                // Set up decode options
                config.options.bypass_filtering = 0;
                config.options.no_fancy_upsampling = 0;
                config.options.use_threads = 1;
                config.options.use_scaling = 1;
                config.options.scaled_width = width;
                config.options.scaled_height = height;

                //Create a BitmapData and Lock all pixels to be written
                if (config.input.Has_alpha == 1)
                {
                    config.output.colorspace = WebpCspMode.ModeBgrA;
                    bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                }
                else
                {
                    config.output.colorspace = WebpCspMode.Mode_BGR;
                    bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                }
                bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);

                // Specify the output format
                config.output.u.RGBA.rgba = bmpData.Scan0;
                config.output.u.RGBA.stride = bmpData.Stride;
                config.output.u.RGBA.size = (UIntPtr)(height * bmpData.Stride);
                config.output.height = height;
                config.output.width = width;
                config.output.is_external_memory = 1;

                // Decode
                result = Natives.NativeMethods.WebPDecode(ptrRawWebP, rawWebP.Length, ref config);
                if (result != Vp8StatusCode.Vp8StatusOk)
                    throw new Exception("Failed WebPDecode with error " + result);

                Natives.NativeMethods.WebPFreeDecBuffer(ref config.output);

                return bmp;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn WebP.Thumbnail"); }
            finally
            {
                //Unlock the pixels
                if (bmpData != null)
                    bmp.UnlockBits(bmpData);

                //Free memory
                if (pinnedWebP.IsAllocated)
                    pinnedWebP.Free();
            }
        }
        #endregion

        #region | Public Encode Functions |
        /// <summary>Save bitmap to file in WebP format</summary>
        /// <param name="bmp">Bitmap with the WebP image</param>
        /// <param name="pathFileName">The file to write</param>
        /// <param name="quality">Between 0 (lower quality, lowest file size) and 100 (highest quality, higher file size)</param>
        public void Save(Bitmap bmp, string pathFileName, int quality = 75)
        {
            try
            {
                //Encode in webP format
                var rawWebP = EncodeLossy(bmp, quality);

                //Write webP file
                File.WriteAllBytes(pathFileName, rawWebP);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.Save");
            }
        }

        /// <summary>Lossy encoding bitmap to WebP (Simple encoding API)</summary>
        /// <param name="bmp">Bitmap with the image</param>
        /// <param name="quality">Between 0 (lower quality, lowest file size) and 100 (highest quality, higher file size)</param>
        /// <returns>Compressed data</returns>
        public static byte[] EncodeLossy(Bitmap bmp, int quality = 75)
        {
            //test bmp
            if (bmp.Width == 0 || bmp.Height == 0)
                throw new ArgumentNullException(nameof(bmp), @"Bitmap contains no data.");
            if (bmp.Width > WebpMaxDimension || bmp.Height > WebpMaxDimension)
                throw new NotSupportedException($"Dimension of bitmap is too large. Max is {WebpMaxDimension}x{WebpMaxDimension} pixels.");
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb && bmp.PixelFormat != PixelFormat.Format32bppArgb)
                throw new NotSupportedException("Only support Format24bppRgb and Format32bppArgb pixelFormat.");

            BitmapData bmpData = null;
            var unmanagedData = IntPtr.Zero;

            try
            {
                //Get bmp data
                bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                    bmp.PixelFormat);

                //Compress the bmp data
                var size = bmp.PixelFormat == PixelFormat.Format24bppRgb ?
                    Natives.NativeMethods.WebPEncodeBgr(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, quality, out unmanagedData) :
                    Natives.NativeMethods.WebPEncodeBgra(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, quality, out unmanagedData);
                if (size == 0)
                    throw new Exception("Can´t encode WebP");

                //Copy image compress data to output array
                var rawWebP = new byte[size];
                Marshal.Copy(unmanagedData, rawWebP, 0, size);

                return rawWebP;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.EncodeLossy");
            }
            finally
            {
                //Unlock the pixels
                if (bmpData != null)
                    bmp.UnlockBits(bmpData);

                //Free memory
                if (unmanagedData != IntPtr.Zero)
                    Natives.NativeMethods.WebPFree(unmanagedData);
            }
        }

        /// <summary>Lossy encoding bitmap to WebP (Advanced encoding API)</summary>
        /// <param name="bmp">Bitmap with the image</param>
        /// <param name="quality">Between 0 (lower quality, lowest file size) and 100 (highest quality, higher file size)</param>
        /// <param name="speed">Between 0 (fastest, lowest compression) and 9 (slower, best compression)</param>
        /// <param name="info"></param>
        /// <returns>Compressed data</returns>
        public byte[] EncodeLossy(Bitmap bmp, int quality, int speed, bool info = false)
        {
            //Initialize config struct
            var config = new WebPConfig();

            //Set compression parameters
            if (Natives.NativeMethods.WebPConfigInit(ref config, WebPPreset.WebpPresetDefault, 75) == 0)
                throw new Exception("Can´t config preset");

            // Add additional tuning:
            config.method = speed;
            if (config.method > 6)
                config.method = 6;
            config.quality = quality;
            config.auto_filter = 1;
            config.pass = speed + 1;
            config.segments = 4;
            config.partitions = 3;
            config.thread_level = 1;
            config.alpha_quality = quality;
            config.alpha_filtering = 2;
            config.use_sharp_yuv = 1;

            if (Natives.NativeMethods.WebPGetDecoderVersion() > 1082)     //Old version don´t support preprocessing 4
            {
                config.preprocessing = 4;
                config.use_sharp_yuv = 1;
            }
            else
                config.preprocessing = 3;

            return AdvancedEncode(bmp, config, info);
        }

        /// <summary>Lossless encoding bitmap to WebP (Simple encoding API)</summary>
        /// <param name="bmp">Bitmap with the image</param>
        /// <returns>Compressed data</returns>
        public byte[] EncodeLossless(Bitmap bmp)
        {
            //test bmp
            if (bmp.Width == 0 || bmp.Height == 0)
                throw new ArgumentException(@"Bitmap contains no data.", nameof(bmp));
            if (bmp.Width > WebpMaxDimension || bmp.Height > WebpMaxDimension)
                throw new NotSupportedException("Bitmap's dimension is too large. Max is " + WebpMaxDimension + "x" + WebpMaxDimension + " pixels.");
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb && bmp.PixelFormat != PixelFormat.Format32bppArgb)
                throw new NotSupportedException("Only support Format24bppRgb and Format32bppArgb pixelFormat.");

            BitmapData bmpData = null;
            var unmanagedData = IntPtr.Zero;
            try
            {
                //Get bmp data
                bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

                //Compress the bmp data
                var size = bmp.PixelFormat == PixelFormat.Format24bppRgb ?
                    Natives.NativeMethods.WebPEncodeLosslessBgr(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, out unmanagedData) :
                    Natives.NativeMethods.WebPEncodeLosslessBgra(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, out unmanagedData);

                //Copy image compress data to output array
                var rawWebP = new byte[size];
                Marshal.Copy(unmanagedData, rawWebP, 0, size);

                return rawWebP;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn WebP.EncodeLossless (Simple)"); }
            finally
            {
                //Unlock the pixels
                if (bmpData != null)
                    bmp.UnlockBits(bmpData);

                //Free memory
                if (unmanagedData != IntPtr.Zero)
                    Natives.NativeMethods.WebPFree(unmanagedData);
            }
        }

        /// <summary>Lossless encoding image in bitmap (Advanced encoding API)</summary>
        /// <param name="bmp">Bitmap with the image</param>
        /// <param name="speed">Between 0 (fastest, lowest compression) and 9 (slower, best compression)</param>
        /// <returns>Compressed data</returns>
        public byte[] EncodeLossless(Bitmap bmp, int speed)
        {
            //Initialize config struct
            var config = new WebPConfig();

            //Set compression parameters
            if (Natives.NativeMethods.WebPConfigInit(ref config, WebPPreset.WebpPresetDefault, (speed + 1) * 10) == 0)
                throw new Exception("Can´t config preset");

            //Old version of dll not support info and WebPConfigLosslessPreset
            if (Natives.NativeMethods.WebPGetDecoderVersion() > 1082)
            {
                if (Natives.NativeMethods.WebPConfigLosslessPreset(ref config, speed) == 0)
                    throw new Exception("Can´t config lossless preset");
            }
            else
            {
                config.lossless = 1;
                config.method = speed;
                if (config.method > 6)
                    config.method = 6;
                config.quality = (speed + 1) * 10;
            }
            config.pass = speed + 1;
            config.thread_level = 1;
            config.alpha_filtering = 2;
            config.use_sharp_yuv = 1;
            config.exact = 0;

            return AdvancedEncode(bmp, config, false);
        }

        /// <summary>Near lossless encoding image in bitmap</summary>
        /// <param name="bmp">Bitmap with the image</param>
        /// <param name="quality">Between 0 (lower quality, lowest file size) and 100 (highest quality, higher file size)</param>
        /// <param name="speed">Between 0 (fastest, lowest compression) and 9 (slower, best compression)</param>
        /// <returns>Compress data</returns>
        public byte[] EncodeNearLossless(Bitmap bmp, int quality, int speed = 9)
        {
            //test dll version
            if (Natives.NativeMethods.WebPGetDecoderVersion() <= 1082)
                throw new Exception("This dll version not suport EncodeNearLossless");

            //Initialize config struct
            var config = new WebPConfig();

            //Set compression parameters
            if (Natives.NativeMethods.WebPConfigInit(ref config, WebPPreset.WebpPresetDefault, (speed + 1) * 10) == 0)
                throw new Exception("Can´t config preset");
            if (Natives.NativeMethods.WebPConfigLosslessPreset(ref config, speed) == 0)
                throw new Exception("Can´t config lossless preset");
            config.pass = speed + 1;
            config.near_lossless = quality;
            config.thread_level = 1;
            config.alpha_filtering = 2;
            config.use_sharp_yuv = 1;
            config.exact = 0;

            return AdvancedEncode(bmp, config, false);
        }
        
        #endregion

        #region | Another Public Functions |
        
        /// <summary>Get the libwebp version</summary>
        /// <returns>Version of library</returns>
        public string GetVersion()
        {
            try
            {
                var v = (uint)Natives.NativeMethods.WebPGetDecoderVersion();
                var revision = v % 256;
                var minor = (v >> 8) % 256;
                var major = (v >> 16) % 256;
                return major + "." + minor + "." + revision;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn WebP.GetVersion"); }
        }

        /// <summary>Get info of WEBP data</summary>
        /// <param name="rawWebP">The data of WebP</param>
        /// <param name="width">width of image</param>
        /// <param name="height">height of image</param>
        /// <param name="hasAlpha">Image has alpha channel</param>
        /// <param name="hasAnimation">Image is a animation</param>
        /// <param name="format">Format of image: 0 = undefined (/mixed), 1 = lossy, 2 = lossless</param>
        public static void GetInfo(byte[] rawWebP, out int width, out int height, out bool hasAlpha, out bool hasAnimation, out string format)
        {
            var pinnedWebP = GCHandle.Alloc(rawWebP, GCHandleType.Pinned);

            try
            {
                var ptrRawWebP = pinnedWebP.AddrOfPinnedObject();

                var features = new WebPBitstreamFeatures();
                var result = Natives.NativeMethods.WebPGetFeatures(ptrRawWebP, rawWebP.Length, ref features);

                if (result != 0)
                    throw new Exception(result.ToString());

                width = features.Width;
                height = features.Height;
                hasAlpha = features.Has_alpha == 1;
                hasAnimation = features.Has_animation == 1;

                format = features.Format switch
                {
                    1 => "lossy",
                    2 => "lossless",
                    _ => "undefined"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.GetInfo");
            }
            finally
            {
                if (pinnedWebP.IsAllocated)
                    pinnedWebP.Free();
            }
        }

        /// <summary>Compute PSNR, SSIM or LSIM distortion metric between two pictures.
        /// Warning: this function is rather CPU-intensive.</summary>
        /// <param name="source">Picture to measure</param>
        /// <param name="reference">Reference picture</param>
        /// <param name="metricType">0 = PSNR, 1 = SSIM, 2 = LSIM</param>
        /// <returns>dB in the Y/U/V/Alpha/All order</returns>
        public float[] GetPictureDistortion(Bitmap source, Bitmap reference, int metricType)
        {
            var picSource = new WebPPicture();
            var picReference = new WebPPicture();
            BitmapData sourceBmpData = null;
            BitmapData referenceBmpData = null;
            var result = new float[5];
            var pinnedResult = GCHandle.Alloc(result, GCHandleType.Pinned);

            try
            {
                if (source is null)
                    throw new Exception("Source picture is void");
                if (reference is null)
                    throw new Exception("Reference picture is void");
                if (metricType > 2)
                    throw new Exception("Bad metric_type. Use 0 = PSNR, 1 = SSIM, 2 = LSIM");
                if (source.Width != reference.Width || source.Height != reference.Height)
                    throw new Exception("Source and Reference pictures have diferent dimensions");

                // Setup the source picture data, allocating the bitmap, width and height
                sourceBmpData = source.LockBits(
                    new Rectangle(0, 0, source.Width, source.Height),
                    ImageLockMode.ReadOnly,
                    source.PixelFormat);
                
                picSource = new WebPPicture();
                if (Natives.NativeMethods.WebPPictureInitInternal(ref picSource) != 1)
                    throw new Exception("Can´t init WebPPictureInit");
                
                picSource.width = source.Width;
                picSource.height = source.Height;

                //Put the source bitmap components in pic
                if (sourceBmpData.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    picSource.use_argb = 1;
                    if (Natives.NativeMethods.WebPPictureImportBgra(ref picSource, sourceBmpData.Scan0,
                        sourceBmpData.Stride) != 1)
                        throw new Exception("Can´t allocate memory in WebPPictureImportBGR");
                }
                else
                {
                    picSource.use_argb = 0;
                    if (Natives.NativeMethods.WebPPictureImportBgr(ref picSource, sourceBmpData.Scan0,
                        sourceBmpData.Stride) != 1)
                        throw new Exception("Can´t allocate memory in WebPPictureImportBGR");
                }

                // Setup the reference picture data, allocating the bitmap, width and height
                referenceBmpData = reference.LockBits(
                    new Rectangle(
                        0, 0,
                        reference.Width, reference.Height),
                    ImageLockMode.ReadOnly,
                    reference.PixelFormat);

                picReference = new WebPPicture();
                if (Natives.NativeMethods.WebPPictureInitInternal(ref picReference) != 1)
                    throw new Exception("Can´t init WebPPictureInit");
                picReference.width = reference.Width;
                picReference.height = reference.Height;
                picReference.use_argb = 1;

                //Put the source bitmap components in pic
                if (sourceBmpData.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    picSource.use_argb = 1;
                    if (Natives.NativeMethods.WebPPictureImportBgra(ref picReference, referenceBmpData.Scan0,
                        referenceBmpData.Stride) != 1)
                        throw new Exception("Can´t allocate memory in WebPPictureImportBGR");
                }
                else
                {
                    picSource.use_argb = 0;
                    if (Natives.NativeMethods.WebPPictureImportBgr(ref picReference, referenceBmpData.Scan0,
                        referenceBmpData.Stride) != 1)
                        throw new Exception("Can´t allocate memory in WebPPictureImportBGR");
                }

                //Measure
                var ptrResult = pinnedResult.AddrOfPinnedObject();
                if (Natives.NativeMethods.WebPPictureDistortion(ref picSource, ref picReference, metricType,
                    ptrResult) != 1)
                    throw new Exception("Can´t measure.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.GetPictureDistortion");
            }
            finally
            {
                //Unlock the pixels
                if (sourceBmpData is not null)
                    source.UnlockBits(sourceBmpData);
                if (referenceBmpData is not null)
                    reference.UnlockBits(referenceBmpData);

                //Free memory
                if (picSource.argb != IntPtr.Zero)
                    Natives.NativeMethods.WebPPictureFree(ref picSource);
                if (picReference.argb != IntPtr.Zero)
                    Natives.NativeMethods.WebPPictureFree(ref picReference);
                
                //Free memory
                if (pinnedResult.IsAllocated)
                    pinnedResult.Free();
            }
        }
        
        #endregion

        #region | Private Methods |
        
        /// <summary>Encoding image  using Advanced encoding API</summary>
        /// <param name="bmp">Bitmap with the image</param>
        /// <param name="config">Config for encode</param>
        /// <param name="info">True if need encode info.</param>
        /// <returns>Compressed data</returns>
        private static byte[] AdvancedEncode(Bitmap bmp, WebPConfig config, bool info)
        {
            var pic = new WebPPicture();
            BitmapData bmpData = null;
            var ptrStats = IntPtr.Zero;
            var pinnedArrayHandle = new GCHandle();
            try
            {
                //Validate the config
                if (Natives.NativeMethods.WebPValidateConfig(ref config) != 1)
                    throw new Exception("Bad config parameters");

                //test bmp
                if (bmp.Width == 0 || bmp.Height == 0)
                    throw new ArgumentException(@"Bitmap contains no data.", nameof(bmp));
                if (bmp.Width > WebpMaxDimension || bmp.Height > WebpMaxDimension)
                    throw new NotSupportedException("Bitmap's dimension is too large. Max is " + WebpMaxDimension +
                                                    "x" + WebpMaxDimension + " pixels.");
                if (bmp.PixelFormat != PixelFormat.Format24bppRgb && bmp.PixelFormat != PixelFormat.Format32bppArgb)
                    throw new NotSupportedException("Only support Format24bppRgb and Format32bppArgb pixelFormat.");

                // Setup the input data, allocating a the bitmap, width and height
                bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                    bmp.PixelFormat);
                if (Natives.NativeMethods.WebPPictureInitInternal(ref pic) != 1)
                    throw new Exception("Can´t init WebPPictureInit");
                pic.width = bmp.Width;
                pic.height = bmp.Height;
                pic.use_argb = 1;

                if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    var result = Natives.NativeMethods.WebPPictureImportBgra(ref pic, bmpData.Scan0, bmpData.Stride);
                    if (result != 1)
                        throw new Exception("Can´t allocate memory in WebPPictureImportBGRA");
                    pic.colorspace = (uint) WebpCspMode.ModeBgrA;
                }
                else
                {
                    var result = Natives.NativeMethods.WebPPictureImportBgr(ref pic, bmpData.Scan0, bmpData.Stride);
                    if (result != 1)
                        throw new Exception("Can´t allocate memory in WebPPictureImportBGR");
                }

                //Set up statistics of compression
                if (info)
                {
                    var stats = new WebPAuxStats();
                    ptrStats = Marshal.AllocHGlobal(Marshal.SizeOf(stats));
                    Marshal.StructureToPtr(stats, ptrStats, false);
                    pic.stats = ptrStats;
                }

                //Memory for WebP output
                var dataWebp = new byte[bmp.Width * bmp.Height * 32];
                pinnedArrayHandle = GCHandle.Alloc(dataWebp, GCHandleType.Pinned);
                var initPtr = pinnedArrayHandle.AddrOfPinnedObject();
                pic.custom_ptr = initPtr;

                // Set up a byte-writing method (write-to-memory, in this case)
                Natives.NativeMethods.OnCallback = MyWriter;
                pic.writer = Marshal.GetFunctionPointerForDelegate(Natives.NativeMethods.OnCallback);

                //compress the input samples
                if (Natives.NativeMethods.WebPEncode(ref config, ref pic) != 1)
                    throw new Exception("Encoding error: " + (WebPEncodingError) pic.error_code);

                //Remove OnCallback
                Natives.NativeMethods.OnCallback = null;

                //Unlock the pixels
                bmp.UnlockBits(bmpData);
                bmpData = null;

                //Copy webpData to rawWebP
                var size = (int) ((long) pic.custom_ptr - (long) initPtr);
                var rawWebP = new byte[size];
                Array.Copy(dataWebp, rawWebP, size);

                //Remove compression data
                pinnedArrayHandle.Free();

                return rawWebP;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nIn WebP.AdvancedEncode");
            }
            finally
            {
                //Free temporal compress memory
                if (pinnedArrayHandle.IsAllocated)
                {
                    pinnedArrayHandle.Free();
                }

                //Free statistics memory
                if (ptrStats != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptrStats);
                }

                //Unlock the pixels
                if (bmpData != null)
                {
                    bmp.UnlockBits(bmpData);
                }

                //Free memory
                if (pic.argb != IntPtr.Zero)
                {
                    Natives.NativeMethods.WebPPictureFree(ref pic);
                }
            }
        }

        private static int MyWriter([In] IntPtr data, UIntPtr dataSize, ref WebPPicture picture)
        {
            Natives.NativeMethods.CopyMemory(picture.custom_ptr, data, (uint)dataSize);
            picture.custom_ptr = IntPtr.Add(picture.custom_ptr, (int)dataSize);
            
            return 1;
        }

        #endregion

        #region | Destruction |

        ~WebP()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        
        #endregion
    }
}