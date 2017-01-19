using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace yhz.Dashboard.Core.Config
{
    public class DashboardTerminalInfo : IPersistenceConfig
    {
        public DashboardTerminalInfo()
        {
            this.IsForbidden = false;
            //this.Paras = new JObject();
            this.LastEditTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            this.Theme = "default";

            this.CreateTime = DateTime.Now;

            this.Dashboards = new List<TerDashboardConfig>();
        }

        private string _terminalId;
        /// <summary>
        /// 终端的标识
        /// </summary>
        public string TerminalId
        {
            get
            {
                return this._terminalId;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Regex regex = new Regex("^[0-9a-zA-Z_]+$");
                    if(!regex.Match(value).Success)
                        throw new Exception("终端的标识只能由数字字母下划线组成！");
                }

                this._terminalId = value;
            }
        }

        public string RedirectTerminalId
        {
            get;
            set;
        }
        public string RedirectTerminalName { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        public string TerminalName { get; set; }
        /// <summary>
        /// 终端信息
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 看板集合
        /// </summary>
        public List<TerDashboardConfig> Dashboards { get; set; }

        /// <summary>
        /// 看板标识
        /// </summary>
        public string DashboardId { get; set; }
        /// <summary>
        /// 看板名称
        /// </summary>
        public string DashboardName
        {
            get
            {
                var dbc = DashboardRepository.DashboardConfigs
                    .Where(d => d.DashboardId == DashboardId).FirstOrDefault();

                if (dbc != null)
                    return dbc.DashboardName;

                return "";
            }
        }
        /// <summary>
        /// 针对终端的参数
        /// </summary>
        public string Paras { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsForbidden { get; set; }

        /// <summary>
        /// 上次编辑时间
        /// 保存到秒、使用字符串格式、避免时间序列化格式的麻烦
        /// 
        /// 客户端使用此字符串来判断是否要更新
        /// </summary>
        public string LastEditTime
        {
            get;
            set;
        }

        /// <summary>
        /// 这个终端所使用的样式的名称
        /// </summary>
        public string Theme
        {
            get;
            set;
        }

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

    public class TerDashboardConfig : DashboardConfig
    {
        public TerDashboardConfig()
            : base()
        {
            this.PlayOrder = 0;
            this.PlayCycle = 60;
        }

        /// <summary>
        /// 播放顺序
        /// </summary>
        public int PlayOrder { get; set; }

        /// <summary>
        /// 播放时长
        /// </summary>
        public int PlayCycle { get; set; }

        /// <summary>
        /// 看板主题
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 看板参数
        /// </summary>
        public string Paras { get; set; }
    }
}
