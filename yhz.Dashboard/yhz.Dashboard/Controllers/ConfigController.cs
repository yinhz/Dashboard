using Newtonsoft.Json.Linq;
using yhz.Dashboard.Common;
using yhz.Dashboard.Core;
using yhz.Dashboard.Core.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;

namespace yhz.Dashboard.Controllers
{
    /// <summary>
    /// 看板整体配置控制器
    /// </summary>
    public class ConfigController : ApiController
    {
        private void ThrowHttpException(string message, string ReasonPhrase = null)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(message),
                ReasonPhrase = ReasonPhrase
            };

            throw new HttpResponseException(resp);
        }

        #region 各种配置删除、克隆

        [HttpGet]
        public bool DeleteDataConfig(string id)
        {
            return yhz.Dashboard.Core.DashboardRepository.DeleteDataConfig(id);
        }
        [HttpGet]
        public DashboardDataConfig CloneDataConfig(string id)
        {
            var config = yhz.Dashboard.Core.DashboardRepository.DataConfigs
                .Where(e => e.DataId == id).FirstOrDefault();

            if (config == null)
            {
                this.ThrowHttpException("Count find the Element Type by given ID");
            }

            var newConfig = ObjectHelper.Clone<DashboardDataConfig>(config);

            newConfig.DataId = System.Guid.NewGuid().ToString().Replace("-", "");
            newConfig.CreateTime = DateTime.Now;
            newConfig.DataName = newConfig.DataName + " (Copy)";

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(newConfig);

            //yhz.Dashboard.Core.DashboardRepository.DataConfigs.Insert(0,
            //    newConfig
            //    );

            return newConfig;
        }
        [HttpGet]
        public bool DeleteElementTypeConfig(string id)
        {
            return yhz.Dashboard.Core.DashboardRepository.DeleteElementTypeConfig(id);
        }
        [HttpGet]
        public DashboardElementTypeConfig CloneElementTypeConfig(string id)
        {
            var config = yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs
                .Where(e => e.TypeId == id).FirstOrDefault();

            if (config == null)
            {
                this.ThrowHttpException("无法根据指定的标识取得相应的元素类型！");
            }

            var newConfig = ObjectHelper.Clone<DashboardElementTypeConfig>(config);

            newConfig.TypeId = System.Guid.NewGuid().ToString().Replace("-", "");
            newConfig.CreateTime = DateTime.Now;
            newConfig.TypeName = newConfig.TypeName + " (Copy)";

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(newConfig);

            //yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs.Insert(0,
            //    newConfig
            //    );

            return newConfig;
        }
        [HttpGet]
        public bool DeleteElementConfig(string id)
        {
            return yhz.Dashboard.Core.DashboardRepository.DeleteElementConfig(id);
        }
        [HttpGet]
        public DashboardElementConfig CloneElementConfig(string id)
        {
            var config = yhz.Dashboard.Core.DashboardRepository.ElementConfigs
                .Where(e => e.ElementId == id).FirstOrDefault();

            if (config == null)
            {
                this.ThrowHttpException("无法根据指定的标识取得相应的元素类型！");
            }

            var newConfig = ObjectHelper.Clone<DashboardElementConfig>(config);

            newConfig.ElementId = System.Guid.NewGuid().ToString().Replace("-", "");
            newConfig.CreateTime = DateTime.Now;
            newConfig.ElementName = newConfig.ElementName + " (Copy)";

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(newConfig);

            yhz.Dashboard.Core.DashboardRepository.ElementConfigs.Insert(0,
                newConfig
                );

            return newConfig;
        }
        [HttpGet]
        public bool DeleteDashboardConfig(string id)
        {
            return yhz.Dashboard.Core.DashboardRepository.DeleteDashboardConfig(id);
        }
        [HttpGet]
        public DashboardConfig CloneDashboardConfig(string id)
        {
            var config = yhz.Dashboard.Core.DashboardRepository.DashboardConfigs
                .Where(e => e.DashboardId == id).FirstOrDefault();

            if (config == null)
            {
                this.ThrowHttpException("无法根据指定的标识取得相应的元素类型！");
            }

            var newConfig = ObjectHelper.Clone<DashboardConfig>(config);

            newConfig.DashboardId = System.Guid.NewGuid().ToString().Replace("-", "");
            newConfig.CreateTime = DateTime.Now;
            newConfig.DashboardName = newConfig.DashboardName + " (Copy)";

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(newConfig);

            yhz.Dashboard.Core.DashboardRepository.DashboardConfigs.Insert(0,
                newConfig
                );

            return newConfig;
        }
        [HttpGet]
        public bool DeleteTerminalInfoConfig(string id)
        {
            return yhz.Dashboard.Core.DashboardRepository.DeleteTerminalInfoConfig(id);
        }

        #endregion

        #region 看板数据操作

        public bool SaveDataConfig([FromBodyAttribute]DashboardDataConfig config)
        {
            var old = yhz.Dashboard.Core.DashboardRepository.DataConfigs
                .Where(d => d.DataId == config.DataId).FirstOrDefault();

            if (old == null)
            {
                ThrowHttpException("不存在此记录！", "修改数据失败！");
            }

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.DataConfigs.Remove(old);
            yhz.Dashboard.Core.DashboardRepository.DataConfigs.Add(config);

            return true;
        }

        [HttpGet]
        public DashboardDataConfig AddDataConfig()
        {
            DashboardDataConfig config = new DashboardDataConfig();

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.DataConfigs.Insert(0,
                config
                );

            return config;
        }

        #endregion

        #region 看板元素类型操作

        public bool SaveElementTypeConfig([FromBodyAttribute]DashboardElementTypeConfig config)
        {
            var old = yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs
                .Where(d => d.TypeId == config.TypeId).FirstOrDefault();

            if (old == null)
            {
                ThrowHttpException("不存在此记录！", "修改数据失败！");
            }

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs.Remove(old);
            yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs.Add(config);

            return true;
        }

        [HttpGet]
        public DashboardElementTypeConfig AddElementTypeConfig(bool widthData = true)
        {
            // 访问本地目录、使用预定义的模版生成元素配置
            string filePath =
                Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                widthData ? "Scripts/CodeTemplate.json" : "Scripts/CodeTemplate1.json");

            var config = ObjectHelper.JsonFile2Object<DashboardElementTypeConfig>(filePath);

            if (config == null)
                throw new Exception("无法自动生成预定义的元素类型对象！");

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs.Insert(0,
                config
                );

            return config;
        }

        [HttpGet]
        public DashboardElementTypeConfig GetElementTypeConfig(string id)
        {
            if (string.IsNullOrEmpty(id))
                this.ThrowHttpException("必须指定要获取的元素类型的标识！");

            var config = DashboardRepository.ElementTypeConfigs
                .Where(e => e.TypeId == id).FirstOrDefault();

            return config;
        }

        [HttpGet]
        public IEnumerable<ParaConfig> GetParaConfigs(string id)
        {
            if (string.IsNullOrEmpty(id))
                this.ThrowHttpException("必须指定要获取的元素类型的标识！");

            var config = DashboardRepository.ElementTypeConfigs
                .Where(e => e.TypeId == id).FirstOrDefault();

            if (config == null)
                this.ThrowHttpException("无法元素类型的参数！");

            return config.ParaConfigs;
        }

        #endregion

        #region 看板操作

        public bool SaveDashboardConfig([FromBodyAttribute]DashboardConfig config)
        {
            var old = yhz.Dashboard.Core.DashboardRepository.DashboardConfigs
                .Where(d => d.DashboardId == config.DashboardId).FirstOrDefault();

            if (old == null)
            {
                ThrowHttpException("不存在此记录！", "修改数据失败！");
            }

            // 这里 检查下元素 并且 把名字更新一次
            // 如果元素不存在了 需要移除
            if (config.ElementConfigs != null && config.ElementConfigs.Count > 0)
            {
                // 这里 get 方法可能导致效率地下、所以定义变量
                var elementConfigs = yhz.Dashboard.Core.DashboardRepository.ElementConfigs;

                for (int i = 0; i < config.ElementConfigs.Count; i++)
                {
                    var ele = elementConfigs.FirstOrDefault(e => e.ElementId == config.ElementConfigs[i].ElementId);

                    if (ele == null)
                    {
                        config.ElementConfigs.RemoveAt(i);
                    }
                    else
                        config.ElementConfigs[i].ElementName = ele.ElementName;
                }
            }

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.DashboardConfigs.Remove(old);
            yhz.Dashboard.Core.DashboardRepository.DashboardConfigs.Add(config);

            return true;
        }

        [HttpGet]
        public DashboardConfig AddDashboardConfig()
        {
            DashboardConfig config = new DashboardConfig();

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.DashboardConfigs.Insert(0,
                config
                );

            return config;
        }

        #endregion

        #region 终端操作

        public bool SaveTerminalInfo([FromBodyAttribute]DashboardTerminalInfo config)
        {
            var old = yhz.Dashboard.Core.DashboardRepository.TerminalInfos
                .Where(d => d.TerminalId == config.TerminalId).FirstOrDefault();

            if (old == null)
            {
                ThrowHttpException("不存在此记录！", "修改数据失败！");
            }

            if (config.Dashboards != null)
                config.Dashboards = config.Dashboards.OrderBy(d => d.PlayOrder).ToList();

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.TerminalInfos.Remove(old);
            yhz.Dashboard.Core.DashboardRepository.TerminalInfos.Add(config);

            return true;
        }

        public bool RedirectTerminalInfo(JObject obj)
        {
            var targetTer = DashboardRepository.TerminalInfos.Where(t => t.TerminalId == obj["targetTerminal"].ToString()).FirstOrDefault();
            
            if (targetTer == null)
            {
                foreach (var terId in obj["terminals"])
                {
                    var terInfo =
                        DashboardRepository.TerminalInfos.Where(t => t.TerminalId == terId.Value<string>()).FirstOrDefault();

                    if (terInfo == null)
                        continue;


                    terInfo.RedirectTerminalId = string.Empty;
                    terInfo.RedirectTerminalName = string.Empty;

                    yhz.Dashboard.Core.DashboardRepository.SaveConfig(terInfo);
                }
            }
            else
            {
                foreach (var terId in obj["terminals"])
                {
                    var terInfo =
                        DashboardRepository.TerminalInfos.Where(t => t.TerminalId == terId.Value<string>()).FirstOrDefault();

                    if (terInfo == null)
                        continue;

                    terInfo.RedirectTerminalId = targetTer.TerminalId;
                    terInfo.RedirectTerminalName = targetTer.TerminalName;


                    yhz.Dashboard.Core.DashboardRepository.SaveConfig(terInfo);
                }
            }

            return true;
        }

        /// <summary>
        /// 新增 终端
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        [HttpGet]
        public bool AddTerminalInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ThrowHttpException("必须指定终端标识！", "新增终端失败！");
            }

            DashboardTerminalInfo terinfo =
                DashboardRepository.TerminalInfos
                .Where(t => t.TerminalId == id).FirstOrDefault();

            if (terinfo == null)
            {
                terinfo = new DashboardTerminalInfo() { TerminalId = id, Ip = GetClientIp(Request) };

                DashboardRepository.SaveConfig(terinfo);

                DashboardRepository.TerminalInfos
                    .Add(terinfo);
            }

            // 根据终端 id 取得正在播放的看板
            var dbc = DashboardRepository.DashboardConfigs
                .Where(d => d.DashboardId == terinfo.DashboardId).FirstOrDefault();

            return true;
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            return ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        }

        [HttpGet]
        public bool IsRefreshTerminal(string terminalId, string lastEditTime)
        {
            var ter = yhz.Dashboard.Core.DashboardRepository.TerminalInfos
                .FirstOrDefault(t => t.TerminalId == terminalId);

            if (ter == null)
            {
                throw new Exception("不存在的终端！");
            }

            // 不相等就更新把 不管大小了
            if (lastEditTime != ter.LastEditTime)
                return true;

            return false;
        }

        #endregion

        #region 看板元素操作

        /// <summary>
        /// 保存元素配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool SaveElementConfig([FromBodyAttribute]DashboardElementConfig config)
        {
            var old = yhz.Dashboard.Core.DashboardRepository.ElementConfigs
                .Where(d => d.ElementId == config.ElementId).FirstOrDefault();

            if (old == null)
            {
                ThrowHttpException("不存在此记录！", "修改数据失败！");
            }

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.ElementConfigs.Remove(old);
            yhz.Dashboard.Core.DashboardRepository.ElementConfigs.Add(config);

            return true;
        }

        /// <summary>
        /// 新增元素配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DashboardElementConfig AddElementConfig([FromBodyAttribute]DashboardElementConfig config)
        {
            // 这里是新增、所以给一些对象重新赋值。例如 dataConfig、前端传过来的只有 dataId
            if (string.IsNullOrEmpty(config.TypeId))
                ThrowHttpException("必须选择元素类型！");

            // 这里用客户端的设置
            var paraConfigs = config.ParaConfigs;

            var _typeConfig = yhz.Dashboard.Core.DashboardRepository.ElementTypeConfigs
                .Where(e => e.TypeId == config.TypeId).FirstOrDefault();

            if (_typeConfig == null)
                ThrowHttpException(
                    string.Format("无法根据指定的元素类型标识取得具体的元素类型！TypeId:{0}", config.TypeId));

            config.TypeConfig = _typeConfig;
            config.TypeConfig.ParaConfigs = paraConfigs;

            if (!string.IsNullOrEmpty(config.DataId))
            {
                var _dataConfig = yhz.Dashboard.Core.DashboardRepository.DataConfigs
                    .Where(d => d.DataId == config.DataId).FirstOrDefault();

                if (_dataConfig == null)
                    ThrowHttpException(
                    string.Format("无法根据指定的数据项标识取得具体的数据项！DataId:{0}", config.DataId));

                config.DataConfig = _dataConfig;
            }

            // 应用 选择的元素类型
            config.ApplyElementTypeSet();

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.ElementConfigs.Insert(0,
                config
                );

            return config;
        }

        /// <summary>
        /// 获取 元素配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public DashboardElementConfig GetElementConfig(string id)
        {
            if (string.IsNullOrEmpty(id))
                this.ThrowHttpException("必须指定要获取的元素的标识！");

            var config = DashboardRepository.ElementConfigs
                .Where(e => e.ElementId == id).FirstOrDefault();

            if (config == null)
                this.ThrowHttpException("无法取得元素！");

            return config;
        }

        /// <summary>
        /// 重置元素配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DashboardElementConfig ResetElementParaConfig([FromBodyAttribute]DashboardElementConfig config)
        {
            // 应用 选择的元素类型
            config.ApplyElementTypeSet();


            // 把时间更新为 最新时间s
            config.CreateTime = DateTime.Now;

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            yhz.Dashboard.Core.DashboardRepository.ElementConfigs.Insert(0,
                config
                );

            return config;
        }

        /// <summary>
        /// 重置元素配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DashboardElementConfig ResetElementAllConfig([FromBodyAttribute]DashboardElementConfig config)
        {
            // 应用 选择的元素类型
            config.ApplyAllElementTypeSet();

            // 把时间更新为 最新时间s
            config.CreateTime = DateTime.Now;

            yhz.Dashboard.Core.DashboardRepository.SaveConfig(config);

            //yhz.Dashboard.Core.DashboardRepository.ElementConfigs.Insert(0,
            //    config
            //    );

            return config;
        }

        #endregion

        #region 获取所有图片
        /// <summary>
        /// 获取所有的图片路径
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetImages()
        {
            return yhz.Dashboard.GlobalResources.UserImages;
        }
        #endregion
    }
}
