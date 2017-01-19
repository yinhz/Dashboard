using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAUtility
{
    public static class UAUtilityConfig
    {
        /// <summary>
        /// OData 地址
        /// </summary>
        public static string UA_ODATA_ENDPOINT =
            System.Configuration.ConfigurationManager.AppSettings["UA_ODATA_ENDPOINT"];
            //@"http://w12ua02/sit-svc/runtime/odata/";

        /// <summary>
        /// 简单模式所要请求的地址
        /// </summary>
        public static string UA_OAUTH_AUTHORIZE_ENDPOINT =
            System.Configuration.ConfigurationManager.AppSettings["UA_OAUTH_AUTHORIZE_ENDPOINT"];
            //@"http://w12ua02/sit-auth/OAuth/Authorize";
        /// <summary>
        /// 密码模式所要请求的 url 地址
        /// </summary>
        public static string UA_OAUTH_ROPC_POINT =
            System.Configuration.ConfigurationManager.AppSettings["UA_OAUTH_ROPC_POINT"];
            //@"http://w12ua02/sit-auth/OAuth/Token";

        /// <summary>
        /// UA 资源所有者用户名
        /// </summary>
        public static string UA_RO_USERNAME =
            System.Configuration.ConfigurationManager.AppSettings["UA_RO_USERNAME"];
            //@"w12ua02\Administrator";
        /// <summary>
        /// UA资源所有者密码
        /// </summary>
        public static string UA_RO_PASSWORD =
            System.Configuration.ConfigurationManager.AppSettings["UA_RO_PASSWORD"];
            //@"Siemens!1";
    }
}
