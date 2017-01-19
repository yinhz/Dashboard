using yhz.Dashboard.Common;
using yhz.Dashboard.Core.Config;
using yhz.Dashboard.Core.Db;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace yhz.Dashboard.Core
{
    public static class DashboardRepository1
    {
        public const string CONST_DATABASE_CONFIG_SECTION_NAME = "DataBaseCfgSection";
        public const string CONST_ELEMENT_CONFIG_FOLDER = "ElementConfigFolder";
        public const string CONST_DATA_CONFIG_FOLDER = "DataConfigFolder";
        public const string CONST_DASHBOARD_CONFIG_FOLDER = "DashboardConfigFolder";
        public const string CONST_ELEMENTTYPE_CONFIG_FOLDER = "ElementTypeConfigFolder";
        public const string CONST_TERMINALINFO_FOLDER = "TerminalInfoFolder";

        public static readonly object LockObj = new object();

        static DashboardRepository1()
        {
            // 读取配置文件。加载 所有数据库。数据库这里必须重启服务
            DataBasies = (List<AbsDataBase>)System.Configuration.ConfigurationManager.GetSection("DataBaseCfgSection");

            ReadConfig();
        }

        #region 各种配置相关（元素配置、数据配置、元素类型配置、看板配置、终端配置）

        /// <summary>
        /// 刷新数据用
        /// </summary>
        public static void ReadConfig()
        {
            lock (LockObj)
            {
                ElementConfigs = GetAllConfigs<DashboardElementConfig>(DashboardRepository.CONST_ELEMENT_CONFIG_FOLDER);

                DataConfigs = GetAllConfigs<DashboardDataConfig>(DashboardRepository.CONST_DATA_CONFIG_FOLDER);

                DashboardConfigs = GetAllConfigs<DashboardConfig>(DashboardRepository.CONST_DASHBOARD_CONFIG_FOLDER);

                ElementTypeConfigs = GetAllConfigs<DashboardElementTypeConfig>(DashboardRepository.CONST_ELEMENTTYPE_CONFIG_FOLDER);

                TerminalInfos = GetAllConfigs<DashboardTerminalInfo>(DashboardRepository.CONST_TERMINALINFO_FOLDER);
            }
        }

        /// <summary>
        /// 所有的数据库配置
        /// </summary>
        public static List<AbsDataBase> DataBasies { get; private set; }

        public static List<string> DataBaseNames
        {
            get
            {
                if (DataBasies == null
                    || DataBasies.Count == 0)
                    return new List<string>();

                return DataBasies.Select(d => d.DataBaseName).ToList();
            }
        }

        public static List<DashboardElementConfig> ElementConfigs
        {
            get;
            private set;
        }

        /// <summary>
        /// 看板数据配置集合
        /// </summary>
        public static List<DashboardDataConfig> DataConfigs
        {
            get;
            private set;
        }

        public static List<DashboardConfig> DashboardConfigs
        {
            get;
            private set;
        }

        public static List<DashboardElementTypeConfig> ElementTypeConfigs
        {
            get;
            private set;
        }

        public static List<DashboardTerminalInfo> TerminalInfos
        {
            get;
            private set;
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 根据指定配置文件中节点的名称
        /// 获取路径下所有 json 文件内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>返回所有json文件反序列化后的集合</returns>
        private static string GetAllConfigsJson<T>(string name)
        {
            return ObjectHelper.SerializeObject(GetAllConfigs<T>(name));
        }

        private static List<T> GetAllConfigs<T>(string name)
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings[name];

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            List<T> dashBoardDataConfigs = new List<T>();

            var files = Directory.GetFiles(folder, "*.json", SearchOption.TopDirectoryOnly);

            if (files == null
                || files.Count() == 0)
                goto ExitHere;

            foreach (var file in files)
            {
                object obj = ObjectHelper.JsonFile2Object(file);

                if (obj == null || !(obj is T))
                    continue;

                dashBoardDataConfigs.Add((T)obj);
            }

        ExitHere:

            return dashBoardDataConfigs;
        }

        #endregion

        #region 配置操作相关（保存、新增）

        private static void InsureFolderExists(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public static void SaveConfig(IPersistenceConfig obj)
        {
            if (obj is DashboardDataConfig)
            {
                SaveDataConfig(ObjectHelper.SerializeObject(obj), (obj as DashboardDataConfig).DataId);
            }
            else if (obj is DashboardConfig)
            {
                SaveDashboardConfig(ObjectHelper.SerializeObject(obj), (obj as DashboardConfig).DashboardId);
            }
            else if (obj is DashboardElementTypeConfig)
            {
                SaveElementTypeConfig(ObjectHelper.SerializeObject(obj), (obj as DashboardElementTypeConfig).TypeId);
            }
            else if (obj is DashboardTerminalInfo)
            {
                SaveTerminalInfo(ObjectHelper.SerializeObject(obj), (obj as DashboardTerminalInfo).TerminalId);
            }
            else if (obj is DashboardElementConfig)
            {
                SaveElementConfig(ObjectHelper.SerializeObject(obj), (obj as DashboardElementConfig).ElementId);
            }
        }

        public static void SaveDataConfig(string dataConfigJson, string Id)
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings[DashboardRepository.CONST_DATA_CONFIG_FOLDER];

            InsureFolderExists(folder);

            File.WriteAllText(
                Path.Combine(folder, Id + ".json"), dataConfigJson, Encoding.UTF8);
        }

        public static void SaveElementTypeConfig(string elementTypeConfigJson, string Id)
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings[DashboardRepository.CONST_ELEMENTTYPE_CONFIG_FOLDER];

            InsureFolderExists(folder);

            File.WriteAllText(
                Path.Combine(folder, Id + ".json"), elementTypeConfigJson, Encoding.UTF8);
        }

        public static void SaveElementConfig(string elementConfigJson, string Id)
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings[DashboardRepository.CONST_ELEMENT_CONFIG_FOLDER];

            InsureFolderExists(folder);

            File.WriteAllText(
                Path.Combine(folder, Id + ".json"), elementConfigJson, Encoding.UTF8);
        }

        public static void SaveDashboardConfig(string dashBoardConfigJson, string Id)
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings[DashboardRepository.CONST_DASHBOARD_CONFIG_FOLDER];

            InsureFolderExists(folder);

            File.WriteAllText(Path.Combine(folder, Id + ".json"), dashBoardConfigJson, Encoding.UTF8);
        }

        public static void SaveTerminalInfo(string terminalInfoJson, string Id)
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings[DashboardRepository.CONST_TERMINALINFO_FOLDER];

            InsureFolderExists(folder);

            File.WriteAllText(Path.Combine(folder, Id + ".json"), terminalInfoJson, Encoding.UTF8);
        }

        #endregion
    }
}
