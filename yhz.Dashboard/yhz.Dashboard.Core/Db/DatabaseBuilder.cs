using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.Db
{
    public static class DatabaseBuilder
    {
        public static AbsDataBase Build(DatabaseType dbType, string dbName, string connectionString, string extendParas)
        {
            switch (dbType) 
            {
                case DatabaseType.SqlServer:
                    return new SqlServerDb(dbName, connectionString, extendParas);
                case DatabaseType.Oracle:
                    return new OracleDb(dbName, connectionString, extendParas);
                default: throw new Exception("unsupport database type");
            }
        }
    }
}
