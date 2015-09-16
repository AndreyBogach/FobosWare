using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ListOfEmployees.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                 "",
                 new
                 {
                     controller = "Employee",
                     action = "Home",
                     status = (string)null,
                     page = 1
                 }
             );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Employee", action = "Home", status = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{status}",
                new { controller = "Employee", action = "Home", page = 1 }
            );

            routes.MapRoute(null,
                "{status}/Page{page}",
                new { controller = "Employee", action = "Home" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}