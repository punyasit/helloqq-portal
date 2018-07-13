using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;
using System.Collections.Specialized;

using HelloQQPortal.Database;

namespace HelloQQPortal.Manager
{
    public class UIManager 
    {
        private string strUIXMLPath = string.Empty;
        private string strPageLogin = "/home/login";
        private hqq_member memberInfo;
        private NameValueCollection webConfig = WebConfigurationManager.AppSettings;

        public UIManager() : base()
        {
            strUIXMLPath = webConfig["page.configuration"];
            strPageLogin = webConfig["page.login"];
        }

        public void ValidatePermission(string strController)
        {
            bool isValidate = false;
            int role = 1;
            try
            {
                if (HttpContext.Current.Session["memberInfo"] != null)
                {
                    memberInfo = (hqq_member)HttpContext.Current.Session["memberInfo"];

                    if (memberInfo != null && memberInfo.role > 0)
                    {
                        role = memberInfo.role;
                    }                 

                    DataSet dsInfo = new DataSet();
                    DataTable dtInfo = new DataTable();
                    DataRow drInfo = null;
                    dsInfo.ReadXml(strUIXMLPath);

                    dtInfo = dsInfo.Tables["UIConfig"];
                    drInfo = dtInfo.Select(string.Format("Controller = '{0}'", "Adminstrator")).FirstOrDefault();

                    isValidate = role >= int.Parse(drInfo["permission"].ToString());
                }               

            }
            catch (Exception)
            {
                isValidate = false;
            }

            if (!isValidate)
            {
                HttpContext.Current.Response.Redirect(strPageLogin);
            }

        }
    }
}