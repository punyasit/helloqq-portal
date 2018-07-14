using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Database;

namespace HelloQQPortal.Models
{
    public class MemberProductInfo
    {
        public List<hqq_member_product> MemberProducts { get; set; }
        public List<int> LstManualProductId { get; set; }
    }
}