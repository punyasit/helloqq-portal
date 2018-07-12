using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HelloQQPortal.Database;
using HelloQQPortal.Manager;
using HelloQQPortal.Models;


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

            List<hqq_product> lstHqqProductInfo = new List<hqq_product>();
            ProductManager productMgr = new ProductManager();
            List<hqq_member_product> lstMemberProductInfo = productMgr.GetMemberProduct(2);

            return View(lstMemberProductInfo);
        }

        public ActionResult View(int id)
        {
            return View();
        }

        public ActionResult Manual(int? id)
        {
            ManualFaqInfo manualFaqInfo = new ManualFaqInfo();


            //if(Session["memberInfo"] == null)
            //{
            //    Response.Redirect("/home/login");
            //}

            // Check Session Info
            // Check Product Id  with product member info

            if (id.HasValue)
            {
                ProductManualManager productManualMgr = new ProductManualManager();
                ProductFaqManager productFaqMgr = new ProductFaqManager();

                manualFaqInfo.ManualInfo = productManualMgr.GetProductManualByProductId(id.Value);
                manualFaqInfo.FaqInfoList = productFaqMgr.GetFaqByProductId(id.Value);

                if (manualFaqInfo.ManualInfo.id > 0)
                {
                    ViewBag.Message = manualFaqInfo.ManualInfo.subject;
                }
            }
            else
            {
                Response.Redirect("/home");
            }

            return View(manualFaqInfo);
        }

        public ActionResult FAQ()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}