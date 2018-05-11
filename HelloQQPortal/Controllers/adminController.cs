using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloQQPortal.Controllers
{
    public class adminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Message = "Administrator.";
            ViewBag.Message = "Your application description page.";
            return View();
        }

        // GET: Admin
        public ActionResult ProductList()
        {
            return View();
        }

        // GET: Admin
        public ActionResult ProductDetail()
        {
            return View();
        }

        public ActionResult UserList()
        {
            return View();
        }

        // GET: Admin
        public ActionResult UserDetail()
        {
            return View();
        }

        public ActionResult ManualList()
        {
            return View();
        }

        // GET: Admin
        public ActionResult ManualDetail()
        {
            return View();
        }

        public ActionResult FaqList()
        {
            return View();
        }

        // GET: Admin
        public ActionResult FaqDetail()
        {
            return View();
        }
    }
}