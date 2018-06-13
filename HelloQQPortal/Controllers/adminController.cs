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
        private MemberManager memberMgr;
        private ImageManager imageMgr;
        private ProductManager productManager;
        private ProductFaqManager productFaqManager;
        private ProductManualManager productManualMgr;

        private NameValueCollection webConfig = WebConfigurationManager.AppSettings;

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
        public ActionResult MemberDetail(int? id)
        {
            MemberInfo memberInfo = new MemberInfo();
            hqq_member member;
            memberMgr = new MemberManager();

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
            if (member.hqq_member_product.Count > 0)
            {
                memberInfo.MemberProduct = member.hqq_member_product.ToList();
            }
            memberInfo.ProductList = productManager.GetProductList();

            return View(memberInfo);
        }

        [HttpPost]
        public ActionResult UpdateMemberDetail(MemberInfo memberInfo, string command)
        {
            string rtnURL = "/admin/member";
            memberMgr = new MemberManager();

            try
            {
                if (command.Equals("updateMemberProduct"))
                {
                    if (memberInfo.SelectedProductId > 0)
                    {
                        hqq_member_product mpInfo = new hqq_member_product();

                        mpInfo.member_id = memberInfo.MemberDetail.id;
                        mpInfo.product_id = memberInfo.SelectedProductId;
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
                else if (command.Contains("cancel-purchase"))
                {
                    string selectedId = command.Split('.')[1];
                    memberMgr.DeleteMemberProduct(int.Parse(selectedId));
                }
                else
                {
                    memberMgr.UpdateMember(memberInfo.MemberDetail);
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

        [HttpPost]
        public ActionResult UpdateMemberProduct(MemberInfo memberInfo)
        {
            string rtnURL = "/admin/member";
            hqq_member_product mpInfo;
            memberMgr = new MemberManager();

            try
            {
                mpInfo = memberInfo.MemberProduct.FirstOrDefault();
                if (mpInfo != null)
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
            productManager = new ProductManager();
            List<hqq_product> lstProductInfo = productManager.GetProductList();
            return View(lstProductInfo);
        }

        // GET: Admin
        public ActionResult ProductDetail(int? id)
        {
            productManager = new ProductManager();

            ProductInfo productInfo = new ProductInfo();

            if (id.HasValue && id.Value > 0)
            {
                productInfo.ProductDetail = productManager.GetProductById(id.Value);
                if (productInfo.ProductDetail.hqq_product_images.Count() > 0)
                {

                    productInfo.ProductImage = productInfo.ProductDetail.hqq_product_images.FirstOrDefault();
                    FileInfo fileInfo = new FileInfo(productInfo.ProductImage.url);
                    productInfo.ProductImageThumbURL = string.Format("{0}/thmb-{1}",
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
            productManager = new ProductManager();

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

        public ActionResult ProductManualList()
        {
            this.productManualMgr = new ProductManualManager();

            List<hqq_product_manual> lstProductManual = productManualMgr.GetProductManualList();
            return View(lstProductManual);
        }

        // GET: Admin
        public ActionResult ProductManualDetail(int? id)
        {
            productManualMgr = new ProductManualManager();
            productManager = new ProductManager();

            ProductManualInfo productManualInfo = new ProductManualInfo();
            productManualInfo.ProductManual = new hqq_product_manual();
            productManualInfo.ProductList = productManager.GetProductList();

            if (id.HasValue)
            {
                productManualInfo.ProductManual = productManualMgr.GetProdutManualById(id.Value);
                productManualInfo.ProductManualContent = productManualInfo.ProductManual.content;
                productManualInfo.SelectedProductId = productManualInfo.ProductManual.product_id;
                productManualInfo.ProductManualImage = productManualMgr.GetManualImagesList(id.Value);
            }
            return View(productManualInfo);
        }


        [HttpPost]
        // GET: Admin
        public ActionResult UpdateProductManualDetail(ProductManualInfo productManualInfo)
        {
            this.productManualMgr = new ProductManualManager();

            hqq_product_manual productManual = new hqq_product_manual();

            string rtnURL = "/admin/product-manual";
            string strProductImg = "";

            if (productManualInfo.ProductManual.id > 0)
            {
                productManual = productManualInfo.ProductManual;
                productManual.product_id = productManualInfo.SelectedProductId;
                productManual.content = productManualInfo.ProductManualContent;
                productManual.modified_on = DateTime.Now;
                productManual.modified_by = 1;
            }
            else
            {
                productManual = productManualInfo.ProductManual;
                productManual.product_id = productManualInfo.SelectedProductId;
                productManual.content = productManualInfo.ProductManualContent;
                productManual.created_on = DateTime.Now;
                productManual.created_by = 1;
                productManual.status = 1;
            }

            productManualMgr.UpdateProductManualDetail(productManual);
            strProductImg = productManualMgr.AddProductManualImage(productManualInfo.ImageUpload, productManual.id, productManual.product_id);

            if (productManualInfo.ProductManual.id > 0)
            {
                rtnURL = rtnURL + "/" + productManualInfo.ProductManual.id;
            }

            return Redirect(rtnURL);
        }

        public ActionResult ProductManualImageDelete(int productManualId, int imageId)
        {
            imageMgr = new ImageManager();

            string rtnURL = "/admin/product-manual";
            rtnURL = rtnURL + "/" + productManualId;

            imageMgr.DeleteImage(imageId);

            return Redirect(rtnURL);
        }

        [HttpPost]
        // GET: Admin
        public ActionResult AddProductManual(ProductManualInfo productManualInfo)
        {
            productManualMgr = new ProductManualManager();

            hqq_product_manual productManual = new hqq_product_manual();

            string rtnURL = "/admin/product-manual";

            //if (productManualInfo.ProductManual.id > 0)
            //{
            //    productManual = productManualInfo.ProductManual;
            //    productManual.product_id = productManualInfo.SelectedProductId;
            //    productManual.content = productManualInfo.ProductManualContent;
            //    productManual.modified_on = DateTime.Now;
            //    productManual.modified_by = 1;
            //}
            //else
            //{
            //    productManual = productManualInfo.ProductManual;
            //    productManual.product_id = productManualInfo.SelectedProductId;
            //    productManual.content = productManualInfo.ProductManualContent;
            //    productManual.created_on = DateTime.Now;
            //    productManual.created_by = 1;
            //    productManual.status = 1;
            //}

            //productManual = productManualMgr.UpdateProductManualDetail(productManual);

            if (productManualInfo.ProductManual.id > 0)
            {
                rtnURL = rtnURL + "/" + productManualInfo.ProductManual.id;
            }

            return Redirect(rtnURL);
        }

        public ActionResult ProductFaqList()
        {
            return View();
        }

        // GET: Admin
        public ActionResult ProductFaqDetail(int? id)
        {
            productManager = new ProductManager();
            productFaqManager = new ProductFaqManager();

            ProductFaqInfo productFaqInfo = new ProductFaqInfo();
            productFaqInfo.ProductFaqList = new List<hqq_product_faq>();
            productFaqInfo.ProductFaq = new hqq_product_faq();

            productFaqInfo.ProductList = productManager.GetProductList();

            if (id.HasValue && id.Value > 0)
            {
                productFaqInfo.ProductFaqList = this.productFaqManager.GetFaqByProductId(id.Value);
                productFaqInfo.ProductFaq = new hqq_product_faq();               
            }

            return View(productFaqInfo);
        }

        public ActionResult UpdateProductFaqDetail(ProductFaqInfo productFaqInfo)
        {
            string rtnURL = "/admin/product-faq";
            productFaqManager = new ProductFaqManager();

            productFaqManager.UpdateProductFAQDetail(productFaqInfo.ProductFaq, productFaqInfo.SelectedProductId);

            if (productFaqInfo.SelectedProductId > 0)
            {
                rtnURL = rtnURL + "/" + productFaqInfo.SelectedProductId;
            }

            return Redirect(rtnURL);
        }


    }
}