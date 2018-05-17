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
               name: "admin-member",
               url: "admin/member/{action}",
               defaults: new { controller = "members", action = "Index", id = UrlParameter.Optional }
               );

            routes.MapRoute(
              name: "register",
              url: "register",
              defaults: new { controller = "members", action = "register", id = UrlParameter.Optional }
              );

            routes.MapRoute(
                name:"default",
                url:"{controller}",
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
              defaults: new { controller = "product"}
            );

            routes.MapRoute(
                name: "Home",
                url: "home/{action}/{id}",
                defaults: new { controller = "home" }
            );

          
        }
    }
}
