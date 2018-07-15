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
        private UIManager uiManager;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            uiManager = new UIManager();
            uiManager.ValidatePermission(this.ToString());
        }

        // GET: Product
        public ActionResult Index()
        {
            hqq_member memeberInfo;
            List<MemberProductInfo> lstMemberPrdInfo = new List<MemberProductInfo>();
            MemberProductInfo memberPrdInfo = new MemberProductInfo();

            if (Session["memberInfo"] != null)
            {
                memeberInfo = (hqq_member)Session["memberInfo"];

                List<hqq_product> lstHqqProductInfo = new List<hqq_product>();
                List<int> lstSelectItem = new List<int>();

                ProductManager productMgr = new ProductManager();
                ProductManualManager productManualMgr = new ProductManualManager();

                List<hqq_member_product> lstMemberProduct = productMgr.GetMemberProduct(memeberInfo.id);
                if (lstMemberProduct.Count > 0)
                {
                    lstSelectItem = lstMemberProduct.Select(item => item.product_id).ToList();
                    lstSelectItem = productManualMgr.CheckAvailableManual(lstSelectItem);

                    foreach (var item in lstMemberProduct)
                    {                        
                        memberPrdInfo = new MemberProductInfo();
                        memberPrdInfo.ProductId = item.product_id;
                        memberPrdInfo.ProductName = item.hqq_product.name;
                        memberPrdInfo.ProductDesc = item.hqq_product.description;

                        memberPrdInfo.PurchaseDate = item.purchase_date;
                        memberPrdInfo.GuaranteeExpire = item.garantee_expired;

                        memberPrdInfo.ProductImagesId = item.hqq_product.hqq_product_images.Select(selecteItem => selecteItem.id).ToList();
                        memberPrdInfo.HaveManual = lstSelectItem.Contains(item.product_id);

                        lstMemberPrdInfo.Add(memberPrdInfo);
                    }
                }
               
            }        

            return View(lstMemberPrdInfo);
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