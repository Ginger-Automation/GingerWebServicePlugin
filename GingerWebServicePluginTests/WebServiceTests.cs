using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amdocs.Ginger.CoreNET.RunLib;
using GingerTestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace GingerWebServicePluginTests
{
    [TestClass]
   public class WebServiceTests
    {


        [TestMethod]
        public void GetMethodTest()
        {
            string filepath = TestResources.GetTestResourcesFile(@"GetRequest.json");

            string filecontent = System.IO.File.ReadAllText(filepath);
            NodePlatformAction nodePlatformAction = JsonConvert.DeserializeObject<NodePlatformAction>(filecontent);

            nodePlatformAction.Output = new NodeActionOutput();

            Tests.Service.PlatformActionHandler.HandleRunAction(Tests.Service, ref nodePlatformAction);


            Assert.AreEqual("OK", nodePlatformAction.Output.OutputValues.Where(x => x.Param == "Header: Status Code ").FirstOrDefault().Value);

        }
        [TestMethod]
        public void PutWithTestBodyTest()
        {
            string filepath = TestResources.GetTestResourcesFile(@"PutWithTestBody.json");

            string filecontent = System.IO.File.ReadAllText(filepath);
            NodePlatformAction nodePlatformAction = JsonConvert.DeserializeObject<NodePlatformAction>(filecontent);

            nodePlatformAction.Output = new NodeActionOutput();

            Tests.Service.PlatformActionHandler.HandleRunAction(Tests.Service, ref nodePlatformAction);


            Assert.AreEqual("OK", nodePlatformAction.Output.OutputValues.Where(x => x.Param == "Header: Status Code ").FirstOrDefault().Value);
            Assert.AreEqual("Put", nodePlatformAction.Output.OutputValues.Where(x => x.Param == "Header: MessageType").FirstOrDefault().Value);
        }

        [TestMethod]
        public void PostWithKeyValuesTest()
        {
            string filepath = TestResources.GetTestResourcesFile(@"PostWithKeyValues.json");

            string filecontent = System.IO.File.ReadAllText(filepath);
            NodePlatformAction nodePlatformAction = JsonConvert.DeserializeObject<NodePlatformAction>(filecontent);

            nodePlatformAction.Output = new NodeActionOutput();

            Tests.Service.PlatformActionHandler.HandleRunAction(Tests.Service, ref nodePlatformAction);


            Assert.AreEqual("OK", nodePlatformAction.Output.OutputValues.Where(x => x.Param == "Header: Status Code ").FirstOrDefault().Value);

        }

        [TestMethod]
        public void PostWithTextBodyAndHeadersTest()
        {
            string filepath = TestResources.GetTestResourcesFile(@"PostWithTextBodyAndHeaders.json");

            string filecontent = System.IO.File.ReadAllText(filepath);
            NodePlatformAction nodePlatformAction = JsonConvert.DeserializeObject<NodePlatformAction>(filecontent);

            nodePlatformAction.Output = new NodeActionOutput();

            Tests.Service.PlatformActionHandler.HandleRunAction(Tests.Service, ref nodePlatformAction);


            Assert.AreEqual("OK", nodePlatformAction.Output.OutputValues.Where(x => x.Param == "Header: Status Code ").FirstOrDefault().Value);

        }

    }
}
