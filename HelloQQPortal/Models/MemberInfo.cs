using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Database;

namespace HelloQQPortal.Models
{
    public class MemberInfo
    {
        public hqq_member MemberDetail { get; set; }
        public int SelectedProductId { get; set; }
        public List<hqq_member_product> MemberProduct { get; set; }
        public List<hqq_product> ProductList { get; set; }
        public string PurchasedOn { get; set; }

    }
}