using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloQQPortal.Database;

namespace HelloQQPortal.Models
{
    public class ProductManualInfo
    {
        public hqq_product_manual ProductManual { get; set; }
        public List<hqq_images> ProductManualImage { get; set; }
        public List<hqq_product> ProductList { get; set; }

        [AllowHtml]
        public string ProductManualContent { get; set; }
        public int SelectedProductId { get; set; }
        public HttpPostedFileBase[] ImageUpload { get; set; }


    }
}