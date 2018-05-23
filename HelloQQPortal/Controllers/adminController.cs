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
        private MemberManager memberMgr = new MemberManager();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Message = "Administrator.";
            ViewBag.Message = "Your application description page.";
            return View();
        }

        // GET: Admin
        public ActionResult ProductList(List<hqq_product> lstProductInfo)
        {
            return View();
        }

        // GET: Admin
        public ActionResult ProductDetail(hqq_product productInfo)
        {
            if (productInfo.id == 0)
            {
               
            }
            else
            {

            }
            return View();
        }

        public ActionResult UserList()
        {
            return View(memberMgr.GetMemberList());
        }

        // GET: Admin
        public ActionResult UserDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            hqq_member member = memberMgr.GetMemberById(id.Value);
         
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }


        [HttpPost]
        public ActionResult UpdateUserDetail(hqq_member memberInfo)
        {
            try
            {
                memberMgr.UpdateMember(memberInfo);
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
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