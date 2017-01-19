using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.Config
{
    /// <summary>
    /// 看板元素类型
    /// </summary>
    public class DashboardElementTypeConfig : IPersistenceConfig
    {
        public DashboardElementTypeConfig()
        {
            this.TypeId = System.Guid.NewGuid().ToString().Replace("-", "");
            this.TypeName = "";

            this.Descript = "";
            this.HtmlTemplate = "";
            this.CssTemplate = "";
            this.JavaScriptTemplate = "";

            this.CreateTime = DateTime.Now;

            this.ParaConfigs = new List<ParaConfig>();
        }

        /// <summary>
        /// 元素类型的标识
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 元素类型的名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 此类型元素的HTML模版
        /// </summary>
        public string HtmlTemplate { get; set; }

        /// <summary>
        /// 次类型元素的脚本模版
        /// </summary>
        public string JavaScriptTemplate { get; set; }

        /// <summary>
        /// Css模版
        /// </summary>
        public string CssTemplate { get; set; }

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

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<ParaConfig> ParaConfigs { get; set; }

        public DashboardElementTypeConfig ApplyRandomElementId(string elementId)
        {
            if (string.IsNullOrEmpty(elementId))
                return this;

            if (!string.IsNullOrEmpty(this.HtmlTemplate))
            {
                this.HtmlTemplate =
                    this.HtmlTemplate.Replace("[ElementId]", elementId);
            }

            if (!string.IsNullOrEmpty(this.CssTemplate))
            {
                this.CssTemplate =
                    this.CssTemplate.Replace("[ElementId]", elementId);
            }

            if (!string.IsNullOrEmpty(this.JavaScriptTemplate))
            {
                this.JavaScriptTemplate =
                    this.JavaScriptTemplate.Replace("[ElementId]", elementId);
            }

            return this;
        }

        public DashboardElementTypeConfig ApplyDataConfig(string dataId)
        {
            if (string.IsNullOrEmpty(dataId))
            {
                return this;
            }

            var dataConfig = DashboardRepository.DataConfigs
                .Where(d => d.DataId == dataId).FirstOrDefault();

            if (dataId == null)
            {
                return this;
            }

            if (!string.IsNullOrEmpty(this.HtmlTemplate))
            {
                this.HtmlTemplate = ReplaceDataConfig(this.HtmlTemplate, dataConfig);
            }

            if (!string.IsNullOrEmpty(this.CssTemplate))
            {
                this.CssTemplate = ReplaceDataConfig(this.CssTemplate, dataConfig);
            }

            if (!string.IsNullOrEmpty(this.JavaScriptTemplate))
            {
                this.JavaScriptTemplate = ReplaceDataConfig(this.JavaScriptTemplate, dataConfig);
            }

            return this;
        }

        private string ReplaceDataConfig(string str, DashboardDataConfig dataConfig)
        {
            return
                str
                .Replace("[DataId]", dataConfig.DataId)
                .Replace("[IsAutoRefresh]", dataConfig.IsAutoRefresh.ToString().ToLower())
                .Replace("[Interval]", dataConfig.Interval.ToString());
        }

        public DashboardElementTypeConfig ApplyParaConfigs()
        {
            if (string.IsNullOrEmpty(this.TypeId))
            {
                throw new Exception("无法取得 元素类型！请确认是否已指定元素类型");
            }

            if (!string.IsNullOrEmpty(this.HtmlTemplate))
            {
                this.HtmlTemplate = ReplacePara(this.HtmlTemplate);
            }

            if (!string.IsNullOrEmpty(this.CssTemplate))
            {
                this.CssTemplate = ReplacePara(this.CssTemplate);
            }

            if (!string.IsNullOrEmpty(this.JavaScriptTemplate))
            {
                this.JavaScriptTemplate = ReplacePara(this.JavaScriptTemplate);
            }

            return this;
        }

        private string ReplacePara(string str)
        {
            // 如果有参数项的配置
            if (this.ParaConfigs != null && this.ParaConfigs.Count > 0)
            {
                foreach (var para in this.ParaConfigs)
                {
                    str = str
                    .Replace("[" + para.ParaCode + "]", para.Value);
                }
            }

            return str;
        }
    }
}
