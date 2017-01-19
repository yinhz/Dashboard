using Newtonsoft.Json.Linq;
using yhz.Dashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAUtility.OAuth;
using UAUtility.OData;

namespace yhz.Dashboard.Controllers
{
    public class UADashboardRuntimeController : Controller
    {
        public UADashboardRuntimeController()
        {
            // 获取 UA 开放认证服务
            this.uaRequestService = UARequestServiceFactory.GetService();
        }
        /// <summary>
        /// UA 开放认证服务接口
        /// </summary>
        private IUARequestService uaRequestService;

        private string ParaReplace(string str, string newElementId, string dataId, string isAutoRefresh, string interval, JArray paras)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            str = str
                .Replace("[ElementId]", string.IsNullOrEmpty(newElementId) ? "" : newElementId)
                    .Replace("[DataId]", string.IsNullOrEmpty(dataId) ? "" : dataId)
                    .Replace("[IsAutoRefresh]", string.IsNullOrEmpty(isAutoRefresh) ? "" : isAutoRefresh)
                    .Replace("[Interval]", string.IsNullOrEmpty(interval) ? "" : interval);

            if (paras != null)
            {
                foreach (JObject item in paras)
                {
                    str = str.Replace(
                        "[" + item["Code"] + "]", item["Value"].Value<string>()
                        );
                }
            }

            return str;
        }

        public ActionResult ElementTypePreview(string elementTypeId, string datasourceId, string parameterJson = "")
        {
            var elementType =
                ((JObject.Parse(
                    this.uaRequestService.EntityQuery(
                        "ElementType",
                        "$select=HtmlTemplate,CssTemplate,JavaScriptTemplate&$expand=Parameters&$filter=Id eq " + elementTypeId)
                ))["value"] as JArray)[0] as JObject;

            var datasource =
                ((JObject.Parse(
                    this.uaRequestService.EntityQuery(
                        "Datasource",
                        "$select=Interval,AutoRefresh&$filter=Id eq " + datasourceId)
                ))["value"] as JArray)[0] as JObject;

            string dataId = string.Empty, interval = string.Empty, autoRefresh = string.Empty;

            if (!string.IsNullOrEmpty(datasourceId))
            {
                dataId = datasourceId;
                interval = datasource["Interval"].Value<int>().ToString();
                autoRefresh = datasource["AutoRefresh"].Value<bool>().ToString().ToLower();
            }

            string newElementId = System.Guid.NewGuid().ToString().Replace("-", "");

            JArray paras = elementType["Parameters"] as JArray;

            ViewBag.HtmlTemplate = this.ParaReplace(elementType["HtmlTemplate"].Value<string>(), newElementId, dataId, autoRefresh.ToString().ToLower(), interval.ToString(), paras);
            ViewBag.CssTemplate = this.ParaReplace(elementType["CssTemplate"].Value<string>(), newElementId, dataId, autoRefresh.ToString().ToLower(), interval.ToString(), paras);
            ViewBag.JavaScriptTemplate = this.ParaReplace(elementType["JavaScriptTemplate"].Value<string>(), newElementId, dataId, autoRefresh.ToString().ToLower(), interval.ToString(), paras);

            ViewBag.ParameterJson = parameterJson;

            return View("ElementPreview");
        }

