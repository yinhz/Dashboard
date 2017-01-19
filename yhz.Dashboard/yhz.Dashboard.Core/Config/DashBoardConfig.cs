using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.Config
{
    /// <summary>
    /// 看板配置
    /// </summary>
    public class DashboardConfig : IPersistenceConfig
    {
        #region 构造函数

        /// <summary>
        /// 无参构造
        /// 无法取得行宽、列高等信息
        /// </summary>
        public DashboardConfig()
        {
            this.DashboardId = System.Guid.NewGuid().ToString().Replace("-", "");

            this.DashboardName = "";

            this.RowNums = 1;
            this.ColumnNums = 1;

            this.ElementConfigs = new List<DashboardElementConfig>();

            this.CreateTime = DateTime.Now;
        }

        #endregion

        #region 私有变量

        /// <summary>
        /// 看板宽度
        /// </summary>
        private double? m_width = null;
        /// <summary>
        /// 看板高度
        /// </summary>
        private double? m_heigth = null;

        #endregion

        #region 公开属性

        /// <summary>
        /// 看板标识
        /// </summary>
        public string DashboardId { get; set; }

        /// <summary>
        /// 看板名称
        /// </summary>
        public string DashboardName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 看板切割行数
        /// </summary>
        private int _rowNums;
        /// <summary>
        /// 看板切割行数
        /// </summary>
        public int RowNums
        {
            get { return _rowNums; }
            set
            {
                if (value < 1)
                {
                    throw new Exception("看板切割必须大于1行");
                }

                _rowNums = value;
            }
        }

        /// <summary>
        /// 看板切割列数
        /// </summary>
        private int _columnNums;
        /// <summary>
        /// 看板切割列数
        /// </summary>
        public int ColumnNums
        {
            get { return _columnNums; }
            set
            {
                if (value < 1)
                {
                    throw new Exception("看板切割必须大于1列");
                }

                _columnNums = value;
            }
        }

        /// <summary>
        /// 元素配置
        /// 这里只序赋值元素的ID并存储下来
        /// </summary>
        public List<DashboardElementConfig> ElementConfigs { get; set; }

        /// <summary>
        /// 查询的参数列表
        /// JObject 对象。
        /// 类似键值对
        /// </summary>
        public JObject QueryParas { get; set; }

        /// <summary>
        /// 背景图片
        /// </summary>
        public string BackgroundImagePath { get; set; }

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
