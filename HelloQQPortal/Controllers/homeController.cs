using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloQQPortal.Models;

using HelloQQPortal.Database;
using HelloQQPortal.Manager;
using System.Data.Entity;

namespace HelloQQPortal.Controllers
{
    public class HomeController : Controller
    {
        private MemberManager memberMgr;
        private hqq_member memberInfo;

        private MetaDataManager metadataMgr = new MetaDataManager();
        private helloqqdbEntities dbInfo = new helloqqdbEntities();
        private ImageManager imgMgr = new ImageManager();

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

        [HttpPost]
        public ActionResult ValidateUserExist(UserInfo userInfo)
        {
            string result = string.Empty;

            if (dbInfo.hqq_member.Any(item => item.facebook_id == userInfo.id))
            {
                result = "home/editMember";
            }

            return Json(new { redirectToUrl = result });
        }

        public ActionResult Register(hqq_member memberInfo)
        {
            if (ModelState.IsValid && this.Request.RequestType == "POST")
            {
                //#Add Record
                memberInfo = AddNewEntity(memberInfo);
                return RedirectToAction("Index");
            }
            return View(memberInfo);

        }

        private hqq_member AddNewEntity(hqq_member memeberInfo)
        {
            hqq_meta_location metaLocation = null;
            memeberInfo = this.AddProperty(memeberInfo);
            // Find Location Code
            if (memeberInfo.location_code != null)
            {
                metaLocation = metadataMgr.GetMetaLocationInfo(memeberInfo.location_code.Replace(", Thailand", ""));
                if (metaLocation != null && metaLocation.id > 0)
                {
                    memeberInfo.location_code = metaLocation.ISO_code;
                }
            }
            // Find HomeTown Code 
            if (memeberInfo.hometown_code != null)
            {
                metaLocation = metadataMgr.GetMetaLocationInfo(memeberInfo.hometown_code.Replace(", Thailand", ""));
                if (metaLocation != null && metaLocation.id > 0)
                {
                    memeberInfo.hometown_code = metaLocation.ISO_code;
                }
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
                Response.Redirect(string.Format("/home/login?RedirectURL={0}",
                    HttpUtility.HtmlEncode("/member/edit")));
            }
            else
            {
                if (ModelState.IsValid && this.Request.RequestType == "POST")
                {
                    hqq_member memberInfo = dbInfo.hqq_member.Find(member.id);

                    memberInfo.fullname = member.fullname;
                    memberInfo.address = member.address;

                    dbInfo.Entry(memberInfo).State = EntityState.Modified;
                    dbInfo.SaveChanges();

                    member = memberInfo;
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

        // [OutputCache(Duration = 20, VaryByParam = "photoId")]
        public ActionResult GetImage(int id, string param)
        {
            byte[] img = null;

            if (param == "thumbnail" || param == "thmb")
            {
                img = imgMgr.GetHQQImage(id, true);
            }
            else if (param == "delete" || param == "del")
            {
                //Check Admin Permission 
                //imgMgr.DeleteImage(id);
            }
            else
            {
                img = imgMgr.GetHQQImage(id, false);
            }

            return File(img, "image/png");
        }
    }
}