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

namespace HelloQQPortal.Controllers
{
    public class adminController : Controller
    {
        private MemberManager memberMgr = new MemberManager();
        private ProductManager productManager = new ProductManager();
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
                    //UploadFile(productInfo);
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

        private void UploadFile(ProductInfo productInfo)
        {
            string pathPattern = "{0}/{1:0000}/";
            string filePattern = "{0}/{1}";
            string filePath = string.Format(pathPattern, webConfig["image.location"], 
                productInfo.ProductDetail.id.ToString());

            if (!Directory.Exists(Server.MapPath(filePath))){
                Directory.CreateDirectory(Server.MapPath(filePath));
            }

            filePath = string.Format(filePattern, filePath, productInfo.ImageUpload.FileName);
            productInfo.ImageUpload.SaveAs(Server.MapPath(filePath));
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

        //public ActionResult Upload()
        //{
        //    bool isSavedSuccessfully = true;
        //    string fName = "";
        //    try
        //    {
        //        foreach (string fileName in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[fileName];
        //            fName = file.FileName;
        //            if (file != null && file.ContentLength > 0)
        //            {
        //                var path = Path.Combine(Server.MapPath("~/MyImages"));
        //                string pathString = System.IO.Path.Combine(path.ToString());
        //                var fileName1 = Path.GetFileName(file.FileName);
        //                bool isExists = System.IO.Directory.Exists(pathString);
        //                if (!isExists) System.IO.Directory.CreateDirectory(pathString);
        //                var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
        //                file.SaveAs(uploadpath);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        isSavedSuccessfully = false;
        //    }
        //    if (isSavedSuccessfully)
        //    {
        //        return Json(new
        //        {
        //            Message = fName
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            Message = "Error in saving file"
        //        });
        //    }
        //}
    }
}