        public ActionResult ElementPreview(string elementId, string parameterJson)
        {
            var element =
                ((JObject.Parse(
                    this.uaRequestService.EntityQuery(
                        "Element",
                        "$select=HtmlTemplate,CssTemplate,JavaScriptTemplate,Datasource,Datasource_Id&$expand=Datasource&$filter=Id eq " + elementId)
                ))["value"] as JArray)[0] as JObject;

            string datasourceId =   string.Empty;
            string interval     =   string.Empty;
            string autoRefresh  =   string.Empty;

            if (element["Datasource_Id"].Value<string>() != null)
            {
                datasourceId = element["Datasource_Id"].Value<string>();
                interval = element["Datasource"]["Interval"].Value<int>().ToString();
                autoRefresh = element["Datasource"]["AutoRefresh"].Value<bool>().ToString().ToLower();
            }

            string newElementId = System.Guid.NewGuid().ToString().Replace("-", "");

            ViewBag.HtmlTemplate = this.ParaReplace(element["HtmlTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
            ViewBag.CssTemplate = this.ParaReplace(element["CssTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
            ViewBag.JavaScriptTemplate = this.ParaReplace(element["JavaScriptTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);

            ViewBag.ParameterJson = parameterJson;

            return View("ElementPreview");
        }

        public ActionResult DashboardPlay(string dashboardId, string parameterJson = "", string theme = "default")
        {
            var dashboard =
                ((JObject.Parse(
                    this.uaRequestService.EntityQuery(
                        "Dashboard",
                        "$expand=DashboardElements&$filter=Id eq " + dashboardId)
                ))["value"] as JArray)[0] as JObject;

            // 生成各个 html
            foreach (JObject dashboardElement in (dashboard["DashboardElements"] as JArray))
            {
                var element =
                    ((JObject.Parse(
                        this.uaRequestService.EntityQuery(
                            "Element",
                            "$select=HtmlTemplate,CssTemplate,JavaScriptTemplate,Datasource,Datasource_Id&$expand=Datasource&$filter=Id eq "
                            + dashboardElement["Element_Id"].Value<string>())
                    ))["value"] as JArray)[0] as JObject;

                string datasourceId = string.Empty;
                string interval = string.Empty;
                string autoRefresh = string.Empty;

                if (element["Datasource_Id"].Value<string>() != null)
                {
                    datasourceId = element["Datasource_Id"].Value<string>();
                    interval = element["Datasource"]["Interval"].Value<int>().ToString();
                    autoRefresh = element["Datasource"]["AutoRefresh"].Value<bool>().ToString().ToLower();
                }

                string newElementId = System.Guid.NewGuid().ToString().Replace("-", "");

                dashboardElement["Html"] = this.ParaReplace(element["HtmlTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
                dashboardElement["Css"] = this.ParaReplace(element["CssTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
                dashboardElement["JavaScript"] = this.ParaReplace(element["JavaScriptTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
            }

            ViewBag.Dashboard = (dynamic)dashboard;

            ViewBag.Theme = theme;
            ViewBag.ParameterJson = parameterJson;

            ViewBag.BackgroundImage = "";

            return View();
        }

        [HttpGet]
        public bool IsRefreshTerminal(string terminalId, string lastEditTime)
        {
            var terminal =
                ((JObject.Parse(
                    this.uaRequestService.EntityQuery(
                        "Terminal",
                        "$select=LastUpdatedOn&$filter=Identification eq " + "'" + terminalId + "'")
                ))["value"] as JArray)[0] as JObject;

            //if (terminal == null)
            //{
            //    throw new Exception("不存在的终端！");
            //}

            // 不相等就更新把 不管大小了
            if (DateTime.Parse(lastEditTime) != (DateTime)terminal["LastUpdatedOn"])
                return true;

            return false;
        }

        public ActionResult TerminalPlay(string terminalId, int index = 0)
        {
            if (string.IsNullOrEmpty(terminalId))
            {
                return View("must set termainal Id！");
            }

            var terminal =
                ((JObject.Parse(
                    this.uaRequestService.EntityQuery(
                        "Terminal",
                        "$expand=TerminalDashboards&$filter=Identification eq " + "'" + terminalId + "'")
                ))["value"] as JArray)[0] as JObject;

            //&$filter=Identification eq " + terminalId

            ViewBag.Terminal = (dynamic)terminal;

            var terminalDashboards = (terminal["TerminalDashboards"] as JArray).OrderBy(t => t["PlayOrder"].Value<string>()).ToList();

            if (terminal["TerminalDashboards"] == null
                || terminal["TerminalDashboards"].Count() == 0)
            {
                ViewBag.TerminalDashboard = null;
            }
            else
            {
                // 如果索引超出范围 则从 0 开始播放
                if (index > terminalDashboards.Count - 1)
                    index = 0;

                ViewBag.TerminalDashboard = terminalDashboards[index];
                ViewBag.Theme = terminalDashboards[index]["Theme"].Value<string>();
                ViewBag.ParameterJson = terminalDashboards[index]["ParameterJson"].Value<string>();

                var dashboard =
                    ((JObject.Parse(
                        this.uaRequestService.EntityQuery(
                            "Dashboard",
                            "$expand=DashboardElements&$filter=Id eq " + ViewBag.TerminalDashboard.Dashboard_Id)
                    ))["value"] as JArray)[0] as JObject;

                // 生成各个 html
                foreach (JObject dashboardElement in (dashboard["DashboardElements"] as JArray))
                {
                    var element =
                        ((JObject.Parse(
                            this.uaRequestService.EntityQuery(
                                "Element",
                                "$select=HtmlTemplate,CssTemplate,JavaScriptTemplate,Datasource,Datasource_Id&$expand=Datasource&$filter=Id eq "
                                + dashboardElement["Element_Id"].Value<string>())
                        ))["value"] as JArray)[0] as JObject;

                    string datasourceId = string.Empty;
                    string interval = string.Empty;
                    string autoRefresh = string.Empty;

                    if (element["Datasource_Id"].Value<string>() != null)
                    {
                        datasourceId = element["Datasource_Id"].Value<string>();
                        interval = element["Datasource"]["Interval"].Value<int>().ToString();
                        autoRefresh = element["Datasource"]["AutoRefresh"].Value<bool>().ToString().ToLower();
                    }

                    string newElementId = System.Guid.NewGuid().ToString().Replace("-", "");

                    dashboardElement["Html"] = this.ParaReplace(element["HtmlTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
                    dashboardElement["Css"] = this.ParaReplace(element["CssTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
                    dashboardElement["JavaScript"] = this.ParaReplace(element["JavaScriptTemplate"].Value<string>(), newElementId, datasourceId, autoRefresh, interval, null);
                }

                ViewBag.TerminalDashboard.RowNumber = dashboard["RowNumber"].Value<int>();
                ViewBag.TerminalDashboard.ColumnNumber = dashboard["ColumnNumber"].Value<int>();

                ViewBag.TerminalDashboard.DashboardElements = (dynamic)dashboard["DashboardElements"];

                ViewBag.Index = index;

                ViewBag.DashboardCount = terminalDashboards.Count;
            }

            return View();
        }
    }
}