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

namespace yhz.Dashboard.Controllers
{
    /// <summary>
    /// data access control
    /// </summary>
    public class DaoController : ApiController
    {
        private static ILog m_log = log4net.LogManager.GetLogger(typeof(DaoController));

        private static SqlDataCache m_sqlDataCache = new SqlDataCache();

        public object GetData(string id, string paras = null, string terminalId = null)
        {
            m_log.Info(
                string.Format(
                "Query begin! data source Id:{0},data paras:{1},terminal id:{2}"
                , id, paras, terminalId));
            
            // check data source by data'id
            var data = yhz.Dashboard.Core.DashboardRepository.DataConfigs
                .Where(d => d.DataId == id).FirstOrDefault();

            // if data doesn't exists. throw exception
            if (data == null)
            {
                string error = string.Format("can't find data source by id。DataId ： {0}", id);

                m_log.Error(error);

                throw new Exception(error);
            }


            object result;
            var sqlDataDescription = new SqlDataDescription(
                data.DataBaseName,
                data.Sql,
                (int)data.Interval,
                paras);

            // check if can we use cache result
            if (m_sqlDataCache.TryGetFromCache(sqlDataDescription, out result))
            {
                return result;
            }

            // if we can't use cache, we need get data from database.

            AbsDataBase db = yhz.Dashboard.Core.DashboardRepository.DataBasies
                .Where(d => d.DataBaseName == data.DataBaseName).FirstOrDefault();

            if (db == null)
            {
                string error = string.Format("can't find database by [{0}],please check the config", data.DataBaseName);

                m_log.Error(error);

                throw new Exception(error);
            }

            try
            {
                // if there is no paras
                if (string.IsNullOrEmpty(paras))
                {
                    result = db.Query(data.Sql);
                }
                // query by paras
                else
                {
                    JObject parasObj = JObject.Parse(paras);

                    result = db.Query(data.Sql, parasObj);
                }
            }
            catch (Exception e)
            {
                m_log.Error(string.Format("Query exception.info{0},stack:{1}", e.Message, e.StackTrace));

                //return this.GetTempleData();

                //throw e;
            }

            m_log.Info("Query end!");

            // before return,we need cache new result 
            m_sqlDataCache.AddtoCache(sqlDataDescription, result);

            return result;
        }

        /// <summary>
        /// 获取看板数据
        /// 传值数据项ID
        /// 终端ID（终端ID 以后考虑去掉、根据客户端信息来自动获取）
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        public object GetDashboardData(string Id = null, string terminalId = null)
        {
            m_log.Info(
                string.Format(
                "查询开始!数据标识:{0},终端标识:{1}"
                , Id, terminalId));

            // 检查数据项
            var data = yhz.Dashboard.Core.DashboardRepository.DataConfigs
                .Where(d => d.DataId == Id).FirstOrDefault();

            // 如果没找到数据项或者没有传递数据项标识、则返回 模拟数据
            if (data == null)
            {
                m_log.Error(string.Format("无法根据指定的看板数据项标识获得相应的数据项。DataId ： {0}", Id));

                #region 测试代码、以后会拿掉

                return this.GetTempleData();

                #endregion

                throw new Exception(
                    string.Format("无法根据指定的看板数据项标识获得相应的数据项。DataId ： {0}", Id));
            }

            // 取得数据库访问对象
            AbsDataBase db = yhz.Dashboard.Core.DashboardRepository.DataBasies
                .Where(d => d.DataBaseName == data.DataBaseName).FirstOrDefault();

            if (db == null)
            {
                m_log.Error(string.Format("无法根据指定的数据库名，获取相应的数据库信息.数据库名称：{0}", data.DataBaseName));

                throw new Exception(
                    string.Format("无法根据指定的数据库名，获取相应的数据库信息.数据库名称：{0}", data.DataBaseName));
            }

            object result;

            try
            {
                // 没传终端、则直接执行sql
                if (string.IsNullOrEmpty(terminalId))
                {
                    result = db.Query(data.Sql);
                }
                // 传递了终端ID 则取出 终端 参数、并进行查询
                else
                {
                    // 检查终端
                    var ter = yhz.Dashboard.Core.DashboardRepository.TerminalInfos
                        .Where(t => t.TerminalId == terminalId).FirstOrDefault();

                    if (ter == null)
                    {
                        m_log.Error(string.Format("不存在此终端的配置！无法执行查询。终端标识：{0}", ter.TerminalId));

                        return this.GetTempleData();

                        throw new Exception(
                            string.Format("不存在此终端的配置！无法执行查询。终端标识：{0}", ter.TerminalId));
                    }

                    result = db.Query(data.Sql, JObject.Parse(ter.Paras));
                }
            }
            catch (Exception e)
            {
                m_log.Error(string.Format("执行发生异常.异常信息{0},堆栈:{1}", e.Message, e.StackTrace));

                return this.GetTempleData();

                throw e;
            }

            m_log.Info("查询结束!");

            return result;
        }

        /// <summary>
        /// 测试代码
        /// </summary>
        /// <returns></returns>
        private DataSet GetTempleData()
        {
            DataSet ds = new DataSet();

            DataTable dt = new DataTable("Table");
            dt.Columns.Add("xCName", typeof(string));
            dt.Columns.Add("serieCName", typeof(string));
            dt.Columns.Add("valueCName", typeof(int));

            dt.Rows.Add("2015-05-01", "甲班", 500);
            dt.Rows.Add("2015-05-01", "乙班", 300);
            dt.Rows.Add("2015-05-02", "甲班", 400);
            dt.Rows.Add("2015-05-02", "乙班", 200);

            dt.AcceptChanges();

            ds.Tables.Add(dt);

            DataTable dt1 = new DataTable("Table1");
            dt1.Columns.Add("Title", typeof(string));
            dt1.Columns.Add("Subtitle", typeof(string));
            dt1.Columns.Add("YAxisTile", typeof(string));

            dt1.Rows.Add("Title", "这是一个测试的标题", "这是Y轴的标题");

            dt1.AcceptChanges();

            ds.Tables.Add(dt1);

            return ds;

            //// 当前为了测试方便。 id 与 paras 都不传的话。直接返回 工单数据。
            //SqlConnection connection = new SqlConnection("server=hengde;database=SitMesDb;uid=sa;pwd=siemens");
            //connection.Open();
            //SqlCommand cmd = new SqlCommand(
            //    "select top 5 OrderID '工单ID',OrderType '工单类型',DefDescript '物料', LineID '产线' from dbo.co_pom_order",
            //    connection);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //return ds;

        }
    }
}
