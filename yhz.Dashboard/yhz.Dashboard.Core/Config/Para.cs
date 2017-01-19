using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace yhz.Dashboard.Core.Config
{
    /// <summary>
    /// 参数配置
    /// </summary>
    [Newtonsoft.Json.JsonObject]
    public class ParaConfig
    {
        /// <summary>
        /// construct
        /// </summary>
        public ParaConfig()
        {
            this.ParaId = System.Guid.NewGuid().ToString()
                .Replace("-", "");
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Newtonsoft.Json.JsonProperty("ParaId")]
        public string ParaId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Newtonsoft.Json.JsonProperty("ParaName")]
        public string ParaName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Newtonsoft.Json.JsonProperty("ParaCode")]
        public string ParaCode { get; set; }
        /// <summary>
        /// 参数渲染脚本
        /// </summary>
        [Newtonsoft.Json.JsonProperty("RenderScript")]
        public string RenderScript { get; set; }
        /// <summary>
        /// 参数的值
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Value")]
        public string Value { get; set; }
    }
}
