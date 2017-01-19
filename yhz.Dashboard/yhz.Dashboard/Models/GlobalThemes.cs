using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace yhz.Dashboard.Models
{
    /// <summary>
    /// 全局的样式
    /// </summary>
    public static class GlobalThemes
    {
        /// <summary>
        /// 全局的样式
        /// </summary>
        static GlobalThemes()
        {
            // 读取 content themes 下的文件夹
            // 有多少个文件夹 代表 有多少套风格
           
            //Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory)
        }

        /// <summary>
        /// 全局样式
        /// key:样式名称
        /// value:路径
        /// </summary>
        public static Dictionary<string, string> Themes { get; set; }

        public static string CurrentTheme { get; set; }
    }
}