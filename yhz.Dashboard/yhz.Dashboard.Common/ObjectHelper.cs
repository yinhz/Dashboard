using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace yhz.Dashboard.Common
{
    public static class ObjectHelper
    {
        public static string SerializeObject(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return Newtonsoft.Json.JsonConvert.SerializeObject(
                obj,
                Newtonsoft.Json.Formatting.Indented,
                ObjectHelper.DefaultJsonSerializerSettings);
        }

        public static void ObjectJson2File(string filePath, object obj)
        {
            if (obj.IsNull())
                throw new ArgumentNullException("obj");

            string file_folder = filePath.Substring(0, filePath.LastIndexOf("\\"));

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(file_folder);
            }

            File.WriteAllText(filePath, ObjectHelper.SerializeObject(obj), Encoding.UTF8);
        }

        public static object JsonFile2Object(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string json = File.ReadAllText(filePath, Encoding.UTF8);

            return ObjectHelper.DeserializeObject(json);
        }

        public static T JsonFile2Object<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string json = File.ReadAllText(filePath, Encoding.UTF8);

            return ObjectHelper.DeserializeObject<T>(json);
        }

        /// <summary>
        /// json 序列化默认设置
        /// </summary>
        public static JsonSerializerSettings DefaultJsonSerializerSettings
        {
            get
            {
                return new Newtonsoft.Json.JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects,
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };
            }
        }

        public static object DeserializeObject(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("json");

            return Newtonsoft.Json.JsonConvert.DeserializeObject(
                json,
                ObjectHelper.DefaultJsonSerializerSettings
                );
        }

        public static T DeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("json");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(
                json,
                ObjectHelper.DefaultJsonSerializerSettings
                );
        }

        public static T Clone<T>(T obj)
        {
            return obj;
        }

        public static bool IsNull(this object obj)
        {
            if (obj == null)
                return true;

            if (obj is string)
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                    return true;
            }

            return false;
        }

        public static bool IsNotNull(this object obj)
        {
            return !ObjectHelper.IsNull(obj);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        public static object CreateInstance(string assemblyName, string typeName)
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName == assemblyName)
                .FirstOrDefault();

            if (ass == null)
            {
                throw new Exception(
                       string.Format("未能取得程序集{0}", assemblyName));
            }

            return ass.CreateInstance(typeName);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string typeName)
        {
            var obj = CreateInstance(assemblyName, typeName);

            if (obj is T)
            {
                return (T)obj;
            }

            throw new Exception(
                string.Format("无法从程序集{0},创建类型{1}", assemblyName, typeName));
        }
    }
}
