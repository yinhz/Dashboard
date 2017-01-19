using log4net;
using yhz.Dashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace yhz.Dashboard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog m_log = log4net.LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            // 这里重新设置 web api 序列化使用的设置
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings =
                ObjectHelper.DefaultJsonSerializerSettings;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //Thread.CurrentThread.CurrentCulture = 
        }

        protected void Application_Error()
        {
            if (this.Context != null
                && this.Context.AllErrors != null
                && this.Context.AllErrors.Length > 0)
            {
                //var resp = new HttpResponseMessage(HttpStatusCode.NotFound);

                foreach (var error in this.Context.AllErrors)
                {

                    //resp.Content = new StringContent(error.ToString());
                    //resp.ReasonPhrase = "";

                    m_log.Error(error.ToString() + ".trace:" + error.StackTrace);
                }

                //throw new HttpResponseException(resp);
            }
        }
    }
}
