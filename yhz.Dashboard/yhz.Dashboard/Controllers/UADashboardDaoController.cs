using yhz.Dashboard.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using yhz.Dashboard.Core.Db;
using System.Web;
using log4net;
using yhz.Dashboard.Cache;
using UAUtility.OData;

namespace yhz.Dashboard.Controllers
{
    /// <summary>
    /// data access control
    /// </summary>
    public class UADashboardDaoController : ApiController
    {
        public UADashboardDaoController()
        {
            // 获取 UA 开放认证服务
            this.uaRequestService = UARequestServiceFactory.GetService();
        }
        /// <summary>
        /// UA 开放认证服务接口
        /// </summary>
        private IUARequestService uaRequestService;

        private static ILog m_log = log4net.LogManager.GetLogger(typeof(UADashboardDaoController));

        private static SqlDataCache m_sqlDataCache = new SqlDataCache();

        public object GetData(string id, string paras = null, string terminalId = null)
        {
            m_log.Info(
                string.Format(
                "Query begin! data source Id:{0},data paras:{1},terminal id:{2}"
                , id, paras, terminalId));

            // get datasource from ua
            var datasource =
                ((JObject.Parse(
                    this.uaRequestService.EntityQuery(
                        "Datasource",
                        "$select=Sql,Interval,AutoRefresh,Database&$expand=Database&$filter=Id eq " + id)
                ))["value"] as JArray)[0] as JObject;

            //// if data doesn't exists. throw exception
            //if (data == null)
            //{
            //    string error = string.Format("can't find data source by id。DataId ： {0}", id);

            //    m_log.Error(error);

            //    throw new Exception(error);
            //}

            object result;
            var sqlDataDescription = new SqlDataDescription(
                datasource["Database"]["Name"].Value<string>(),
                datasource["Sql"].Value<string>(),
                datasource["Interval"].Value<int>(),
                paras);

            // check if can we use cache result
            if (m_sqlDataCache.TryGetFromCache(sqlDataDescription, out result))
            {
                return result;
            }

            JObject database = datasource["Database"] as JObject;

            // if we can't use cache, we need get data from database.

            AbsDataBase db = DatabaseBuilder.Build(
                    (DatabaseType)Enum.Parse(typeof(DatabaseType), database["DatabaseType"].Value<int>().ToString()),
                    database["Name"].Value<string>(),
                    database["ConnectionString"].Value<string>(),
                    database["ExternalPara"].Value<string>());

            try
            {
                // if there is no paras
                if (string.IsNullOrEmpty(paras))
                {
                    result = db.Query(sqlDataDescription.Sql);
                }
                // query by paras
                else
                {
                    JObject parasObj = JObject.Parse(paras);

                    result = db.Query(sqlDataDescription.Sql, parasObj);
                }
            }
            catch (Exception e)
            {
                m_log.Error(string.Format("Query exception.info{0},stack:{1}", e.Message, e.StackTrace), e);

                throw e;
            }

            m_log.Info("Query end!");

            // before return,we need cache new result 
            m_sqlDataCache.AddtoCache(sqlDataDescription, result);

            return result;
        }
    }
}
