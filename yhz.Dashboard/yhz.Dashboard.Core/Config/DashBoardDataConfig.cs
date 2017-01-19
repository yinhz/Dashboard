using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.Config
{
    /// <summary>
    /// 数据源配置
    /// </summary>
    public class DashboardDataConfig : IPersistenceConfig
    {
        public DashboardDataConfig()
        {
            this.DataId = System.Guid.NewGuid().ToString().Replace("-", "");
            this.DataName = "";
            this.DataBaseName = "";
            this.Sql = "";
            this.PreSql = "";
            this.IsAutoRefresh = false;
            this.Interval = 1000;
            this.CreateTime = DateTime.Now;
        }

        private string dataId;
        /// <summary>
        /// 数据源的标识
        /// </summary>
        public string DataId
        {
            get { return this.dataId; }
            set
            {
                this.dataId = value;
            }
        }

        private string dataName;
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string DataName
        {
            get { return this.dataName; }
            set
            {
                this.dataName = value;
            }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; set; }

        /// <summary>
        /// 是否自动刷新
        /// </summary>
        public bool IsAutoRefresh { get; set; }

        private double _interval;
        /// <summary>
        /// 自动刷新的频率。单位：毫秒
        /// </summary>
        public double Interval
        {
            get { return this._interval; }
            set
            {
                this._interval = value;
            }
        }

        /// <summary>
        /// SQL语句
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 预览用SQL语句
        /// </summary>
        public string PreSql { get; set; }

        #region IPersistenceConfig 成员

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }

        #endregion
    }
}
