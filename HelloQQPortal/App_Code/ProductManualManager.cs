using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using HelloQQPortal.Models;
using HelloQQPortal.Database;
using System.Data;
using System.Data.Entity;

namespace HelloQQPortal.Manager
{

    public class ProductManualManager : HQQBase
    {
        private helloqqdbEntities dbInfo = new helloqqdbEntities();
        private ProductManager productManager = new ProductManager();
        private ImageManager imgMgr = new ImageManager();

        public ProductManualManager() : base()
        {

        }

        public List<int> CheckAvailableManual(List<int> lstProductId)
        {
            List<int> lstAvailableManual = new List<int>();

            lstAvailableManual = dbInfo.hqq_product_manual.Where(item => 
            lstProductId.Contains(item.product_id)).Select(item => item.product_id).ToList();

            return lstAvailableManual;
        }

        public List<hqq_product_manual> GetProductManualList()
        {
            List<hqq_product_manual> lstProductManual = dbInfo.hqq_product_manual
                                            .Include("hqq_product")
                                            .ToList();

            return lstProductManual;
        }

        public hqq_product_manual GetProductManualById(int id)
        {
            hqq_product_manual hqqProductManual = dbInfo.hqq_product_manual.Find(id);
            return hqqProductManual;
        }

        public hqq_product_manual GetProductManualByProductId(int id)
        {
            hqq_product_manual hqqProductManual = dbInfo.hqq_product_manual
                .Where(item => item.product_id == id).FirstOrDefault();
            return hqqProductManual;
        }

        public hqq_product_manual UpdateProductManualDetail(hqq_product_manual productManual)
        {
            using (dbInfo = new helloqqdbEntities())
            {
                if (productManual.id > 0)
                {
                    productManual.modified_on = DateTime.Now;
                    productManual.modified_by = 1;
                    dbInfo.Entry(productManual).State = EntityState.Modified;
                    //dbInfo.hqq_product.Attach(productInfo);
                }
                else
                {
                    dbInfo.hqq_product_manual.Add(productManual);
                }

                dbInfo.SaveChanges();
            }

            return productManual;
        }

        public bool DeleteMemberProduct(int id)
        {
            bool result = false;
            using (dbInfo = new helloqqdbEntities())
            {
                hqq_product_manual selectedItem = dbInfo.hqq_product_manual.Find(id);
                if (selectedItem != null)
                {
                    dbInfo.Entry(selectedItem).State = EntityState.Deleted;
                    dbInfo.SaveChanges();

                    result = true;
                }
            }
            return result;
        }

        public hqq_product_manual UpdateProductManualDetail(hqq_product_manual productManual, int productId)
        {
            using (dbInfo = new helloqqdbEntities())
            {
                if (productManual.id > 0)
                {
                    productManual.modified_on = DateTime.Now;
                    productManual.modified_by = 1;
                    dbInfo.Entry(productManual).State = EntityState.Modified;
                    dbInfo.hqq_product_manual.Attach(productManual);
                }
                else
                {
                    productManual.product_id = productId;
                    dbInfo.hqq_product_manual.Add(productManual);
                }

                dbInfo.SaveChanges();
            }

            return productManual;
        }

        public string AddProductManualImage(HttpPostedFileBase[] imageUploads, int productManualId, int productId)
        {
            string strResult = string.Empty;

            if (imageUploads.Count() > 0)
            {
                return strResult;
            }

            hqq_images hqqImageInfo = new hqq_images();
            FileInfo fileInfo;
            ImageManager.UploadResult uploadResult = new ImageManager.UploadResult();

            string strImageURL = string.Empty;

            using (dbInfo = new helloqqdbEntities())
            {
                foreach (var imageUpload in imageUploads)
                {
                    uploadResult = imgMgr.UploadProductManualImage(imageUpload, productId,
                        productManualId, ImageManager.IMAGE_TYPE.PRODUCT_MANUAL);
                    hqqImageInfo = new hqq_images();

                    fileInfo = new FileInfo(base.currServer.MapPath(strImageURL));

                    hqqImageInfo.file_type = ImageManager.IMAGE_TYPE.PRODUCT_MANUAL.ToString();
                    hqqImageInfo.filename = fileInfo.Name + "." + fileInfo.Extension;
                    hqqImageInfo.path = uploadResult.URL;
                    hqqImageInfo.location = uploadResult.Location;
                    hqqImageInfo.entity_id = productManualId;
                    hqqImageInfo.created_on = DateTime.Now;
                    hqqImageInfo.created_by = 1;
                    hqqImageInfo.status = 1;

                    dbInfo.hqq_images.Add(hqqImageInfo);
                    dbInfo.SaveChanges();
                }
            }

            return strResult;
        }

        public List<hqq_images> GetManualImagesList(int productManualId)
        {
            List<hqq_images> lstImageInfo = new List<hqq_images>();
            string strFileType = ImageManager.IMAGE_TYPE.PRODUCT_MANUAL.ToString();

            using (dbInfo = new helloqqdbEntities())
            {
                lstImageInfo = dbInfo.hqq_images
                    .Where(item => item.entity_id == productManualId
                            && item.file_type == strFileType)
                            .ToList();
            }

            return lstImageInfo;
        }

        public bool RemoveProductManualImage(int productImageManualId)
        {
            bool blResult = true;

            return blResult;
        }
    }
}
