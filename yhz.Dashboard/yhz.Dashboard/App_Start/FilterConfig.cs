using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace yhz.Dashboard
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new UrlFilter());
        }
    }

    public class ff : IAuthorizationFilter
    {
        #region IAuthorizationFilter 成员

        public void OnAuthorization(AuthorizationContext filterContext)
        {
        }

        #endregion
    }


    public class UrlFilter : IActionFilter
    {
        #region IActionFilter 成员

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpCookie cookie_language = filterContext.HttpContext.Response.Cookies["language"];
            //if (cookie_language == null
            //    || string.IsNullOrEmpty(cookie_language.Value))
            //{
            //    cookie_language = new HttpCookie("language", "en") { Expires = DateTime.Now.AddYears(10) };
            //    System.Web.HttpContext.Current.Response.Cookies.Add(cookie_language);
            //}

            //HttpCookie cookie_culture = filterContext.HttpContext.Response.Cookies["culture"];
            //if (cookie_culture == null
            //    || string.IsNullOrEmpty(cookie_culture.Value))
            //{
            //    cookie_culture = new HttpCookie("culture", "us") { Expires = DateTime.Now.AddYears(10) };
            //    System.Web.HttpContext.Current.Response.Cookies.Add(cookie_culture);
            //}

            if (!filterContext.RouteData.Values.Keys
                .Contains("language")
                ||
                !filterContext.RouteData.Values.Keys
                .Contains("culture")
                ||
                string.IsNullOrEmpty(filterContext.RouteData.Values["language"].ToString())
                ||
                string.IsNullOrEmpty(filterContext.RouteData.Values["culture"].ToString()))
            {
                filterContext.RouteData.Values.Add("language", "en");
                filterContext.RouteData.Values.Add("culture", "us");

                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                //var values = filterContext.RouteData.Values;

                //foreach (var paraKey in filterContext.HttpContext.Request.QueryString.AllKeys)
                //{
                //    values.Add(paraKey, filterContext.HttpContext.Request.QueryString[paraKey]);
                //}
                //filterContext.Result = new RedirectToRouteResult("DefaultWithCulture",
                //    filterContext.RouteData.Values);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(
                    filterContext.RouteData.Values["language"].ToString()
                    +
                    "-"
                    +
                    filterContext.RouteData.Values["culture"].ToString()
                    );
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
        }

        #endregion
    }
}