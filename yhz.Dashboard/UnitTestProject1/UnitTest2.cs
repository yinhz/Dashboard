using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using yhz.Dashboard.Common;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("WorkShopId", "1");
            dic.Add("LineId", "2");

            XmlNode xn = null;
            xn.IsNullOrEmptyCNode("1");
            //var str = ObjectHelper.SerializeObject(dic);

            //JObject j = new JObject();
            //j["WorkShopId"] = "1";
            //j["LineId"] = "2";
            //j["abc"] = 3.4;

            //var str1 = j.ToString();

            //foreach (var item in j)
            //{
                
            //}
        }
    }
}
