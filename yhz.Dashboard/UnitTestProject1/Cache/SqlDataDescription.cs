using Newtonsoft.Json.Linq;
using System;

namespace UnitTestProject1.Cache
{
    public class SqlDataDescription
    {
        #region contructor
        public SqlDataDescription(string sql, int interval = 0, JObject paras = null)
        {
            this.Sql = sql;
            this.Paras = paras;

            this.Interval = interval;
            this.QueryTime = DateTime.Now;
        }

        #endregion

        #region properties

        public string Sql { get; private set; }
        public JObject Paras { get; private set; }
        public int Interval { get; private set; }
        public DateTime QueryTime { get; set; }

        #endregion
    }
}