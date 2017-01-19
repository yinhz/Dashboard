using yhz.Dashboard.Core.Db;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using yhz.Dashboard.Common;
using System;
using System.Linq;

namespace yhz.Dashboard.Core.CfgSection
{
    public class DataBaseCfgSectionHandler : IConfigurationSectionHandler
    {
        public DataBaseCfgSectionHandler()
        { }

        #region IConfigurationSectionHandler 成员

        /// <summary>
        /// 在获取节点时所执行的方法
        /// 例如(List<AbsDataBase>)ConfigurationSettings.GetConfig("DashboardDataBaseConfig");
        /// 此为虚方法、可重写
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public virtual object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // 获取所有的程序集
            var all_assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<AbsDataBase> dbs = new List<AbsDataBase>();

            foreach (XmlNode db in section.ChildNodes)
            {
                if (db.IsNullOrEmptyNode())
                    continue;

                if (db.IsNullOrEmptyCNode("DataBaseName"))
                    throw new Exception("无法取得 DataBaseName 的配置！");

                if (db.IsNullOrEmptyCNode("DataBaseType"))
                    throw new Exception("无法取得 DataBaseType 的配置！");

                var dataBaseType = db.SelectSingleNode("DataBaseType").InnerText;

                if (dataBaseType.Split(new char[] { ',' }).Length != 2)
                    throw new Exception("DataBaseType 配置错误！");

                var dataBaseName = db.SelectSingleNode("DataBaseName").InnerText;

                string connectionString = string.Empty;

                if (db.SelectSingleNode("ConnectionString") != null)
                    connectionString = db.SelectSingleNode("ConnectionString").InnerText;

                var typeName = dataBaseType.Split(new char[] { ',' })[0];
                var assemblyName = dataBaseType.Split(new char[] { ',' })[1];

                string extParas = string.Empty;

                if (db.SelectSingleNode("ExtendParas") != null)
                    extParas = db.SelectSingleNode("ExtendParas").InnerText;

                var ass = all_assemblies
                    .Where(a => a.GetName().Name == assemblyName).FirstOrDefault();

                if (ass == null)
                    throw new Exception(
                        string.Format("无法取得程序集{0}", assemblyName));

                try
                {
                    var _dataBase = ass.CreateInstance(typeName, false, System.Reflection.BindingFlags.CreateInstance,
                        null, new object[] { dataBaseName, connectionString, extParas }, null, null);

                    if (_dataBase is AbsDataBase)
                    {
                        dbs.Add(_dataBase as AbsDataBase);
                    }
                    else
                    {
                        throw new Exception(
                            string.Format("指定的数据库类型必须为 AbsDataBase 子类！程序集：{0},类型:{1},数据库名称：{2},链接字符串{3}",
                            assemblyName, typeName, dataBaseName, connectionString));
                    }
                }
                catch (Exception e) {
                    throw new Exception(
                        string.Format("初始化数据库出错！程序集：{0},类型:{1},数据库名称：{2},链接字符串{3}。异常信息：{4}",
                        assemblyName, typeName, dataBaseName, connectionString, e.Message));
                }
            }

            return dbs;
        }

        #endregion
    }
}
