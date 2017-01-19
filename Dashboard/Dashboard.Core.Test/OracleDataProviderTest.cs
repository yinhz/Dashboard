using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dashboard.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string connStr =
                "user id=NGK_COE_DEV2017;password=flxuser;data source=" +
                 "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)" +
                 "(HOST=10.90.247.174)(PORT=1521))(CONNECT_DATA=" +
                 "(SERVICE_NAME=ORCL)))";

            OracleDataProvider dataProvider = new OracleDataProvider("test", connStr, 30);
            var rsl = dataProvider.Query("select 1 from dual;");
        }
    }
}
