using Amdocs.Ginger.CoreNET.Drivers.CommunicationProtocol;
using Amdocs.Ginger.Plugin.Core;
using RestSharp;
using System;

namespace GingerWebServicePluginConsole
{
    class program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start GingerWebServicePluginConsole");

            using (GingerNodeStarter gingerNodeStarter = new GingerNodeStarter())
            {
                if (args.Length > 0)
                {
                    gingerNodeStarter.StartFromConfigFile(args[0]);  // file name 
                }
                else
                {



                    //gingerNodeStarter.StartNode("WebService Service", new GingerWebService(), "10.120.8.135", 15001);
                    gingerNodeStarter.StartNode("WebService Service", new GingerWebService(), SocketHelper.GetLocalHostIP(), 15009);
                    //gingerNodeStarter.StartNode("WebService Service", new GingerWebService(), "10.120.21.102", 15005);
                    //gingerNodeStarter.StartNode("WebService Service", new GingerWebService(), "10.120.8.162", 15008);
                }
                gingerNodeStarter.Listen();
            }



            Console.WriteLine("End GingerWebServicePluginConsole");
        }



    }
}
