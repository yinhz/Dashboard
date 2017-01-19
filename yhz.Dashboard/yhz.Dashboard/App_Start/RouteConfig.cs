using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace yhz.Dashboard
{
    /// <summary>
    /// url 路由配置
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DefaultWithCulture",
                url: "{language}-{culture}/{controller}/{action}/{id}",
                defaults: new { language = "en", culture = "us", controller = "Dashboard", action = "Main", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Main", id = UrlParameter.Optional }
            );
        }
    }
}
