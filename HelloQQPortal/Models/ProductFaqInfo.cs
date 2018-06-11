using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloQQPortal.Database;

namespace HelloQQPortal.Models
{
    public class ProductFaqInfo
    {
        public hqq_product_faq ProductFaq{ get; set; }
        public List<hqq_product_faq> ProductFaqList { get; set; }
        public List<hqq_product> ProductList { get; set; }
        public hqq_product ProductInfo { get; set; }
        public int SelectedProductId { get; set; }
    }
}