using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using HelloQQPortal.Database;
using System.Data;
using System.Data.Entity;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace HelloQQPortal.Manager
{  

    public class ImageManager : HQQBase
    {
        private HttpServerUtility server;
        //private NameValueCollection webConfig = WebConfigurationManager.AppSettings;
        
        public enum IMAGE_TYPE { PRODUCT, PRODUCT_MANUAL, FAQ };

        public class UploadResult
        {
            public string URL { get; set; }
            public string Location { get; set; }
        }

        public ImageManager() : base()
        {
            server = HttpContext.Current.Server;
        }

        protected bool ResizeAbort()
        {
            return false;
        }

        public bool UploadImage(HttpPostedFileBase imageUpload, string fullPath)
        {
            bool result = false;
            try
            {
                FileInfo fileInfo = new FileInfo(fullPath);                

                if (!fileInfo.Directory.Exists)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }

               imageUpload.SaveAs(fileInfo.FullName);

                result = true;
            }
            catch (Exception) { }
            return result;
        }

        public void CreateThumbnail(string sourceImagePath, int width)
        {
            FileInfo fileInfo = new FileInfo(sourceImagePath);
            string thumbnailPattern = "{0}\\thmb-{1}{2}";

            if (fileInfo.Exists)
            {
                System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(sourceImagePath);
                int X = sourceImage.Width;
                int Y = sourceImage.Height;
                int height = (int)((width * Y) / X);

                System.Drawing.Image.GetThumbnailImageAbort dummyCallback = new System.Drawing.Image.GetThumbnailImageAbort(ResizeAbort);
                System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(sourceImagePath);
                System.Drawing.Image thumbImg = fullSizeImg.GetThumbnailImage(width, height, dummyCallback, IntPtr.Zero);
                
                thumbImg.Save(string.Format(thumbnailPattern, 
                    fileInfo.DirectoryName, 
                    Path.GetFileNameWithoutExtension(sourceImagePath), 
                    fileInfo.Extension));
            }
        }

        public UploadResult UploadProductManualImage(HttpPostedFileBase imageUpload,int productId, int productManualId, IMAGE_TYPE imageType )
        {
            UploadResult result = new UploadResult();

            string strFileTypeURL = "{0}/{1}/{2}-{3}";
            string strFullPath = string.Empty;
            string fileURL = string.Format(this.PRODUCT_IMAGE_PATH_PATTERN,
                base.webConfig["image.product.location"], productId);

            string filePath = string.Empty;

            try
            {
                switch (imageType)
                {
                    case IMAGE_TYPE.PRODUCT_MANUAL:

                        strFileTypeURL = string.Format(strFileTypeURL, fileURL, 
                            imageType.ToString(), 
                            DateTime.Now.ToString("yyyyMMddHHmmss"),
                            imageUpload.FileName);

                        break;
                    case IMAGE_TYPE.FAQ:

                        break;
                }

                FileInfo fileInfo = new FileInfo(server.MapPath(strFileTypeURL));
                result.Location = strFileTypeURL;

                if (!fileInfo.Directory.Exists)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }

                if (this.UploadImage(imageUpload, server.MapPath(strFileTypeURL)))
                {
                    this.CreateThumbnail(server.MapPath(strFileTypeURL), 180);
                }

                result.URL = "~/"+strFileTypeURL;
            }
            catch (Exception) { }
            return result;
        }

        public byte[] GetHQQImage(int id, bool isThumbnail)
        {
            byte[] result = null;

            using (helloqqdbEntities dbInfo = new helloqqdbEntities())
            {
                hqq_images imgInfo = dbInfo.hqq_images.Find(id);
                if(imgInfo != null)
                {
                    if (isThumbnail) {

                        FileInfo fileInfo = new FileInfo(currServer.MapPath(imgInfo.path));
                        string filePath = string.Format("{0}/thmb-{1}",
                           imgInfo.path.Substring(0, imgInfo.path.LastIndexOf("/"))
                           , fileInfo.Name, fileInfo.Extension);

                        result = File.ReadAllBytes(currServer.MapPath(filePath));
                    }
                    else
                    {
                        result = File.ReadAllBytes(currServer.MapPath(imgInfo.path));
                    }
                }
            }

            return result;
        }

        public byte[] GetHQQProductImage(int id, int width, int height)
        {
            byte[] result = null;

            using (helloqqdbEntities dbInfo = new helloqqdbEntities())
            {
                hqq_product_images imgInfo = dbInfo.hqq_product_images.Find(id);
                if (imgInfo != null)
                {
                    if (width == 0 || height == 0)
                    {
                        FileInfo fileInfo = new FileInfo(currServer.MapPath(imgInfo.path));
                        string filePath = string.Format("{0}/thmb-{1}",
                           imgInfo.path.Substring(0, imgInfo.path.LastIndexOf("/"))
                           , fileInfo.Name, fileInfo.Extension);

                        result = File.ReadAllBytes(currServer.MapPath(filePath));
                    }
                    else
                    {
                        result = File.ReadAllBytes(currServer.MapPath(imgInfo.url));
                        result = CroppedImage(result, width, height);                        
                    }
                }
            }

            return result;
        }

        public bool DeleteImage(int id)
        {
            bool result = false;
            string fullPath = string.Empty;
            string thmbnail = string.Empty;
            string thumbnailPattern = "{0}/thmb-{1}{2}";

            try
            {
                using (helloqqdbEntities dbInfo = new helloqqdbEntities())
                {
                    hqq_images imgInfo = dbInfo.hqq_images.Find(id);
                    if (imgInfo != null)
                    {
                        fullPath = currServer.MapPath(imgInfo.location);
                        FileInfo fileInfo = new FileInfo(fullPath);
                        thmbnail = string.Format(thumbnailPattern, fileInfo.DirectoryName, fileInfo.Name, fileInfo.Extension);

                        File.Delete(thmbnail);
                        fileInfo.Delete();

                        dbInfo.Entry(imgInfo).State = EntityState.Deleted;
                        dbInfo.SaveChanges();

                    }
                }               
                result = true;
            }
            catch (Exception) { }
            return result;
        }

        public bool DeleteImage(string fullPath)
        {
            bool result = false;
            string thmbnail = string.Empty;
            string thumbnailPattern = "{0}/thmb-{1}{2}";

            try
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                thmbnail = string.Format(thumbnailPattern, fileInfo.DirectoryName, fileInfo.Name, fileInfo.Extension);

                File.Delete(thmbnail);
                fileInfo.Delete();

                result = true;
            }
            catch (Exception) { }
            return result;
        }

        public byte[] CroppedImage(byte[] byteImg, int maxWidth, int maxHeight)
        {
            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();

            Image image = Image.FromStream(new MemoryStream(byteImg));
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            }
            catch { }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(memoryStream, jpgInfo, encParams);
                  
                }
            }
            catch { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            return memoryStream.ToArray();
        }


    }
}