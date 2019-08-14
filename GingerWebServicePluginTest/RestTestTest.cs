using Amdocs.Ginger.Plugin.Core;
using GingerWebServicePluginConsole;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;

namespace GingerWebServicePluginTest.RestTest
{
    [TestClass]
    public class RestTestTest
    {
        const String baseURL = "https://httpbin.org/";
        const int OUTPUT_VALUES_COUNT = 16;
        const int VALID_RESPONSE_CODE = 200;
        const string VALID_RESPONSE_STATUS = "Completed";

        #region Default Class/Test Initialize Methods
        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            //
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // before every test
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            //after every test
        }
        #endregion

        [TestMethod]
        public void RestTest_GET()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA_GET = new GingerAction();

            List<HeaderParam> httpHeaders = new List<HeaderParam>();
            httpHeaders.Add(new HeaderParam("City", "Champaign"));
            httpHeaders.Add(new HeaderParam("State", "Illinois"));
            httpHeaders.Add(new HeaderParam("Country", "USA"));

            String actionURL = baseURL + "get";

            //Act
            service.RunWebService(GA_GET, actionURL,
                REST_ACTION.GET,
                REQ_TYPE.APP_JSON,
                httpHeaders);
            //string responseContent = GA_GET.Output["ResponseContent"].ToString();

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA_GET.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA_GET.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA_GET.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA_GET.Output["ResponseURI"].ToString());
            //StringAssert.Contains(responseContent, "Champaign");
        }

        [TestMethod]
        public void RestTest_PUT()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA_PUT = new GingerAction();

            List<HeaderParam> httpHeaders = new List<HeaderParam>();
            httpHeaders.Add(new HeaderParam("City", "Champaign"));
            httpHeaders.Add(new HeaderParam("State", "Illinois"));
            httpHeaders.Add(new HeaderParam("Country", "USA"));
            httpHeaders.Add(new HeaderParam("Continent", "North America"));

            String actionURL = baseURL + "put";
            String actionBody = "TestData";

            //Act
            service.RunWebService(GA_PUT, actionURL,
                REST_ACTION.PUT,
                REQ_TYPE.APP_JSON,
                httpHeaders, actionBody);
            string responseContent = GA_PUT.Output["ResponseContent"].ToString();

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA_PUT.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA_PUT.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA_PUT.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA_PUT.Output["ResponseURI"].ToString());
            StringAssert.Contains(responseContent, "North America");
        }

        [TestMethod]
        public void RestTest_DELETE()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA_DELETE = new GingerAction();

            List<HeaderParam> httpHeaders = new List<HeaderParam>();
            httpHeaders.Add(new HeaderParam("City", "Dallas"));
            httpHeaders.Add(new HeaderParam("State", "Texas"));
            httpHeaders.Add(new HeaderParam("Country", "USA"));
            httpHeaders.Add(new HeaderParam("Continent", "North America"));

            String actionURL = baseURL + "delete";

            //Act
            service.RunWebService(GA_DELETE, actionURL,
                REST_ACTION.DELETE,
                REQ_TYPE.APP_JSON,
                httpHeaders);
            string responseContent = GA_DELETE.Output["ResponseContent"].ToString();

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA_DELETE.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA_DELETE.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA_DELETE.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA_DELETE.Output["ResponseURI"].ToString());
            StringAssert.Contains(responseContent, "Texas");

            //JObject jObject = JObject.Parse(responseContent);
            //Assert.AreEqual("Champaign",(string)jObject["headers"][0]["City"]);
        }

    }
}
