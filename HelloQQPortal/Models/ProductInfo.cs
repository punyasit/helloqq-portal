using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Database;

namespace HelloQQPortal.Models
{
    public class ProductInfo
    {
        public hqq_product ProductDetail { get; set; }
        public hqq_product_images ProductImage { get; set; }
        public string ProductImageThumbURL { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }
    }


}