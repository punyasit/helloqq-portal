using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Models;
using HelloQQPortal.Database;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Web.Configuration;
using System.Collections.Specialized;


namespace HelloQQPortal.Manager
{
    public class ProductManager : HQQBase
    {
        private helloqqdbEntities dbInfo;
        private HttpServerUtility server;

        private ImageManager imgMgr = new ImageManager();

        public ProductManager() : base()
        {
            server = System.Web.HttpContext.Current.Server;
        }

        public List<hqq_product> GetProductList()
        {
            List<hqq_product> lstResult = new List<hqq_product>();
            using (dbInfo = new helloqqdbEntities())
            {
                lstResult = dbInfo.hqq_product.ToList();
            }

            return lstResult;
        }

        public hqq_product GetProductById(int id)
        {
            hqq_product productInfo;

            using (dbInfo = new helloqqdbEntities())
            {
                productInfo = dbInfo.hqq_product
                    .Where(item => item.id == id)
                    .Include("hqq_product_images")
                    .FirstOrDefault();
            }

            return productInfo;
        }

        public hqq_product UpdateProductDetail(hqq_product productInfo)
        {
            using (dbInfo = new helloqqdbEntities())
            {
                if (productInfo.id > 0)
                {
                    productInfo.modified_on = DateTime.Now;
                    productInfo.modified_by = 1;
                    dbInfo.Entry(productInfo).State = EntityState.Modified;
                    //dbInfo.hqq_product.Attach(productInfo);
                }
                else
                {
                    dbInfo.hqq_product.Add(productInfo);
                }

                dbInfo.SaveChanges();
            }

            return productInfo;
        }

        public hqq_product_images AddProductImage(hqq_product productDetail, HttpPostedFileBase imageUpload)
        {
            hqq_product_images prdImgInfo;

            string fileURL = string.Format(this.PRODUCT_IMAGE_PATH_PATTERN, base.webConfig["image.product.location"],
                productDetail.id);
            string filePath = string.Empty;

            //if (!Directory.Exists(server.MapPath(filePath)))
            //{
            //    Directory.CreateDirectory(server.MapPath(filePath));
            //}

            //imageUpload.SaveAs(server.MapPath(
            //    string.Format(
            //        filePattern, filePath, imageUpload.FileName);));

            filePath = server.MapPath(string.Format(this.PRODUCT_IMAGE_FILE_PATTERN, fileURL, imageUpload.FileName));

            using (dbInfo = new helloqqdbEntities())
            {
                prdImgInfo = dbInfo.hqq_product_images.Where(
                    item => item.hqq_product.id == productDetail.id)
                    .FirstOrDefault();

                if (prdImgInfo != null)
                {
                    imgMgr.DeleteImage(prdImgInfo.path);
                    dbInfo.Entry(prdImgInfo).State = EntityState.Deleted;
                    dbInfo.SaveChanges();
                }

                if (imgMgr.UploadImage(imageUpload, filePath))
                {
                    imgMgr.CreateThumbnail(filePath, 180);
                }

                prdImgInfo = new hqq_product_images();
                prdImgInfo.product_id = productDetail.id;

                prdImgInfo.filename = imageUpload.FileName;
                prdImgInfo.file_type = imageUpload.ContentType;
                prdImgInfo.length = imageUpload.ContentLength;

                prdImgInfo.created_on = DateTime.Now;
                prdImgInfo.created_by = 1;

                prdImgInfo.path = filePath;
                prdImgInfo.url = string.Format(this.PRODUCT_IMAGE_URL_PATTERN, fileURL, imageUpload.FileName);

                dbInfo.hqq_product_images.Add(prdImgInfo);
                dbInfo.SaveChanges();
            }

            return prdImgInfo;
        }

        public List<hqq_member_product> GetMemberProduct(int id)
        {

            List<hqq_member_product> lstMemberProductInfo = new List<hqq_member_product>();
            using (dbInfo = new helloqqdbEntities())
            {

                //lstProductInfo = (from item in dbInfo.hqq_product
                //                  where item.hqq_member_product.All(item2 => item2.member_id.Equals(id))
                //                  select item).ToList();

                lstMemberProductInfo = dbInfo.hqq_member_product
                     .Include("hqq_product")
                     .Where(item => item.hqq_member.id == id)
                     .ToList();

            }


            return lstMemberProductInfo;
        }
    }
}