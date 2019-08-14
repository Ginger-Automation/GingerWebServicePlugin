using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace GingerWebServicePluginConsole
{
    class RestHandler
    {
        private static RestRequest request = null;

        public static RestHandler Builder()
        {
            return new RestHandler();
        }

        public RestHandler SetRequest(REST_ACTION action)
        {
            try
            {
                if (REST_ACTION.GET == action)
                    request = new RestRequest(Method.GET);
                else if (REST_ACTION.POST == action)
                    request = new RestRequest(Method.POST);
                else if (REST_ACTION.PUT == action)
                    request = new RestRequest(Method.PUT);
                else if (REST_ACTION.DELETE == action)
                    request = new RestRequest(Method.DELETE);
                else if (REST_ACTION.PATCH == action)
                    request = new RestRequest(Method.PATCH);
                if (request != null)
                    request.Timeout = 10000;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to set REST action");
                throw e;
            }
            return this;
        }

        public RestHandler SetCookies(Dictionary<String, String> cookiesMap)
        {
            try
            {
                if (cookiesMap != null && request != null)
                {
                    foreach (KeyValuePair<String, String> entry in cookiesMap)
                    {
                        request.AddCookie(entry.Key, entry.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add cookies");
                throw e;
         }
            return this;
        }

        public RestHandler SetHeader(Dictionary<String, String> headerMap)
        {
            try
            {
                if (headerMap != null && request != null)
                {
                    foreach (KeyValuePair<String, String> entry in headerMap)
                    {
                        request.AddHeader(entry.Key, entry.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add header");
                throw e;
            }
            return this;
        }

        public RestHandler SetHeaders(List<HeaderParam> headerParamList)
        {
            try
            {
                if (headerParamList != null && request != null)
                {
                    foreach (HeaderParam entry in headerParamList)
                    {
                        request.AddHeader(entry.Key, entry.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add header");
                throw e;
            }
            return this;
        }

        public RestHandler SetParamMap(Dictionary<String, String> paramMap)
        {
            try
            {
                if (paramMap != null && request != null){
                    foreach (KeyValuePair<String, String> entry in paramMap){
                        request.AddParameter(entry.Key,entry.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add parameters");
                throw e;
            }
            return this;
        }

        public RestHandler SetBody(String body, REQ_TYPE req)
        {
            try
            {
                if (body != null && request != null) 
                {
                   if (REQ_TYPE.APP_JSON == req)      
                     request.AddJsonBody(body);
                   else if (REQ_TYPE.XML == req)
                     request.AddXmlBody(body);
               }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add body");
                throw e;
            }
            return this;
        }

        public RestHandler SetSecurity(SECURITY security)
        {
            try
            {
                if (security == SECURITY.TLS)
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
                else if (security == SECURITY.TLS_11)
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11;
                else if (security == SECURITY.TLS_12)
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                else if (security == SECURITY.SSL_3)
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;
             }
            catch (Exception e)
            {
                Console.WriteLine("Failed to set security protocol");
                throw e;
            }
            return this;
        }

        public RestHandler SetFileList(REST_ACTION action,List<FileDescription> fileDscList)
        {
            try
            {
                if (fileDscList != null && request != null)
                {
                    if (action == REST_ACTION.POST || action == REST_ACTION.PUT)
                    {
                        foreach (FileDescription entry in fileDscList)
                        {
                            if (entry != null)
                              request.AddFile(entry.fileName,entry.filePath,entry.contentType);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add file to body");
                throw e;
            }
            return this;
        }

        public RestRequest getRequest()
        {
            return request;
        }

        public string execute(string url)
        {
            RestClient client = new RestClient(url);

            ServicePointManager.ServerCertificateValidationCallback +=
             (sender, certificate, chain, errors) => true;

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

       

    }
}
