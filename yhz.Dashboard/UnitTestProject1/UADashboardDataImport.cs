using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UAUtility.OData;
using yhz.Dashboard.Core;

namespace UnitTestProject1
{
    [TestClass]
    public class UADashboardDataImport
    {
        private IUARequestService uaService = UARequestServiceFactory.GetService();

        [TestMethod]
        public void Import()
        {
            foreach (var item in DashboardRepository.ElementTypeConfigs)
            {
                
            }
        }
    }
}
