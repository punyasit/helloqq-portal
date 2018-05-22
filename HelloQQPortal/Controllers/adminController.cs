using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using HelloQQPortal.Database;
using HelloQQPortal.Manager;

namespace HelloQQPortal.Controllers
{
    public class adminController : Controller
    {
        private helloqqdbEntities dbInfo = new helloqqdbEntities();
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
            List<member> lstMember = dbInfo.members.ToList();
            return View(dbInfo.members.ToList());
        }

        // GET: Admin
        public ActionResult UserDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = dbInfo.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
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