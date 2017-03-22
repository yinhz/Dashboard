using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dashboard
{
    public interface IDashboard<TKey>
    {
        /// <summary>
        /// 看板标识
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// 看板名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }

        int RowNums { get; set; }

        int ColumnNums { get; set; }

        /// <summary>
        /// 背景图片
        /// </summary>
        string BackgroundImagePath { get; set; }
    }
}