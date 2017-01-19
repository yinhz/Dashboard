using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace yhz.Dashboard
{
    /// <summary>
    /// web api 配置
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DaoApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
