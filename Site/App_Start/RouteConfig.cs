using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "LoginView", id = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "CustomRout",
            url: "{controller}/{action}/{id}/{publication_type}",
            defaults: new { controller = "Publication", action = "Index", id = UrlParameter.Optional, publication_type = UrlParameter.Optional }
            );

        }
    }
}
