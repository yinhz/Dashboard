using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1.Cache;

namespace UnitTestProject1
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void BeginTest()
        {
            int interval = 5;

            // 首先进行 100 次独立的查询
            for (int i = 0; i < 10000; i++)
            {
                if (interval >= 10)
                    interval = 10;
                
                string paras = "{\"index\":\"" + i.ToString() + "\"}";

                var dataDes = new SqlDataDescription(sqlbase + i, interval, JObject.Parse(paras));

                object result = QueryData(dataDes);

                interval++;

                DebugInfo(
                        JObject.FromObject(result).ToString()
                    );
            }

            DateTime dt = DateTime.Now;

            SqlDataCache.m_cache_datas.Keys.Where(
                k => k.Sql == sqlbase + "9999").FirstOrDefault();

            System.Diagnostics.Debug.WriteLine((DateTime.Now - dt).TotalMilliseconds);
        }

        private void DebugInfo(string txt)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToLocalTime() + " " + txt);
        }

        public object QueryData(SqlDataDescription dataDes)
        {
            object result;
            var sqlDataDescription = new SqlDataDescription(
                dataDes.Sql,
                dataDes.Interval,
                dataDes.Paras);

            // 首先看缓存里是否有
            if (SqlDataCache.TryGetFromCache(sqlDataDescription, out result))
            {
                System.Diagnostics.Debug.WriteLine(
                    JObject.FromObject(sqlDataDescription).ToString()
                    + "  rsl: "
                    + JObject.FromObject(result)
                    );

                return result;
            }

            // 没有择取数据库查询
            result = QueryDatabase(dataDes);

            // 插入缓存
            SqlDataCache.AddtoCache(dataDes, result);

            return result;
        }

        public object QueryDatabase(SqlDataDescription dataDes)
        {
            return dataDes;
        }

        public string sqlbase =
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "select 下次vasdfg阿斯再度发生地方啊上帝发送为对方娃儿福" +
                    "娃前额发撒旦法撒旦法撒旦法撒旦法撒旦法撒大夫撒旦";
    }
}
