using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Models;
using HelloQQPortal.Database;
using System.Data;
using System.Data.Entity;


namespace HelloQQPortal.App_Code
{
    public class ProductManager
    {
        private helloqqdbEntities dbInfo;

        public ProductManager()
        {
           
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
                productInfo = dbInfo.hqq_product.Find(id);
            }

            return productInfo;
        }

       
        public hqq_product UpdateMember(hqq_product productInfo)
        {
            using (dbInfo = new helloqqdbEntities())
            {
                if(productInfo.id > 0)
                {
                    productInfo.created_on = DateTime.Now;
                    dbInfo.hqq_product.Attach(productInfo);
                }

                dbInfo.SaveChanges();
                
            }

            return productInfo;
        }
    }
}