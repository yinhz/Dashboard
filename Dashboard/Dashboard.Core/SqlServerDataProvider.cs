using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dashboard
{
    public class SqlServerDataProvider : SqlServerDataProvider<string>
    { }

    public class SqlServerDataProvider<TKey> : IDataProvider<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public int CommandTimeout { get; set; } = 60 * 5;

        public object Query(string command)
        {
            return this.Query(command, null);
        }

        public object Query(string command, Dictionary<string, string> paras)
        {
            if (paras != null
                   && paras.Count() > 0)
            {
                // 目前先简单替换。后面考虑 使用 提供者 模式 解耦。 IParaApplyProvide
                foreach (var para in paras)
                {
                    command = command.Replace("@" + para.Key, para.Value);
                }
            }

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(command, connection);
                cmd.CommandTimeout = this.CommandTimeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
    }
}
