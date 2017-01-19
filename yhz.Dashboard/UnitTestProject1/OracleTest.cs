using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class OracleTest
    {
        public yhz.Dashboard.Core.Db.AbsDataBase
            oracledb;

        [TestInitialize]
        public void init()
        {
            oracledb = new yhz.Dashboard.Core.Db.OracleDb("orcl", "Data Source=192.168.95.131/sitmesdb;User Id=system;Password=sitmesdb;Integrated Security=no", "");
        }
        [TestMethod]
        public void test()
        {
            log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());

            log.Info("1");
            log.Error("123");

            //oracledb.Query("asdf");
        }
    }
}
