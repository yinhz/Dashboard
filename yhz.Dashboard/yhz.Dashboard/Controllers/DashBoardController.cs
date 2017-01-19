using log4net;
using yhz.Dashboard.Common;
using yhz.Dashboard.Core;
using yhz.Dashboard.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace yhz.Dashboard.Controllers
{
    /// <summary>
    /// 看板视图控制器
    /// </summary>
    /// [Authorize]
    public class DashboardController : Controller
    {
        private static ILog m_log = log4net.LogManager.GetLogger(typeof(DashboardController));

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            m_log.Error(filterContext.Exception == null ? "" : filterContext.Exception.Message);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DashboardError(string info)
        {
            return View();
        }

        //public ActionResult DashboardPreView(string themeName)
        public ActionResult DashboardPlayer(string dashboardId, string theme = null, string paras = null)
        {
            // 根据终端 id 取得正在播放的看板
            ViewBag.DashboardConfig = DashboardRepository.DashboardConfigs
                .Where(d => d.DashboardId == dashboardId).FirstOrDefault();

            if (ViewBag.DashboardConfig == null)
            {
                return View("can't find Dashboard by the parameter [dashboardId] which set in url");
            }

            ViewBag.Theme = theme;

            ViewBag.Paras = paras;

            return View();
        }

        public ActionResult TerminalPlayer(string terminalId, int index = 0)
        {
            if (string.IsNullOrEmpty(terminalId))
            {
                return View("must set termainal Id！");
            }

            DashboardTerminalInfo terinfo =
                DashboardRepository.TerminalInfos
                .Where(t => t.TerminalId == terminalId).FirstOrDefault();

            if (terinfo == null)
            {
                terinfo = new DashboardTerminalInfo() { TerminalId = terminalId, Ip = Request.UserHostAddress, IsForbidden = true };

                DashboardRepository.SaveConfig(terinfo);

                DashboardRepository.TerminalInfos
                    .Add(terinfo);
            }

            ViewBag.Index = 0;
            ViewBag.PlayCycle = 1000;
            ViewBag.Paras = "";
            ViewBag.DashboardCount = 0;

            DashboardTerminalInfo targetTerminalInfo = terinfo;

            // 如果 终端没有配置任何看板 同时也没有配置重定向看板，那么则 看板配置为 null
            if ((terinfo.Dashboards == null
                || terinfo.Dashboards.Count == 0) &&
                string.IsNullOrEmpty(terinfo.RedirectTerminalId))
            {
                ViewBag.DashboardConfig = null;
            }
            else
            {
                // 如果存在重定向 terminal
                if (!string.IsNullOrEmpty(terinfo.RedirectTerminalId))
                {
                    targetTerminalInfo = DashboardRepository.TerminalInfos
                        .Where(t => t.TerminalId == terinfo.RedirectTerminalId).FirstOrDefault();
                }

                if (targetTerminalInfo != null && targetTerminalInfo.Dashboards != null && targetTerminalInfo.Dashboards.Count > 0)
                {
                    // 如果索引超出范围 则从 0 开始播放
                    if (index > targetTerminalInfo.Dashboards.Count - 1 || index < 0)
                    {
                        return RedirectToAction("TerminalPlayer", new { terminalId = terminalId, index = 0 });
                    }

                    var srcDashboard = targetTerminalInfo.Dashboards[index];

                    // 根据终端 id 取得正在播放的看板
                    ViewBag.DashboardConfig = DashboardRepository.DashboardConfigs
                        .Where(d => d.DashboardId == targetTerminalInfo.Dashboards[index].DashboardId).FirstOrDefault();

                    ViewBag.Index = index;

                    ViewBag.PlayCycle = srcDashboard.PlayCycle;

                    ViewBag.Paras = srcDashboard.Paras;

                    ViewBag.DashboardCount = targetTerminalInfo.Dashboards.Count;

                    if (string.IsNullOrEmpty(srcDashboard.Theme))
                        ViewBag.Theme = targetTerminalInfo.Theme;
                    else
                        ViewBag.Theme = srcDashboard.Theme;
                }
            }

            ViewBag.TerminalInfo = terinfo;
            ViewBag.TargetTerminalInfo = targetTerminalInfo;

            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult DataConfig()
        {
            if (!Request.IsAuthenticated)
            {

            }

            return View();
        }

        public ActionResult ElementTypeConfig()
        {
            return View();
        }

        public ActionResult ElementConfig()
        {
            return View();
        }

        public ActionResult DashboardConfig()
        {
            return View();
        }

        public ActionResult TerminalInfo()
        {
            return View();
        }

        public ActionResult TerminalView()
        {
            return View();
        }

        public ActionResult DashboardPreView(string dashBoardId)
        {
            ViewBag.DashboardId = dashBoardId;

            return View();
        }

        [ValidateInput(false)]
        public ActionResult EleTypePreView(string configJson = "", string paras = null, string dataId = null)
        {
            var config = ObjectHelper.DeserializeObject<DashboardElementTypeConfig>(configJson);

            config
                .ApplyRandomElementId(System.Guid.NewGuid().ToString().Replace("-", ""))
                .ApplyDataConfig(dataId)
                .ApplyParaConfigs();

            ViewBag.ElementTypeConfig = config;

            ViewBag.Paras = paras;

            return View();
        }
    }
}