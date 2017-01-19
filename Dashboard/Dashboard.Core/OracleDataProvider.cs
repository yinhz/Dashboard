using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Dashboard
{
    public class OracleDataProvider : OracleDataProvider<string>
    {
        public OracleDataProvider()
        {

        }

        public OracleDataProvider(string name, string connnectionString, int commandTimeout)
        {
            base.Name = name;
            base.ConnectionString = connnectionString;
            base.CommandTimeout = commandTimeout;
        }
    }

    public class OracleDataProvider<TKey> : IDataProvider<TKey>
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
                foreach (var para in paras)
                {
                    command = command.Replace("@" + para.Key, para.Value.ToString());
                }
            }

            using (OracleConnection connection = new OracleConnection(this.ConnectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand(command, connection);
                cmd.CommandTimeout = this.CommandTimeout;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
    }
}
