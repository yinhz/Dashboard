using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface IDashboardElement : IDashboardElement<string>
    { }

    public interface IDashboardElement<TKey> : IElement<TKey>
    {
        /// <summary>
        /// 元素行索引
        /// 从0开始
        /// </summary>
        int RowIndex { get; set; }

        /// <summary>
        /// 元素列索引
        /// 从0开始
        /// </summary>
        int ColumnIndex { get; set; }

        /// <summary>
        /// 元素行跨度
        /// </summary>
        int RowSpan { get; set; }

        /// <summary>
        /// 元素列跨度
        /// </summary>
        int ColumnSpan { get; set; }

        /// <summary>
        /// ZIndex
        /// </summary>
        int ZIndex { get; set; }
    }
}
