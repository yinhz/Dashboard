using yhz.Dashboard.Core.Data;
using System;
using System.Linq;

namespace yhz.Dashboard.Core.DataEx
{
    /// <summary>
    /// 查询模型扩展方法
    /// </summary>
    public static class QueryModelEx
    {
        /// <summary>
        /// 获取sql
        /// </summary>
        /// <param name="qm"></param>
        /// <returns></returns>
        public static string GetSql(this QueryModel qm)
        {
            if (string.IsNullOrEmpty(qm.DataId))
            {
                throw new Exception("无法取得数据项的标识！");
            }

            var dc = yhz.Dashboard.Core.DashBoardRepository.DataConfigs
                .Where(d => d.DataId == qm.DataId).FirstOrDefault();

            if (dc == null)
            {
                throw new Exception("无法根据给定的数据项标识取得数据配置！");
            }

            if (qm.QueryMode == QueryMode.Formal)
            {
                return dc.Sql;
            }
            else if (qm.QueryMode == QueryMode.Preview)
            {
                return dc.PreSql;
            }

            throw new Exception("无法取得Sql");
        }
    }
}