using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Data;

namespace yhz.Dashboard.Core.Db
{
    public class OracleDb : AbsDataBase
    {
        public OracleDb(string dbName, string connectionString, string extendParas)
            : base(dbName, connectionString, extendParas)
        {

        }

        public override object Query(string command)
        {
            using (OracleConnection connection = new OracleConnection(base.ConnectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand(command, connection);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
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
