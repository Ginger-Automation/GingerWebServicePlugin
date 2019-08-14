using Amdocs.Ginger.Plugin.Core;
using CustomersAPIService.Data;
using GingerWebServicePluginConsole;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GingerWebServicePluginTest.CustomerTests
{
    [TestClass]
    public class CustomerServiceTest
    {
        const int OUTPUT_VALUES_COUNT = 7;
        const int VALID_RESPONSE_CODE = 200;
        const string VALID_RESPONSE_CODE_STR = "OK";
        const string VALID_RESPONSE_STATUS = "Completed";

        #region Default Class/Test Initialize Methods
        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {            
            Task.Factory.StartNew(()=> {
                IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder(null).UseStartup<CustomersAPIService.Startup>();                
                webHostBuilder.Build().Run();                                
            });

            bool serverReady = WaitForServerReady().Result;
            if(!serverReady)
            {
                throw new Exception("Customers Web API server not ready");
            }
        }

        private static async Task<bool> WaitForServerReady()
        {
            string rc = "";
            int i = 0;
            while (rc == "" && i <10)
            {
                String actionURL = "http://localhost:5000/api/customer";
                var client = new HttpClient();
                try
                {
                    rc = await client.GetStringAsync(actionURL);
                }
                catch(System.Net.Http.HttpRequestException httpRequestException)
                {
                    // just wait for retry
                    Thread.Sleep(1000);
                }                
            }
            if (!string.IsNullOrEmpty(rc))
            {
                return true;
            }
            else
            {
                return false;
            }            
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
        public void CustomerTest_GETCustomersList()
        {
            // Arrange

            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA_GET = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer";

            List<HeaderParam> httpHeaders = new List<HeaderParam>();
            httpHeaders.Add(new HeaderParam("City", "Dallas"));
            httpHeaders.Add(new HeaderParam("State", "Texas"));

            //Act
            service.RunWebService(GA_GET, actionURL,
                REST_ACTION.GET,
                REQ_TYPE.APP_JSON,
                httpHeaders);
            string responseContent = GA_GET.Output["ResponseContent"].ToString();
            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(responseContent);

            //Assert
            //Assert.AreEqual(3, customers.Count);
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA_GET.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA_GET.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA_GET.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA_GET.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA_GET.Output["ResponseURI"].ToString());
            StringAssert.Contains(responseContent, "Champaign");
        }

        [TestMethod]
        public void CustomerTest_GETCustomerByID_1()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA_GET = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer/1";

            //Act
            service.RunWebService(GA_GET, actionURL,
                REST_ACTION.GET,
                REQ_TYPE.APP_JSON);
            string responseContent = GA_GET.Output["ResponseContent"].ToString();
            Customer customer = JsonConvert.DeserializeObject<Customer>(responseContent);

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA_GET.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA_GET.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA_GET.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA_GET.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA_GET.Output["ResponseURI"].ToString());
            Assert.AreEqual("Amdocs", customer.CustomerName);
            StringAssert.Contains(responseContent, "Israel");
        }

        [TestMethod]
        public void CustomerTest_GETCustomerByID_2()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA_GET = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer/2";

            //Act
            service.RunWebService(GA_GET, actionURL,
                REST_ACTION.GET,
                REQ_TYPE.APP_JSON);
            string responseContent = GA_GET.Output["ResponseContent"].ToString();
            Customer customer = JsonConvert.DeserializeObject<Customer>(responseContent);

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA_GET.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA_GET.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA_GET.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA_GET.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA_GET.Output["ResponseURI"].ToString());
            Assert.AreEqual("AmdocsDVCI", customer.CustomerName);
            StringAssert.Contains(responseContent, "India");
        }


        [TestMethod]
        public void CustomerTest_GETCustomerByID_0()
        {   
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA_GET = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer/0";

            //Act
            service.RunWebService(GA_GET, actionURL,
                REST_ACTION.GET,
                REQ_TYPE.APP_JSON);
            string responseContent = GA_GET.Output["ResponseContent"].ToString();
            Customer customer = JsonConvert.DeserializeObject<Customer>(responseContent);

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA_GET.Output.OutputValues.Count);
            Assert.AreEqual(204, GA_GET.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA_GET.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA_GET.Output["ResponseURI"].ToString());
            Assert.AreEqual(null, customer);
        }

        [TestMethod]
        [Ignore]
        public void CustomerTest_GETCustomerByName_1()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA1 = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer/Amdocs";

            //Act
            service.RunWebService(GA1, actionURL,
                REST_ACTION.GET,
                REQ_TYPE.APP_JSON);
            string responseContent = GA1.Output["ResponseContent"].ToString();
            Customer customer = JsonConvert.DeserializeObject<Customer>(responseContent);

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA1.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA1.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA1.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA1.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA1.Output["ResponseURI"].ToString());
            Assert.AreEqual(null, customer);
        }

        [TestMethod]
        public void CustomerTest_POSTAddCustomer()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA1 = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer/";

            Customer customer = new Customer() { CustomerID = "4", CustomerName = "Trump", CustomerAddress = "White House, Washington DC" };
            //List<Customer> customersList = new List<Customer>();
            //customersList.Add(customer);


            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(customer); //serialize the object
            //Customer obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(jsonCustomer); //deserialize the object 

            //Act
            service.RunWebService(GA1, actionURL,
                REST_ACTION.POST,
                REQ_TYPE.APP_JSON,
                null, jsonBody);

            string responseContent = GA1.Output["ResponseContent"].ToString();

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA1.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA1.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA1.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA1.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA1.Output["ResponseURI"].ToString());
            StringAssert.Contains(responseContent, "Customer Added");
        }


        [TestMethod]
        [Ignore]
        public void CustomerTest_POSTAddCustomers()
        {
            // Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA1 = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer/";

            Customer customer1 = new Customer() { CustomerID = "11", CustomerName = "Sachin Tendulkar", CustomerAddress = "Mumbai" };
            Customer customer2 = new Customer() { CustomerID = "12", CustomerName = "Virat Kohli", CustomerAddress = "Delhi" };
            Customer customer3 = new Customer() { CustomerID = "13", CustomerName = "Mahendra Singh Dhoni", CustomerAddress = "Ranchi" };

            List<Customer> customersList = new List<Customer>();
            customersList.Add(customer1);
            customersList.Add(customer2);
            customersList.Add(customer3);

            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(customersList); //serialize the object
            //Customer obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(jsonCustomer); //deserialize the object 

            //Act
            service.RunWebService(GA1, actionURL,
                REST_ACTION.POST,
                REQ_TYPE.APP_JSON,
                null, jsonBody);

            string responseContent = GA1.Output["ResponseContent"].ToString();

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA1.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA1.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA1.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA1.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA1.Output["ResponseURI"].ToString());
            StringAssert.Contains(responseContent, "Customers Added");
        }


        [TestMethod]
        public void CustomerTest_AddAndDeleteCustomer()
        {
            //Arrange
            GingerWebService service = new GingerWebService();
            GingerAction GA1 = new GingerAction();

            String actionURL = "http://localhost:5000/api/customer/";

            Customer customer = new Customer() { CustomerID = "22", CustomerName = "Bill Gates", CustomerAddress = "Seattle, Washington, U.S." };

            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(customer); //serialize the object

            //Act
            service.RunWebService(GA1, actionURL,
                REST_ACTION.POST,
                REQ_TYPE.APP_JSON,
                null, jsonBody);

            string responseContent = GA1.Output["ResponseContent"].ToString();

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA1.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA1.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA1.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA1.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA1.Output["ResponseURI"].ToString());
            StringAssert.Contains(responseContent, "Customer Added");

            //Arrange
            GingerAction GA2 = new GingerAction();
            actionURL = "http://localhost:5000/api/customer/" + customer.CustomerID;

            //Act
            service.RunWebService(GA2, actionURL,
                REST_ACTION.DELETE,
                REQ_TYPE.APP_JSON);
            responseContent = GA2.Output["ResponseContent"].ToString();

            //Assert
            Assert.AreEqual(OUTPUT_VALUES_COUNT, GA2.Output.OutputValues.Count);
            Assert.AreEqual(VALID_RESPONSE_CODE, GA2.Output["ResponseCode"]);
            Assert.AreEqual(VALID_RESPONSE_CODE_STR, GA2.Output["ResponseCodeStr"]);
            Assert.AreEqual(VALID_RESPONSE_STATUS, GA2.Output["ResponseStatus"].ToString());
            Assert.AreEqual(actionURL, GA2.Output["ResponseURI"].ToString());
            StringAssert.Contains(responseContent, "Customer Deleted");
        }


    }
}
