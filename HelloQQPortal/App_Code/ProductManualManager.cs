using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Models;
using HelloQQPortal.Database;
using System.Data;
using System.Data.Entity;

namespace HelloQQPortal.Manager
{
 
    public class ProductManualManager
    {
        private helloqqdbEntities dbInfo = new helloqqdbEntities();

        public ProductManualManager()
        {

        }

        public List<hqq_product_manual> GetProductManualList()
        {
            List<hqq_product_manual> lstProductManual = dbInfo.hqq_product_manual
                                            .Include("hqq_product")
                                            .ToList();

            return lstProductManual;
        }

        public hqq_product_manual GetProdutManualById(int id)
        {
            hqq_product_manual hqqProductManual = dbInfo.hqq_product_manual.Find(id);
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

        public hqq_product_manual UpdateProductManualDetail(hqq_product_manual productManual, int productId)
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
    }
}
