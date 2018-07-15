using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Database;

namespace HelloQQPortal.Models
{
    public class MemberProductInfo
    {
        public int ProductId { get; set; }
        public string ProductName{ get; set; }
        public string ProductDesc { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? GuaranteeExpire { get; set; }
        public bool HaveManual { get; set; }
        public List<int> ProductImagesId { get; set; }
    }

}