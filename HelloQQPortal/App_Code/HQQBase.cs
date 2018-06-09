using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;
using System.Collections.Specialized;

namespace HelloQQPortal.Manager
{
    public class HQQBase
    {
        protected NameValueCollection webConfig = WebConfigurationManager.AppSettings;

        protected string PRODUCT_IMAGE_PATH_PATTERN { get { return "{0}/{1:D5}/"; } }
        protected string PRODUCT_IMAGE_FILE_PATTERN { get { return "{0}/{1}"; } }
        protected string PRODUCT_IMAGE_URL_PATTERN { get { return "~{0}/{1}"; } }

        protected HttpServerUtility currServer = HttpContext.Current.Server;

        public HQQBase()
        {

        }
    }
}