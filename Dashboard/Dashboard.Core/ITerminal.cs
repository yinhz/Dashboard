using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface ITerminal : ITerminal<string>
    {
    }

    public interface ITerminal<TKey>
    {
        TKey Id { get; set; }
        string TerminalName { get; set; }
        string Ip { get; set; }

        ///// <summary>
        ///// 看板集合
        ///// </summary>
        //IEnumerable<TerDashboardConfig> Dashboards { get; set; }

        /// <summary>
        /// 针对终端的参数
        /// </summary>
        string Paras { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        bool IsForbidden { get; set; }

        /// <summary>
        /// 上次编辑时间
        /// 保存到秒、使用字符串格式、避免时间序列化格式的麻烦
        /// 
        /// 客户端使用此字符串来判断是否要更新
        /// </summary>
        string LastEditTime { get; set; }

        /// <summary>
        /// 这个终端所使用的样式的名称
        /// </summary>
        string Theme { get; set; }
    }
}
