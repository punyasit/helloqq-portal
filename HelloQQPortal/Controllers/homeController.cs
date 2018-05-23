using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloQQPortal.Models;

using HelloQQPortal.Database;
using HelloQQPortal.Manager;

namespace HelloQQPortal.Controllers
{
    public class HomeController : Controller
    {
        private MemberManager memberMgr;
        private hqq_member memberInfo;

        private MetaDataManager metadataMgr = new MetaDataManager();
        private helloqqdbEntities dbInfo = new helloqqdbEntities();

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(UserInfo userInfo)
        {
            memberMgr = new MemberManager();

            if (userInfo != null)
            {
                memberInfo = memberMgr.Login(userInfo);
                Session["memberInfo"] = memberInfo;
                return Json(new { redirectToUrl = Url.Action("Index", "Product") });
            }
            else
            {
                return View();
            }
        }

        public ActionResult Register(hqq_member memberInfo)
        {

            if (ModelState.IsValid && this.Request.RequestType == "POST")
            {
                if (dbInfo.hqq_member.Any(item => item.facebook_id == memberInfo.facebook_id))
                {
                  // return RedirectToAction("editMember");
                }
                else
                {
                    //#Add Record
                    memberInfo = AddNewEntity(memberInfo);
                }

                return RedirectToAction("Index");
            }
            return View(memberInfo);

        }

        private hqq_member AddNewEntity(hqq_member memeberInfo)
        {
            hqq_meta_location metaLocation = null;
            memeberInfo = this.AddProperty(memeberInfo);
            // Find Location Code
            metaLocation = metadataMgr.GetMetaLocationInfo(memeberInfo.location_code.Replace(", Thailand", ""));
            if (metaLocation != null && metaLocation.id > 0)
            {
                memeberInfo.location_code = metaLocation.ISO_code;
            }
            // Find HomeTown Code 
            metaLocation = metadataMgr.GetMetaLocationInfo(memeberInfo.hometown_code.Replace(", Thailand", ""));
            if (metaLocation != null && metaLocation.id > 0)
            {
                memeberInfo.hometown_code = metaLocation.ISO_code;
            }

            dbInfo.hqq_member.Add(memeberInfo);
            dbInfo.SaveChanges();
            return memeberInfo;
        }

        private hqq_member AddProperty(hqq_member memberInfo)
        {
            memberInfo.created_on = DateTime.Now;
            memberInfo.created_by = 1;
            memberInfo.role = 1;

            return memberInfo;
        }

        public ActionResult editMember(hqq_member member)
        {
            if (Session["memberInfo"] == null)
            {
                Response.Redirect("/home/login");
            }
            else
            {
                if (ModelState.IsValid && this.Request.RequestType == "POST")
                {

                }
                else
                {
                    if (Session["memberInfo"] != null)
                    {
                        member = (hqq_member)Session["memberInfo"];
                    }
                }
            }


            return View(member);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}