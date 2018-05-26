using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.IO;
using HelloQQPortal.Database;
using HelloQQPortal.Manager;
using HelloQQPortal.Models;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Globalization;

namespace HelloQQPortal.Controllers
{
    public class adminController : Controller
    {
        private MemberManager memberMgr = new MemberManager();
        private ProductManager productManager = new ProductManager();
        private NameValueCollection webConfig = WebConfigurationManager.AppSettings;
        public MemberInfo memberInfo = new MemberInfo();

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
            hqq_member member;

            memberInfo.MemberDetail = new hqq_member();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            member = memberMgr.GetMemberById(id.Value);
            

            if (member == null)
            {
                return HttpNotFound();
            }

            memberInfo.MemberDetail = member;
            if(member.hqq_member_product.Count> 0)
            {
                memberInfo.MemberProduct = member.hqq_member_product.ToList();
            }
            memberInfo.ProductList = productManager.GetProductList();

            return View(memberInfo);
        }


        [HttpPost]
        public ActionResult UpdateUserDetail(MemberInfo memberInfo)
        {
            string rtnURL = "/admin/member";

            try
            {
                memberMgr.UpdateMember(memberInfo.MemberDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (memberInfo.MemberDetail.id > 0)
            {
                rtnURL = rtnURL + "/" + memberInfo.MemberDetail.id;
            }

            return Redirect(rtnURL);

        }

        [HttpPost]
        public ActionResult UpdateUserProduct(MemberInfo memberInfo)
        {
            string rtnURL = "/admin/member";
            hqq_member_product mpInfo;

            try
            {
                mpInfo = memberInfo.MemberProduct.FirstOrDefault();
                if(mpInfo != null)
                {
                    mpInfo.member_id = memberInfo.MemberDetail.id;
                    mpInfo.purchase_date = DateTime.ParseExact(memberInfo.PurchasedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture); 
                    mpInfo.garantee_expired = mpInfo
                        .purchase_date.AddMonths(
                        int.Parse(webConfig["product.garantee"]));

                    memberMgr.UpdateMemberProduct(mpInfo);                    

                    //memberProductInfo.garantee_expired = 
                    //memberInfo.PurchasedOn
                    //memberProductInfo.purchase_date = 
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (memberInfo.MemberDetail.id > 0)
            {
                rtnURL = rtnURL + "/" + memberInfo.MemberDetail.id;
            }

            return Redirect(rtnURL);
        }

        // GET: Admin
        public ActionResult ProductList()
        {
            List<hqq_product> lstProductInfo = productManager.GetProductList();
            return View(lstProductInfo);
        }

        // GET: Admin
        public ActionResult ProductDetail(int? id)
        {
            ProductInfo productInfo = new ProductInfo();

            if (id.HasValue && id.Value > 0)
            {
                productInfo.ProductDetail = productManager.GetProductById(id.Value);
                if (productInfo.ProductDetail.hqq_product_images.Count() > 0)                {

                    productInfo.ProductImage = productInfo.ProductDetail.hqq_product_images.FirstOrDefault();
                    FileInfo fileInfo = new FileInfo(productInfo.ProductImage.url);
                    productInfo.ProductImageThumbURL = string.Format("{0}thmb-{1}",
                        productInfo.ProductImage.url.Substring(0, productInfo.ProductImage.url.LastIndexOf("/"))
                        , fileInfo.Name, fileInfo.Extension);
                }
            }
            else
            {
                productInfo.ProductDetail = new hqq_product();
            }

            return View(productInfo);
        }

        [HttpPost]
        public ActionResult UpdateProductDetail(ProductInfo productInfo)
        {
            hqq_product productDetail = productInfo.ProductDetail;
            string rtnURL = "/admin/product";

            try
            {
                if (productDetail.id == 0)
                {
                    productDetail.created_on = DateTime.Now;
                    productDetail.created_by = 1;
                    productDetail = productManager.UpdateProductDetail(productDetail);

                }
                else
                {
                    productDetail.modified_on = DateTime.Now;
                    productDetail.modified_by = 1;
                    productDetail = productManager.UpdateProductDetail(productDetail);

                }

                if (productInfo.ImageUpload != null && productDetail.id > 0)
                {
                    productManager.AddProductImage(
                        productInfo.ProductDetail, 
                        productInfo.ImageUpload);
                    
                }
            }
            catch (Exception ex)
            {
                return View(productInfo);
            }

            if (productInfo.ProductDetail.id > 0)
            {
                rtnURL = rtnURL + "/" + productInfo.ProductDetail.id;
            }

            return Redirect(rtnURL);
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