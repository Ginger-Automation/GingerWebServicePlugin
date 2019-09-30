using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using GingerWebServicePlugin.Service;
using GingerTestHelper;
using System.Reflection;

namespace GingerWebServicePluginTests
{
    [TestClass]
    public class Tests
    {
   
        public static WebServicePlugin Service=null;
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext TestContext)
        {
            IWebHostBuilder webHostBuilder; webHostBuilder = WebHost.CreateDefaultBuilder().UseStartup<CustomersAPIService.Startup>();
            IWebHost webHost = webHostBuilder.Build();
            webHost.RunAsync();

            bool serverReady = WaitForServerReady().Result;
            if (!serverReady)
            {
                throw new Exception("Customers Web API server not ready");
            }
            Service = new WebServicePlugin();
            Service.StartSession();

            TestResources.Assembly = Assembly.GetExecutingAssembly();

        
         
        }
        private static async Task<bool> WaitForServerReady()
        {
            string rc = "";
            int i = 0;
            while (rc == "" && i < 10)
            {

                String actionURL = "http://localhost:5000/api/customer";
                var client = new HttpClient();
                try
                {
                    rc = await client.GetStringAsync(actionURL);
                }
                catch (HttpRequestException)
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

    }
}
