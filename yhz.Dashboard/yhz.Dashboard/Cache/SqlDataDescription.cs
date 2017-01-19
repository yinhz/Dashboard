using Newtonsoft.Json.Linq;
using System;

namespace yhz.Dashboard.Cache
{
    /// <summary>
    /// sql 数据描述
    /// 包含
    ///     数据库名称、SQL语句、参数、轮询时间、上次查询时间
    /// </summary>
    public class SqlDataDescription
    {
        #region contructor
        public SqlDataDescription(
            string databaseName, 
            string sql, 
            int interval = 0, 
            string paras = null)
        {
            this.DatabaseName = databaseName;
            this.Sql = sql;
            this.Paras = paras;

            this.Interval = interval;
            this.QueryTime = DateTime.Now;
        }

        #endregion

        #region properties

        public string DatabaseName { get; private set; }
        public string Sql { get; private set; }
        public string Paras { get; private set; }
        public int Interval { get; private set; }
        public DateTime QueryTime { get; set; }

        #endregion
    }
}