using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UAUtility.OData;
using yhz.Dashboard.Core;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace UnitTestProject2
{
    [TestClass]
    public class UADashboardDataImport
    {
        private IUARequestService uaService = UARequestServiceFactory.GetService();

        [TestMethod]
        public void ImportElementType()
        {
            var elementTypes = JArray.Parse(this.uaService.EntityQuery("ElementType"));

            foreach (var item in elementTypes)
            {
                CommandModel cm = new CommandModel(
                "UAMesApp",
                "ElementTypeDelete",
                new { Id = item["Id"].Value<string>() });

                this.uaService.CallCommand(cm);
            }

            foreach (var item in DashboardRepository.ElementTypeConfigs)
            {
                var Parameters = new List<object>();
                foreach (var para in item.ParaConfigs)
                {
                    Parameters.Add(new
                    {
                        Name = para.ParaName,
                        Code = para.ParaCode,
                        Value = para.Value,
                        RenderScript = para.RenderScript
                    });
                }

                var obj =
                    new
                    {
                        ElementType = new
                        {
                            Name = item.TypeName,
                            Group = item.TypeName,
                            Description = item.Descript,
                            HtmlTemplate = item.HtmlTemplate,
                            CssTemplate = item.CssTemplate,
                            JavaScriptTemplate =
                                string.IsNullOrEmpty(item.JavaScriptTemplate) ?
                                "//"
                                :
                                item.JavaScriptTemplate,
                            Parameters = Parameters
                        }
                    };

                CommandModel cm = new CommandModel(
                "UAMesApp",
                "ElementTypeCreate",
                obj);

                this.uaService.CallCommand(cm);
            }
        }
    }

}
