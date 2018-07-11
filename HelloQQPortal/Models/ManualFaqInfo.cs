using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Database;

namespace HelloQQPortal.Models
{
    public class ManualFaqInfo
    {
        public hqq_product_manual ManualInfo { get; set; }
        public List<hqq_product_faq> FaqInfoList { get; set; }
    }
}