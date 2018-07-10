using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HelloQQPortal.Database;
using HelloQQPortal.Manager;

namespace HelloQQPortal.Controllers
{
    public class productController : Controller
    {
       
        // GET: Product
        public ActionResult Index()
        {
            //if(Session["memberInfo"] == null)
            //{
            //    Response.Redirect("/home/login");
            //}

            //Example::
            List<hqq_product> lstHqqProductInfo = new List<hqq_product>();
            ProductManager productMgr = new ProductManager();

            List<hqq_member_product> lstMemberProductInfo = productMgr.GetMemberProduct(2);

            return View(lstMemberProductInfo);
        }

       public ActionResult View(int id)
        {
            return View();
        }

        public ActionResult Manual(int id)
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}