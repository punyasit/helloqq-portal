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
        private member memberInfo;

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

        public ActionResult Register(member member)
        {

            if (ModelState.IsValid && this.Request.RequestType == "POST")
            {
                if (dbInfo.members.Any(item => item.facebook_id == member.facebook_id))
                {
                    // Modify Record
                    //return RedirectToAction("Index");
                }
                else
                {
                    //#Add Record
                    member = AddNewEntity(member);
                }

                return RedirectToAction("Index");
            }
            return View(member);

        }

        private member AddNewEntity(member member)
        {
            meta_location metaLocation = null;
            member = this.AddProperty(member);
            // Find Location Code
            metaLocation = metadataMgr.GetMetaLocationInfo(member.location_code.Replace(", Thailand", ""));
            if (metaLocation != null && metaLocation.id > 0)
            {
                member.location_code = metaLocation.ISO_code;
            }
            // Find HomeTown Code 
            metaLocation = metadataMgr.GetMetaLocationInfo(member.hometown_code.Replace(", Thailand", ""));
            if (metaLocation != null && metaLocation.id > 0)
            {
                member.hometown_code = metaLocation.ISO_code;
            }

            dbInfo.members.Add(member);
            dbInfo.SaveChanges();
            return member;
        }

        private member AddProperty(member member)
        {
            member.created_on = DateTime.Now;
            member.created_by = 1;

            return member;
        }

        public ActionResult editMember(member member)
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
                        member = (member)Session["memberInfo"];
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