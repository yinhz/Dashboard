using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.Db
{
    /// <summary>
    /// sql server 数据库
    /// </summary>
    public class SqlServerDb : AbsDataBase
    {
        /// <summary>
        /// sqlserver 构造函数
        /// </summary>
        /// <param name="dbName">数据库名称（必填）</param>
        /// <param name="connectionString">链接字符串（可为空）</param>
        /// <param name="extendParas">扩展参数（可为空）</param>
        public SqlServerDb(string dbName, string connectionString, string extendParas)
            : base(dbName, connectionString, extendParas)
        {

        }

        public override object Query(string command)
        {
            using (SqlConnection connection = new SqlConnection(base.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(command, connection);
                cmd.CommandTimeout = 60 * 5;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public override object Query(string command, Newtonsoft.Json.Linq.JObject paras)
        {
            if (paras == null
                || !paras.HasValues)
                return this.Query(command);

            // 目前先简单替换。后面考虑 使用 提供者 模式 解耦。 IParaApplyProvide
            foreach (var para in paras)
            {
                command = command.Replace("@" + para.Key, para.Value.ToString());
            }

            return this.Query(command);
        }
    }
}
