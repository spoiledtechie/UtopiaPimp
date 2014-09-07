using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using System.Xml;
using System.Xml.Linq;

namespace ImageHandlers
{
    /// <summary>
    /// Summary description for ImageResize
    /// </summary>
    public class ImageResize : IHttpHandler
    {
        enum imageType { JPG, PNG, GIF }
        string resizedImagesDirectory = ConfigurationManager.AppSettings["ResizedImagesDirectory"];
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(System.Web.HttpContext ctx)
        {
            HttpRequest req = ctx.Request;
            int width = 0;
            int height = 0;
            string key = string.Empty;

            Int32.TryParse(req.QueryString["w"], out width);
            Int32.TryParse(req.QueryString["h"], out height);
            if (req.QueryString["k"] != null)
                key = req.QueryString["k"];

            if (key.Length > 0)
            {
                ResizedImage keyImage = new ResizedImage();
                keyImage = GetImageDimensions(ctx, key);
                height = keyImage.Height;
                width = keyImage.Width;
            }
            bool displayResizedImage = true;
            if (width == 0 & height == 0)
                displayResizedImage = false;

            string physicalPath = Regex.Replace(req.PhysicalPath, @"\.ashx.*", "");
            imageType imgType = imageType.PNG;
            if (physicalPath.EndsWith(".jpg") || physicalPath.EndsWith(".jpeg"))
            {
                ctx.Response.ContentType = "image/jpeg";
                imgType = imageType.JPG;
            }
            else if (physicalPath.EndsWith(".gif"))
            {
                ctx.Response.ContentType = "image/gif";
                imgType = imageType.GIF;
            }
            else if (physicalPath.EndsWith(".png"))
            {
                ctx.Response.ContentType = "image/png";
                imgType = imageType.PNG;
            }

            //            'Name the images based on their width, height, and path to ensure they're unique.
            //            'The image name starts out looking like /HttpModule/images/turtle.jpg (the virtual path), and gets
            //            'converted to 400_200_images_turtle.jpg.  The 400 is the width, and the 200 is the height.
            //            'If a width or height is not specified, it will look like 0_200_images_turtle.jpg (an example
            //            'where the width is not specified).           
            string virtualPath = Regex.Replace(req.Path, @"\.ashx.*", "");
            string resizedImageName = virtualPath.Remove(0, virtualPath.LastIndexOf("/") + 1);
            resizedImageName = Regex.Replace(resizedImageName, ".*?", "");
            resizedImageName = width + "_" + height + "_" + resizedImageName;

            //            Dim ri As New ResizedImage
            //            ri = GetResizedImage(Ctx, ResizedImageName, Height, Width, ImgType)

            ResizedImage ri = new ResizedImage();
            ri = GetResizedImage(ctx, resizedImageName, height, width, imgType);

                           if (displayResizedImage) //display resized image
                {
                    ctx.Response.WriteFile(Path.Combine(ri.ImagePath, ri.ImageName));
                }
                else
                {
                    //display original image
                    ctx.Response.WriteFile(physicalPath);
                }
          
        }

