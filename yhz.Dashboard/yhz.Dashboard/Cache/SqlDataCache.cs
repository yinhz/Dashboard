using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace yhz.Dashboard.Cache
{
    public class SqlDataCache
    {
        private static readonly int MaxCacheCount =
            Convert.ToInt32(
                ConfigurationManager.AppSettings["SqlDataCacheCount"]
            );

        public SqlDataCache()
        {
            m_cache_datas = new Dictionary<SqlDataDescription, object>();
            m_lock_obj = (m_cache_datas as ICollection).SyncRoot;
        }

        private Dictionary<SqlDataDescription, object> m_cache_datas;
        private object m_lock_obj;

        public void AddtoCache(SqlDataDescription dataDes, object result)
        {
            lock (m_lock_obj)
            {
                // 由于查询是多线程进行的
                // 例如 在查询被缓存之前存在 并发的5次查询
                // 可能导致 同时缓存了 5次，这里需要解决这个问题
                var existsDataDes = m_cache_datas.Keys
                    .Where(o =>
                        o.DatabaseName == dataDes.DatabaseName
                        && o.Sql == dataDes.Sql
                        && o.Paras == dataDes.Paras).FirstOrDefault();

                // 如果已存在的缓存的查询时间 比 当前要进行缓存的对象的查询时间还晚
                // 那么则不用进行缓存了

                // 这样做 保证了 同样的 查询描述 在缓存中只有一个

                // 如果存在相同的数据描述
                if (existsDataDes != null)
                {
                    // 并且 已存在的 查询时间 < 现有查询时间，则需要 移除老的缓存
                    if (existsDataDes.QueryTime < dataDes.QueryTime)
                        m_cache_datas.Remove(existsDataDes);
                    // 否则 新的查询结果直接不需要缓存
                    else
                        return;
                }

                m_cache_datas.Add(dataDes, result);
            }
        }

        public bool TryGetFromCache(SqlDataDescription newDataDes, out object result)
        {
            lock (m_lock_obj)
            {
                // 如果超过最大缓存数
                if (m_cache_datas.Count > MaxCacheCount)
                {
                    // 移除第一个
                    m_cache_datas.Remove(m_cache_datas.First().Key);

                    result = null;
                    return false;
                }

                var oldDataDes = m_cache_datas.Keys
                    .Where(o =>
                        o.DatabaseName == newDataDes.DatabaseName
                        && o.Sql == newDataDes.Sql
                        && o.Paras == newDataDes.Paras).FirstOrDefault();

                // 如果没找到相关的 key 
                if (oldDataDes == null)
                {
                    result = null;
                    return false;
                }

                // 或者缓存的时间超过了对象查询的时间，则需要移除缓存
                if (
                    (newDataDes.QueryTime - oldDataDes.QueryTime) 
                    > 
                    TimeSpan.FromMilliseconds(newDataDes.Interval)
                    )
                {

                    m_cache_datas.Remove(oldDataDes);

                    result = null;
                    return false;
                }

                // 返回结果
                result = m_cache_datas[oldDataDes];

                // 这里是个bug ，注释掉代码，因为 需要保持老的缓存在
                // 如果按 下面代码做的话，时间总是被更新成最新的了
                // m_cache_datas.Remove(oldDataDes);
                // m_cache_datas.Add(newDataDes, result);

                return true;
            }
        }
    }
}