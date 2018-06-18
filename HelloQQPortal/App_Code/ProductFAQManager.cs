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
    public class ProductFaqManager
    {
        private helloqqdbEntities dbInfo = new helloqqdbEntities();

        public ProductFaqManager()
        {

        }

        public List<hqq_product_faq> GetProductFAQList()
        {
            List<hqq_product_faq> lstProductFAQ = dbInfo.hqq_product_faq
                                            .Include("hqq_product")
                                            .ToList();

            return lstProductFAQ;
        }

        public hqq_product_faq GetFaqById(int id)
        {
            hqq_product_faq productFAQ = dbInfo.hqq_product_faq
                .Where(item => item.id == id)
                .Include("hqq_product")
                .FirstOrDefault();

            return productFAQ;
        }

        public List<hqq_product_faq> GetFaqByProductId(int id)
        {
            List<hqq_product_faq> lstProductFAQ = dbInfo.hqq_product_faq.Where(item => item.hqq_product.id == id).ToList();
            return lstProductFAQ;
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

        public hqq_product_faq UpdateProductFAQDetail(hqq_product_faq productFaqInfo, int productId)
        {
            int order;

            using (dbInfo = new helloqqdbEntities())
            {
                if (productFaqInfo.id > 0)
                {
                    productFaqInfo.modified_on = DateTime.Now;
                    productFaqInfo.modified_by = 1;
                    dbInfo.Entry(productFaqInfo).State = EntityState.Modified;
                    //dbInfo.hqq_product.Attach(productInfo);
                }
                else
                {
                    productFaqInfo.product_id = productId;
                    productFaqInfo.created_by = 1;
                    productFaqInfo.created_on = DateTime.Now;

                    if(dbInfo.hqq_product_faq.Where(item => item.hqq_product.id == productId).Count() > 0)
                    {
                        order = dbInfo.hqq_product_faq.Where(item => item.hqq_product.id == productId).Max(item => item.order);
                        order++; 
                    }
                    else
                    {
                        order = 1;
                    }

                    productFaqInfo.order = order;
                    dbInfo.hqq_product_faq.Add(productFaqInfo);

                }

                dbInfo.SaveChanges();
            }

            return productFaqInfo;
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