        private ResizedImage GetResizedImage(System.Web.HttpContext ctx, string imageName, int height, int width, imageType imgType)
        {
            //Look in the cache first for a list of images that have been resized
            List<ResizedImage> resizedImageList = new List<ResizedImage>();
            resizedImageList = (List<ResizedImage>)ctx.Cache.Get("ResizedImageList");

            ResizedImage ResizedImage = new ResizedImage();
            bool ImageFound = false;

            if (resizedImageList == null)
            {
                //Nothing in the cache, start a new image list
                resizedImageList = new List<ResizedImage>();
            }
            else
            {
                //Let's see if an image with this name and size is already created
                foreach (ResizedImage ri in resizedImageList)
                {
                    if (ri.ImageName == imageName && ri.Height == height && ri.Width == width)
                    {
                        //The image already exists, no need to create it.
                        ResizedImage = ri;
                        ImageFound = true;
                        break;
                    }
                }
            }

            //Create the folder where we want to save the resized images if it's not already there
            string ResizedImagePath = ctx.Server.MapPath(resizedImagesDirectory);
            if (!(Directory.Exists(ResizedImagePath)))
                Directory.CreateDirectory(ResizedImagePath);

            //Clear the cache anytime the resized image folder changes (in case items were removed from it)
            CacheDependency cd = new CacheDependency(ResizedImagePath);

            if (!ImageFound)
            {
                //We didn't find the image in the list of resized images...look in the resized folder
                //and see if it's there
                string imageFullPath = Path.Combine(ctx.Server.MapPath(resizedImagesDirectory), imageName);
                if (File.Exists(imageFullPath))
                {
                    //The image has already been created, set the properties for the image
                    //and add it to the cached image list
                    ResizedImage.ImageName = imageName;
                    ResizedImage.ImagePath = ctx.Server.MapPath(resizedImagesDirectory);
                    ResizedImage.Height = height;
                    ResizedImage.Width = width;
                    resizedImageList.Add(ResizedImage);

                    //Keep the cache for a day, unless new images get added to or deleted from
                    //the resized image folder
                    TimeSpan ts = new TimeSpan(48, 0, 0);
                    ctx.Cache.Add("ResizedImageList", resizedImageList, cd, Cache.NoAbsoluteExpiration, ts, CacheItemPriority.Default, null);
                }
            }

            HttpRequest Req = ctx.Request;
            string physicalPath = Regex.Replace(Req.PhysicalPath, "\\.ashx.*", "");
            if (ResizedImage.ImageName == "")
            {
                //The image isn't already created, we need to create it add it to the cache
                ResizeImage(physicalPath, ResizedImagePath, imageName, width, height, imgType);

                //Now update the cache since we've added a new resized image 
                ResizedImage.Width = width;
                ResizedImage.Height = height;
                ResizedImage.ImageName = imageName;
                ResizedImage.ImagePath = ResizedImagePath;
                resizedImageList.Add(ResizedImage);

                TimeSpan ts = new TimeSpan(48, 0, 0);
                ctx.Cache.Add("ResizedImageList", resizedImageList, cd, Cache.NoAbsoluteExpiration, ts, CacheItemPriority.Default, null);
            }

            return ResizedImage;
        }
        private void ResizeImage(string ImagePath, string ResizedSavePath, string ResizedImageName, int NewWidth, int NewHeight, imageType ImgType)
        {
            //Make sure the image exists before trying to resize it
            if (File.Exists(ImagePath) & !(NewHeight == 0 && NewWidth == 0))
            {
                using (Bitmap OriginalImage = new Bitmap(ImagePath))
                {

                    if (NewHeight > OriginalImage.Height)
                    {
                        NewHeight = OriginalImage.Height;
                        NewWidth = Convert.ToInt32(OriginalImage.Width * (Convert.ToDouble(NewHeight) / (Convert.ToDouble(OriginalImage.Height))));
                    }
                    else if (NewWidth > OriginalImage.Width)
                    {
                        NewWidth = OriginalImage.Width;
                        NewHeight = Convert.ToInt32(OriginalImage.Height * (Convert.ToDouble(NewWidth) / (Convert.ToDouble(OriginalImage.Width))));
                    }
                    else if (OriginalImage.Height > OriginalImage.Width)
                        NewWidth = Convert.ToInt32(OriginalImage.Width * (Convert.ToDouble(NewHeight) / (Convert.ToDouble(OriginalImage.Height))));
                    else
                        NewHeight = Convert.ToInt32(OriginalImage.Height * (Convert.ToDouble(NewWidth) / (Convert.ToDouble(OriginalImage.Width))));

                    //if (NewWidth > 0 && NewHeight == 0 && OriginalImage.Width > NewWidth)
                    //    //The user only set the width, calculate the new height
                    //    NewHeight = Convert.ToInt32(Math.Floor(Convert.ToDouble(OriginalImage.Height) / (OriginalImage.Width / NewWidth)));

                    //if (NewHeight > 0 && NewWidth == 0)
                    //    //The user only set the height, calculate the width
                    //    NewWidth = Convert.ToInt32(Math.Floor(OriginalImage.Width / (OriginalImage.Height / (double)NewHeight)));

                    //if (NewHeight > OriginalImage.Height | NewWidth > OriginalImage.Width)
                    //{
                    //    //Keep the original height and width to avoid losing image quality
                    //    NewHeight = OriginalImage.Height;
                    //    NewWidth = OriginalImage.Width;
                    //}


                    using (Bitmap ResizedImage = new Bitmap(OriginalImage, NewWidth, NewHeight))
                    {
                        ResizedImage.SetResolution(72F, 72F);

                        Graphics newGraphic = Graphics.FromImage(ResizedImage);
                        newGraphic.Clear(Color.White);
                        newGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                        newGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        newGraphic.DrawImage(OriginalImage, 0, 0, NewWidth, NewHeight);

                        //Save the image as the appropriate type
                        //INSTANT C# NOTE: The following VB 'Select Case' included range-type or non-constant 'Case' expressions and was converted to C# 'if-else' logic:
                        //						Select Case ImgType
                        //ORIGINAL LINE: Case ImageType.GIF
                        if (ImgType == imageType.GIF)
                        {
                            ResizedImage.Save(System.IO.Path.Combine(ResizedSavePath, ResizedImageName), System.Drawing.Imaging.ImageFormat.Gif);
                        }
                        //ORIGINAL LINE: Case ImageType.JPG
                        else if (ImgType == imageType.JPG)
                        {
                            ResizedImage.Save(System.IO.Path.Combine(ResizedSavePath, ResizedImageName), System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        //ORIGINAL LINE: Case ImageType.PNG
                        else if (ImgType == imageType.PNG)
                        {
                            ResizedImage.Save(System.IO.Path.Combine(ResizedSavePath, ResizedImageName), System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
            }
        }

        private ResizedImage GetImageDimensions(HttpContext Ctx, string Key)
        {

            ResizedImage ri = new ResizedImage();

            //If I enter the key "thumbnail", this function will go to the xml file to find out
            //what the width and height of a thumbnail should be.

            //The xml that we're reading from looks like this.  You can set the width, the height, or both
            //<ResizedImages>
            //  <image name="thumbnail" width="100" height="100" />
            //  <image name="normal" width="200" />  
            //  <image name="large" height="300" />  
            //</ResizedImages>

            //Load the xml file
            XElement XMLSource = GetResizedImageKeys(Ctx);

            //Get all nodes where the name equals the key
            //To make this code work in .Net 2.0, use an xpath query to get the height
            //and width values instead of a LINQ query
            var ResizedQuery = from r in XMLSource.Elements("image")
                               where r.Attribute("name").ToString() == Key
                               select r;

            //Set the resized image we're returning with the width and height
            foreach (XElement r in ResizedQuery)
            {
                if (r.Attribute("height") != null)
                    ri.Height = Convert.ToInt32(r.Attribute("height"));

                if (r.Attribute("width") != null)
                    ri.Width = Convert.ToInt32(r.Attribute("width"));
            }

            return ri;
        }
        private XElement GetResizedImageKeys(HttpContext ctx)
        {
            XElement xel = null;
            string ResizedImageKeys = ctx.Server.MapPath(ConfigurationManager.AppSettings["ResizedImageKeys"]);
            if (ResizedImageKeys != null)
            {
                //Try to get the xml from the cache first
                xel = (XElement)ctx.Cache.Get("ResizedImageKeys");

                //If it's not there, load the xml document and then add it to the cache
                if ((xel == null))
                {
                    xel = XElement.Load(ResizedImageKeys);
                    CacheDependency cd = new CacheDependency(ResizedImageKeys);
                    TimeSpan ts = new TimeSpan(48, 0, 0);
                    ctx.Cache.Add("ResizedImageKeys", xel, cd, Cache.NoAbsoluteExpiration, ts, CacheItemPriority.Default, null);
                }
            }
            return xel;
        }
        //This class is used to keep track of which images are resized.  We save this in a cached list and look here first,
        //so we don't have to look through the folder on the file system every time we want to see if the resized image
        //exists or not
        private class ResizedImage
        {
            private string _ImageName;
            private string _ImagePath;
            private int _Width;
            private int _Height;
            public string ImageName
            {
                get
                {
                    return _ImageName;
                }
                set
                {
                    _ImageName = value;
                }
            }
            public string ImagePath
            {
                get
                {
                    return _ImagePath;
                }
                set
                {
                    _ImagePath = value;
                }
            }
            public int Width
            {
                get
                {
                    return _Width;
                }
                set
                {
                    _Width = value;
                }
            }
            public int Height
            {
                get
                {
                    return _Height;
                }
                set
                {
                    _Height = value;
                }
            }
            public ResizedImage()
            {
                Width = 0;
                Height = 0;
                ImagePath = "";
                ImageName = "";
            }
        }
    }
}


