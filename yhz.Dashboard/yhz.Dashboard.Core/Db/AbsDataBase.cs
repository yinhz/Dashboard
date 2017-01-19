using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.Db
{
    public enum DatabaseType : byte
    { 
        SqlServer = 0,
        Oracle = 1
    }

    /// <summary>
    /// 数据库
    /// </summary>
    public abstract class AbsDataBase
    {
        public AbsDataBase(string dbName, string connectionString, string extendParas)
        {
            if (dbName == null)
                throw new ArgumentNullException("dbName");

            this.DataBaseName = dbName;

            this.ConnectionString = connectionString;

            this.ExtendParas = extendParas;
        }

        /// <summary>
        /// 数据库链接字符串.可选参数
        /// </summary>
        public virtual string ConnectionString { get; protected set; }

        /// <summary>
        /// 数据库名称
        /// 注意这里是名称、并非类型
        /// 使用的时候拿这个来匹配
        /// 必须提供无参构造函数
        /// </summary>
        public virtual string DataBaseName { get; protected set; }

        /// <summary>
        /// 扩展参数
        /// （留给其他数据库且需要配置用）
        /// </summary>
        public virtual string ExtendParas { get; protected set; }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">sql脚本</param>
        /// <returns>DataSet数据集</returns>
        public abstract object Query(string command);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">sql脚本</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public abstract object Query(string command, JObject paras);
    }
}
