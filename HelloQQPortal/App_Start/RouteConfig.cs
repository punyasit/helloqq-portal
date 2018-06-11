using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelloQQPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("members/");

            routes.MapRoute(
                name: "admin-product",
                url: "admin/product/",
                defaults: new { controller = "admin", action = "productlist", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "admin-product-edit",
                url: "admin/product/{id}",
                defaults: new { controller = "admin", action = "productDetail", id = UrlParameter.Optional }
                );

            routes.MapRoute(
               name: "admin-product-manual",
               url: "admin/product-manual/",
               defaults: new { controller = "admin", action = "ProductManualList", id = UrlParameter.Optional }
               );

            routes.MapRoute(
                name: "admin-product-manual-edit",
                url: "admin/product-manual/{id}",
                defaults: new { controller = "admin", action = "ProductManualDetail", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                 name: "admin-product-faq",
                 url: "admin/product-faq/",
                 defaults: new { controller = "admin", action = "ProductFaqList", id = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "admin-product-faq-edit",
                url: "admin/product-faq/{id}",
                defaults: new { controller = "admin", action = "ProductFaqDetail", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "admin-product-manual-delete-image",
                url: "admin/product-manual-image/{productManualId}/{imageId}",
                defaults: new { controller = "admin", action = "ProductManualImageDelete" }
                );

            routes.MapRoute(
               name: "admin-member",
               url: "admin/member/",
               defaults: new { controller = "admin", action = "MemberList", id = UrlParameter.Optional }
               );

            routes.MapRoute(
               name: "admin-member-edit",
               url: "admin/member/{id}",
               defaults: new { controller = "admin", action = "Memberdetail", id = UrlParameter.Optional }
               );

            routes.MapRoute(
               name: "GetImage",
                url: "image/{id}/{param}",
               defaults: new { controller = "Home", action = "GetImage", id = UrlParameter.Optional, param = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "register",
              url: "register",
              defaults: new { controller = "home", action = "register", id = UrlParameter.Optional }
              );

            routes.MapRoute(
                name: "default",
                url: "{controller}",
                defaults: new { controller = "home", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "default-action",
                url: "{controller}/{action}",
                defaults: new { controller = "home", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
              name: "Product",
              url: "product/{id}/{action}",
              defaults: new { controller = "product" }
            );

            routes.MapRoute(
                name: "Home",
                url: "home/{action}/{id}",
                defaults: new { controller = "home" }
            );


        }
    }
}
