using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloQQPortal.Models
{
    public class UserInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string photoURL { get; set; }
        public string token { get; set; }
        public string location { get; set; }
        public string hometown { get; set; }

    }
}