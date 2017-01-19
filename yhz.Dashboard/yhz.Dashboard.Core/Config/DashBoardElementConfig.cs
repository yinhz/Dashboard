using System;
using System.Collections.Generic;
using System.Linq;

namespace yhz.Dashboard.Core.Config
{
    /// <summary>
    /// 看板元素配置
    /// </summary>
    public class DashboardElementConfig : IPersistenceConfig
    {
        #region 构造函数

        /// <summary>
        /// 无参构造
        /// 无法取得位置及大小信息
        /// </summary>
        public DashboardElementConfig()
        {
            this.ElementId = System.Guid.NewGuid().ToString().Replace("-", "");
            this.ElementName = "";

            this.RowIndex = 0;
            this.ColumnIndex = 0;

            this.RowSpan = 1;
            this.ColumnSpan = 1;

            this.HtmlTemplate = this.CssTemplate = this.JavaScriptTemplate = this.RenderHtml = this.Descript = "";

            this.DataConfig = new DashboardDataConfig();
            this.TypeConfig = new DashboardElementTypeConfig();

            this.ZIndex = 0;

            this.CreateTime = DateTime.Now;
        }

        #endregion

        #region 公开方法

        public void ApplyElementTypeSet()
        {
            if (string.IsNullOrEmpty(this.TypeId))
            {
                throw new Exception("无法取得 元素类型！请确认是否已指定元素类型");
            }

            if (!string.IsNullOrEmpty(this.TypeConfig.HtmlTemplate))
            {
                this.HtmlTemplate = ReplacePara(this.TypeConfig.HtmlTemplate);
            }

            if (!string.IsNullOrEmpty(this.TypeConfig.CssTemplate))
            {
                this.CssTemplate = ReplacePara(this.TypeConfig.CssTemplate);
            }

            if (!string.IsNullOrEmpty(this.TypeConfig.JavaScriptTemplate))
            {
                this.JavaScriptTemplate = ReplacePara(this.TypeConfig.JavaScriptTemplate);
            }
        }

        public void ApplyAllElementTypeSet()
        {
            if (string.IsNullOrEmpty(this.TypeId))
            {
                throw new Exception("无法取得 元素类型！请确认是否已指定元素类型");
            }

            var elementType = yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs
                .Where(t => t.TypeId == this.TypeId).First();

            if (!string.IsNullOrEmpty(elementType.HtmlTemplate))
            {
                this.HtmlTemplate = ReplacePara(elementType.HtmlTemplate);
            }

            if (!string.IsNullOrEmpty(elementType.CssTemplate))
            {
                this.CssTemplate = ReplacePara(elementType.CssTemplate);
            }

            if (!string.IsNullOrEmpty(elementType.JavaScriptTemplate))
            {
                this.JavaScriptTemplate = ReplacePara(elementType.JavaScriptTemplate);
            }

            var paraConfigs = this.TypeConfig.ParaConfigs;

            this.TypeConfig = elementType;
            this.TypeConfig.ParaConfigs = paraConfigs;
        }

        private string ReplacePara(string str)
        {
            // 如果有参数项的配置
            if (this.TypeConfig != null && this.TypeConfig.ParaConfigs != null && this.TypeConfig.ParaConfigs.Count > 0)
            {
                foreach (var para in this.TypeConfig.ParaConfigs)
                {
                    str = str
                    .Replace("[" + para.ParaCode + "]", para.Value);
                }
            }

            return str;
        }

        public DashboardElementConfig ApplyRandomElementId()
        {
            string elementId = System.Guid.NewGuid().ToString().Replace("-", "");

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

        public DashboardElementConfig ApplyDataConfig()
        {
            if (string.IsNullOrEmpty(this.DataId))
            {
                return this;
            }

            var dataConfig = DashboardRepository.DataConfigs
                .Where(d => d.DataId == this.DataId).FirstOrDefault();

            if (dataConfig == null)
            {
                throw new Exception(
                    string.Format("cant't find data config by dataid {0}", this.DataId));
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

        #endregion

        #region 公开属性

        public int ZIndex { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 这里给看板使用
        /// 看板为其使用的元素会新生成ID
        /// </summary>
        public string LocElementId { get; set; }

        /// <summary>
        /// 元素Id
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// 元素行索引
        /// 从0开始
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 元素列索引
        /// 从0开始
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 元素行跨度
        /// </summary>
        public int RowSpan { get; set; }

        /// <summary>
        /// 元素列跨度
        /// </summary>
        public int ColumnSpan { get; set; }

        /// <summary>
        /// 元素类型
        /// </summary>
        public DashboardElementTypeConfig TypeConfig { get; set; }

        /// <summary>
        /// 元素类型ID (只读）
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string TypeId
        {
            get
            {
                if (TypeConfig == null)
                    return string.Empty;

                return TypeConfig.TypeId;
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public List<ParaConfig> ParaConfigs
        {
            get
            {
                if (this.TypeConfig != null)
                {
                    return this.TypeConfig.ParaConfigs;
                }

                return null;
            }
        }

        /// <summary>
        /// 元素类型名称（只读）
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string TypeName
        {
            get
            {
                if (TypeConfig == null)
                    return string.Empty;

                return TypeConfig.TypeName;
            }
        }

        #region 元素所对应的数据的ID

        /// <summary>
        /// 元素所使用的数据
        /// </summary>
        public DashboardDataConfig DataConfig { get; set; }

        /// <summary>
        /// 数据ID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string DataId
        {
            get
            {
                if (DataConfig == null)
                    return "";

                return DataConfig.DataId;
            }
        }

        /// <summary>
        /// 数据名称
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string DataName
        {
            get
            {
                if (DataConfig == null)
                    return "";

                return this.DataConfig.DataName;
            }
        }

        #endregion

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

        /// <summary>
        /// 最终渲染html
        /// </summary>
        public string RenderHtml { get; set; }

        #endregion

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
