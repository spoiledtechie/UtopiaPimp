using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;

namespace Boomers.Utilities.Imaging
{
    public class ImageFunctions
    {
        /// <summary>
        /// Exports a Shrunken image.
        /// </summary>
        /// <param name="TargetH">Height for exported Image.</param>
        /// <param name="TargetW">Width for Exported Image.</param>
        /// <param name="Extension">Extension of Image.</param>
        /// <param name="FullSizeImg">The Image to export.</param>
        /// <remarks>Exports the image and outputs it to the user.</remarks>
        public static void ImageShrink(int TargetH, int TargetW, string Extension, System.Drawing.Image FullSizeImg)
        {
            System.Drawing.Image
                original = FullSizeImg;
            if (original.Height > original.Width)
                TargetW = Convert.ToInt32(original.Width * (Convert.ToDouble(TargetH) / (Convert.ToDouble(original.Height))));
            else
                TargetH = Convert.ToInt32(original.Height * (Convert.ToDouble(TargetW) / (Convert.ToDouble(original.Width))));

            System.Drawing.Image imgPhoto = FullSizeImg;
            Bitmap bmPhoto = new Bitmap(TargetW, TargetH, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(72F, 72F);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, TargetW, TargetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
            switch (Extension)
            {
                case ".gif": //checks for .gif
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".png":
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case ".tiff":
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
            }
            original.Dispose();
            imgPhoto.Dispose();
            grPhoto.Dispose();
            bmPhoto.Dispose();
            HttpContext.Current.Response.End();
        }
        public static Bitmap ImageShrink(int targetH, int targetW, System.Drawing.Image original, float resolution)
        {
            if (original.Height > original.Width)
                targetW = Convert.ToInt32(original.Width * (Convert.ToDouble(targetH) / (Convert.ToDouble(original.Height))));
            else
                targetH = Convert.ToInt32(original.Height * (Convert.ToDouble(targetW) / (Convert.ToDouble(original.Width))));

                        Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(resolution, resolution);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.DrawImage(original, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);

            return bmPhoto;
        }

        /// <summary>
        /// A better alternative to Image.GetThumbnail. Higher quality but slightly slower
        /// </summary>
        /// <param name="source"></param>
        /// <param name="thumbWi"></param>
        /// <param name="thumbHi"></param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(Bitmap source, int thumbWi, int thumbHi, bool maintainAspect)
        {
            // return the source image if it's smaller than the designated thumbnail
            if (source.Width < thumbWi && source.Height < thumbHi) return source;

            System.Drawing.Bitmap ret = null;
            try
            {
                int wi, hi;

                wi = thumbWi;
                hi = thumbHi;

                if (maintainAspect)
                {
                    // maintain the aspect ratio despite the thumbnail size parameters
                    if (source.Width > source.Height)
                    {
                        wi = thumbWi;
                        hi = (int)(source.Height * ((decimal)thumbWi / source.Width));
                    }
                    else
                    {
                        hi = thumbHi;
                        wi = (int)(source.Width * ((decimal)thumbHi / source.Height));
                    }
                }

                // original code that creates lousy thumbnails
                // System.Drawing.Image ret = source.GetThumbnailImage(wi,hi,null,IntPtr.Zero);
                ret = new Bitmap(wi, hi);
                using (Graphics g = Graphics.FromImage(ret))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, wi, hi);
                    g.DrawImage(source, 0, 0, wi, hi);
                }
            }
            catch
            {
                ret = null;
            }

            return ret;
        }
        /// <summary>
        /// Builds JPEG Encoder Parameters object
        /// </summary>
        /// <param name="compressionQuality">0 - 100 integer for jpeg compression quality (100 is the highest quality)</param>
        /// <returns></returns>
        public static System.Drawing.Imaging.EncoderParameters GetJPEGEncoderParams(int compressionQuality)
        {
            //Configure JPEG Compression Engine
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            long[] quality = new long[1];
            quality[0] = compressionQuality;
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    break;
                }
            }
            return encoderParams;
        }
        public static void ConvertImage(string fileNamePath, System.Drawing.Imaging.ImageFormat desiredFormat, string newFileNamePath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(fileNamePath);
            image.Save(newFileNamePath, desiredFormat);
        }
    }
}
