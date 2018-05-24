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
        private ProductManager productManager = new ProductManager();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Message = "Administrator.";
            ViewBag.Message = "Your application description page.";
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

        // GET: Admin
        public ActionResult ProductList()
        {
            List<hqq_product> lstProductInfo =  productManager.GetProductList();
            return View(lstProductInfo);
        }

        // GET: Admin
        public ActionResult ProductDetail(int? id)
        {
            hqq_product productInfo = new hqq_product();

            if (id.HasValue && id.Value > 0)
            {
                productInfo = productManager.GetProductById(id.Value);
            }

            return View(productInfo);
        }

        [HttpPost]
        public ActionResult UpdateProductDetail(hqq_product productInfo)
        {
            try
            {
                if (productInfo.id == 0)
                {
                    productInfo.created_on = DateTime.Now;
                    productInfo.created_by = 1;
                    productInfo = productManager.UpdateMember(productInfo);
                }
                else
                {
                    productInfo.modified_on = DateTime.Now;
                    productInfo.modified_by = 1;
                    productInfo = productManager.UpdateMember(productInfo);
                }
            }
            catch(Exception)
            {
                return View(productInfo);
            }

            return Redirect("/admin/product");
          
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