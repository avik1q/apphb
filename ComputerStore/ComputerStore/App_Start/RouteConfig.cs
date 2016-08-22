using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComputerStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "home",
                url: "",
                defaults: new { controller = "Home", action = "home", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "buy",
                url: "buy",
                defaults: new { controller = "Home", action = "buy", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "items",
                url: "GetitemsReqAsync",
                defaults: new { controller = "Home", action = "GetitemsReqAsync", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
