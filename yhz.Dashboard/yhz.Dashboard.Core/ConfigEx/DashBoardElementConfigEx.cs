using yhz.Dashboard.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yhz.Dashboard.Core.ConfigEx
{
    /// <summary>
    /// 看板元素配置扩展方法
    /// </summary>
    public static class DashboardElementConfigEx
    {
        ///// <summary>
        ///// html 渲染
        ///// </summary>
        ///// <param name="dbec"></param>
        ///// <returns></returns>
        //public static string RenderHtml(this DashboardElementConfig dbec)
        //{
        //    // 每次都用最新的 元素配置来生成
        //    DashboardElementConfig _dbec = yhz.Dashboard.Core.DashboardRepository.ElementConfigs
        //        .Where(e => e.ElementId == dbec.ElementId).FirstOrDefault();

        //    if (_dbec == null)
        //        throw new Exception(string.Format("无法根据配置的元素ID取得相应元素。元素ID：{0}", dbec.ElementId));

        //    string html = string.Empty;

        //    if (!string.IsNullOrEmpty(_dbec.CssTemplate))
        //    {
        //        html += "<style>\n";
        //        html += "  " + _dbec.CssTemplate;
        //        html += "</style>\n";
        //    }

        //    if (!string.IsNullOrEmpty(_dbec.HtmlTemplate))
        //    {
        //        html += "\n";
        //        html += _dbec.HtmlTemplate;
        //        html += "\n";
        //    }

        //    if (!string.IsNullOrEmpty(_dbec.JavaScriptTemplate))
        //    {
        //        html += "<script>\n";
        //        html += "  " + _dbec.JavaScriptTemplate;
        //        html += "</script>\n";
        //    }

        //    return html;
        //}

        /// <summary>
        /// 2015-07-13 modify
        /// 这里把 elementId 再替换一次
        /// 防止一个页面出现2个相同的元素
        /// 
        /// 2016-1-20 日
        /// 这里把 数据项与元素项解耦
        /// 所以需要在渲染的时候、重新生成下 数据项相关内容
        /// 
        /// </summary>
        /// <param name="dbec"></param>
        /// <returns></returns>
        public static string RenderHtml(DashboardElementConfig dbec)
        {
            // 每次都用最新的 元素配置来生成
            DashboardElementConfig _dbec = yhz.Dashboard.Core.DashboardRepository.ElementConfigs
                .Where(e => e.ElementId == dbec.ElementId).FirstOrDefault();

            if (_dbec == null)
                throw new Exception(string.Format("无法根据配置的元素ID取得相应元素。元素ID：{0}", dbec.ElementId));

            // make random elementid
            _dbec.ApplyRandomElementId()
                .ApplyDataConfig();

            string html = string.Empty;

            if (!string.IsNullOrEmpty(_dbec.CssTemplate))
            {
                html += "<style>\n";
                html += "  " + _dbec.CssTemplate;
                html += "</style>\n";
            }

            if (!string.IsNullOrEmpty(_dbec.HtmlTemplate))
            {
                html += "\n";
                html += _dbec.HtmlTemplate;
                html += "\n";
            }

            if (!string.IsNullOrEmpty(_dbec.JavaScriptTemplate))
            {
                html += "<script>\n";
                html += "  " + _dbec.JavaScriptTemplate;
                html += "</script>\n";
            }

            return html;
        }
    }
}
