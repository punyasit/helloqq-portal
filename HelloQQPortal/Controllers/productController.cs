using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloQQPortal.Controllers
{
    public class productController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            if(Session["userInfo"] == null)
            {
                Response.Redirect("/home/login");
            }
            return View();
        }

        public ActionResult Manual()
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