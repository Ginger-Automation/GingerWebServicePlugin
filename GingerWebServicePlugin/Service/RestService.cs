using Amdocs.Ginger.Plugin.Core;
using Amdocs.Ginger.Plugin.Core.ActionsLib;
using Amdocs.Ginger.Plugin.Core.Attributes;
using Ginger.Plugin.Platform.WebService;
using Ginger.Plugin.Platform.WebService.Execution;
using GingerWebServicePlugin.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace GingerWebServicePlugin.Service
{
    [GingerService("RestService", "Rest Service")]
    public class RestService : IServiceSession, IWebServicePlatform
    {
        public IPlatformActionHandler PlatformActionHandler { get; set; } = new WebServicePlatformActionHandler();


        #region Plugin Configuration

        [ValidValue(new string[] { "Direct", "Manual", "ProxyAutoConfigure", "AutoDetect", "System" })]
        [ServiceConfiguration("Proxy Type", "Proxy type")]
        public string Proxy { get; set; }


        [MinLength(10)]
        [ServiceConfiguration("Proxy Url", "Proxy URL or prixy autoconfig url")]
        public string ProxyUrl { get; set; }
        public IRestClient RestClient { get; set; }

        #endregion



        public void StartSession()
        {
            RestClient = new RestClient(Proxy,ProxyUrl);
        }

        public void StopSession()
        {
            
        }
    }
}
