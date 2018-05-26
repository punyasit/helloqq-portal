using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Web.Configuration;
using System.Collections.Specialized;


namespace HelloQQPortal.Manager
{
   

    public class ImageManager
    {

        private HttpServerUtility server;
        private NameValueCollection webConfig = WebConfigurationManager.AppSettings;

        public ImageManager()
        {

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

        public bool DeleteImage(string fullPath)
        {
            bool result = false;
            string thmbnail = string.Empty;
            string thumbnailPattern = "{0}/thmb-{0}{1}";

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