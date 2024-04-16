using AutomatedTestAPI.HomeControllerRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTestAPI
{
    [TestClass]
    public class AutomatedAPITest
    {
        private readonly string domain_url = "http://localhost:7125";

        [TestMethod]
        public void TestIndexRouteAsync()
        {
            TestHomeController testHomeController = new TestHomeController();
            var result = testHomeController.TestIndexRoute().Result;
            Assert.AreEqual("OK", result);
        }
    }
}
