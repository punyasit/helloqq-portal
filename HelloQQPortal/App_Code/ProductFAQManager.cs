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
    public class ProductFAQManager
    {
        private helloqqdbEntities dbInfo = new helloqqdbEntities();

        public ProductFAQManager()
        {

        }

        public List<hqq_product_faq> GetProductFAQList()
        {
            List<hqq_product_faq> lstProductFAQ = dbInfo.hqq_product_faq
                                            .Include("hqq_product")
                                            .ToList();

            return lstProductFAQ;
        }

        public hqq_product_faq GetProdutManualById(int id)
        {
            hqq_product_faq productFAQ = dbInfo.hqq_product_faq.Find(id);
            return productFAQ;
        }

        public hqq_product_faq UpdateProductFAQlDetail(hqq_product_faq productFAQ)
        {
            using (dbInfo = new helloqqdbEntities())
            {
                if (productFAQ.id > 0)
                {
                    productFAQ.modified_on = DateTime.Now;
                    productFAQ.modified_by = 1;
                    dbInfo.Entry(productFAQ).State = EntityState.Modified;
                    //dbInfo.hqq_product.Attach(productInfo);
                }
                else
                {
                    dbInfo.hqq_product_faq.Add(productFAQ);
                }

                dbInfo.SaveChanges();
            }

            return productFAQ;
        }

        public hqq_product_faq UpdateProductFAQDetail(hqq_product_faq productManual, int productId)
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
                    productManual.product_id = productId;
                    dbInfo.hqq_product_faq.Add(productManual);
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

                hqq_product_faq selectedItem = dbInfo.hqq_product_faq.Find(id);
                if (selectedItem != null)
                {
                    dbInfo.Entry(selectedItem).State = EntityState.Deleted;
                    dbInfo.SaveChanges();

                    result = true;
                }
            }
            return result;
        }

    }
}