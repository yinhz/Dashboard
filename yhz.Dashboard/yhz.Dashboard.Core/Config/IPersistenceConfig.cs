using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.Config
{
    /// <summary>
    /// 契约-所有要被持久化到本地的内容必须实现的接口
    /// </summary>
    public interface IPersistenceConfig
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
