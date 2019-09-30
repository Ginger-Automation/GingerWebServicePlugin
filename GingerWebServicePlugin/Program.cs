using Amdocs.Ginger.Plugin.Core;
using GingerWebServicePlugin.Service;
using System;
using System.Collections.Generic;

namespace GingerWebServicePlugin
{
    class Program
    {
        internal static List<RestService> DriverSessions = new List<RestService>();
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CleanUp);
            Console.Title = "Rest Plugin";
            Console.WriteLine("Starting Rest Plugin");

            using (GingerNodeStarter gingerNodeStarter = new GingerNodeStarter())
            {
                if (args.Length > 0)
                {
                    gingerNodeStarter.StartFromConfigFile(args[0]);  // file name 
                }
                else
                {
                    gingerNodeStarter.StartNode("Rest Service 1", new RestService(),"10.20.121.221",15074);

                }
                gingerNodeStarter.Listen();
            }
        }

        private static void CleanUp(object sender, EventArgs e)
        {
            foreach (RestService RS in DriverSessions)
            {

                try
                {
                    RS.StopSession();
                }

                catch
                {

                }
            }

        }

    }

}
