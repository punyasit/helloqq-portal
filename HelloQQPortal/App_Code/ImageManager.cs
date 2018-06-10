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

       
    }
}