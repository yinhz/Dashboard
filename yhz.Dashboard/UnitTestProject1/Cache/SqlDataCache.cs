using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1.Cache
{
    public static class SqlDataCache
    {
        private static readonly int MaxCacheCount = 1000;

        static SqlDataCache()
        {
            m_cache_datas = new Dictionary<SqlDataDescription, object>();
            m_lock_obj = (m_cache_datas as ICollection).SyncRoot;
        }

        public static Dictionary<SqlDataDescription, object> m_cache_datas;
        private static object m_lock_obj;

        public static void AddtoCache(SqlDataDescription dataDes, object result)
        {
            // set query time to Now.
            dataDes.QueryTime = DateTime.Now;

            lock (m_lock_obj)
            {
                m_cache_datas.Add(dataDes, result);
            }
        }

        public static bool TryGetFromCache(SqlDataDescription newDataDes, out object result)
        {
            // 如果超过最大缓存数
            if (m_cache_datas.Count > MaxCacheCount)
            {
                // 移除第一个
                m_cache_datas.Remove(m_cache_datas.First().Key);

                result = null;
                return false ;
            }

            var oldDataDes = m_cache_datas.Keys
                .Where(o => o.Sql.Equals(newDataDes.Sql)
                && o.Paras.Equals(newDataDes.Paras)).FirstOrDefault();

            // 如果没找到相关的 key 
            if (oldDataDes == null)
            {
                result = null;
                return false;
            }

            // 或者缓存的时间超过了对象查询的时间
            if ((newDataDes.QueryTime - oldDataDes.QueryTime) > TimeSpan.FromSeconds(newDataDes.Interval))
            {
                lock (m_lock_obj)
                {
                    m_cache_datas.Remove(oldDataDes);
                }

                result = null;
                return false;
            }

            result = m_cache_datas[oldDataDes];

            lock (m_lock_obj)
            {
                m_cache_datas.Remove(oldDataDes);
                m_cache_datas.Add(newDataDes, result);
            }

            return true;
        }
    }
}