using Amdocs.Ginger.Plugin.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GingerWebServicePluginConsole
{
    [GingerService("WebService", "API Web Service")]
    public class GingerWebService
    {

        [GingerAction("RunWebService", "Run Web Service")]
        public void RunWebService(IGingerAction GA,
                                    String RestURL,
                                    REST_ACTION RestAction,
                                    REQ_TYPE RequestType,
                                    List<HeaderParam> HTTPHeaders = null,
                                    String RequestBody = null, 
                                    String ProxyIPAddress = null,
                                    String ProxyPort = null, 
                                    String CertificateFilePath = null,
                                    String CertificatePassword = null)
        {

            Console.WriteLine("Inside RunWebService");

            
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("RestURL:" + RestURL);
            //    sb.AppendLine("RestAction:" + RestAction.ToString());
            //    sb.AppendLine("RequestType:" + RequestType.ToString());
            //    if (HTTPHeaders != null)
            //        sb.AppendLine("HTTPHeaders:" + HTTPHeaders.ToString());
            //    if (RequestBody != null)
            //        sb.AppendLine("RequestBody:" + RequestBody);
            //    File.AppendAllText(@"c:\temp\ginger_plugin_log.txt", sb.ToString());
            
            

            // create rest objects
            RestClient restClient = new RestClient(RestURL);
            if ( (!String.IsNullOrEmpty(ProxyIPAddress)) && (!String.IsNullOrEmpty(ProxyPort)) )
            {
                restClient.Proxy = new WebProxy(ProxyIPAddress, Convert.ToInt32(ProxyPort));
            }

            RestRequest restRequest = new RestRequest();

            // ignore the certificate check when ssl
            ServicePointManager.ServerCertificateValidationCallback +=
             (sender, certificate, chain, errors) => true;

            // set the request method
            switch (RestAction)
            {
                case REST_ACTION.PUT:
                    restRequest.Method = Method.PUT;
                    break;
                case REST_ACTION.GET:
                    restRequest.Method = Method.GET;
                    break;
                case REST_ACTION.POST:
                    restRequest.Method = Method.POST;
                    break;
                case REST_ACTION.DELETE:
                    restRequest.Method = Method.DELETE;
                    break;
                case REST_ACTION.PATCH:
                    restRequest.Method = Method.PATCH;
                    break;
                    //TODO: add all options: Head etc.
            }

            // adding headers
            if (HTTPHeaders != null && HTTPHeaders.Count > 0)
            {
                foreach (HeaderParam headerParam in HTTPHeaders)
                {
                    restRequest.AddHeader(headerParam.Key, headerParam.Value);
                }
            }

            // adding body
            if (RequestType == REQ_TYPE.APP_JSON)
            {
                restRequest.AddJsonBody(RequestBody);
                //const string contentType = "application/json";
                //request.AddParameter(contentType, body, ParameterType.RequestBody);
            }
            else if (RequestType == REQ_TYPE.XML)
            {
                restRequest.AddXmlBody(RequestBody);
            }
            else if (RequestType == REQ_TYPE.TEXT_PLAIN)
            {
                //plain text handling
            }

            // add client certificate
            if ((!String.IsNullOrEmpty(CertificateFilePath)) && (!String.IsNullOrEmpty(CertificatePassword)))
            {
                X509Certificate2  x509Certificate2 = new X509Certificate2(CertificateFilePath, CertificatePassword);
                restClient.ClientCertificates = new X509CertificateCollection() { x509Certificate2 };
            }
            
            //X509Certificate2 clientCertificate = new X509Certificate2();
            
            // execute the request
            IRestResponse response = restClient.Execute(restRequest);

            // response code
            int numericStatusCode = (int)response.StatusCode;

            // debug purpose
            if (string.IsNullOrEmpty(response.Content))
            {
                Console.WriteLine("Error Message:" + response.ErrorMessage);
                Console.WriteLine("Error Exception:" + response.ErrorException);
            }
            else
            {
                Console.WriteLine(response.Content);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Made a call to " + RestURL);
            stringBuilder.AppendLine("Rest Action :" + RestAction.ToString());
            stringBuilder.AppendLine("Request Type :" + RequestType.ToString());
            stringBuilder.AppendLine("Status Code :" + numericStatusCode);
            stringBuilder.AppendLine("Status Code Str :" + response.StatusCode.ToString());
            stringBuilder.AppendLine("Response Status :" + response.ResponseStatus);

            // adding response to GA object
            GA.AddExInfo(stringBuilder.ToString());

            // add all the output values to the GA object
            GA.AddOutput("ResponseContent", response.Content);
            //GA.AddOutput("ResponseContent", response.Content.Substring(1,1000));
            GA.AddOutput("ResponseStatus", response.ResponseStatus);
            GA.AddOutput("ResponseCode", numericStatusCode);
            GA.AddOutput("ResponseCodeStr", response.StatusCode.ToString());

            int itr = 1;
            foreach(Parameter parameter in response.Headers)
            {
                GA.AddOutput(parameter.Name, parameter.Value, "Response Header " + itr++);
            }
            //GA.AddOutput("ResponseHeaders", response.Headers);

            GA.AddOutput("ResponseURI", response.ResponseUri);
            GA.AddOutput("ResponseErrorMessage", response.ErrorMessage);
        }

        //[GingerAction("RunScript", "Run web service")]
        //public Object RunWebService(IGingerAction GA, 
        //                            String url, 
        //                            Constants.REST_ACTION restAction, 
        //                            Constants.REQ_TYPE reqType,
        //                            Constants.RES_TYPE resType, 
        //                            Constants.SECURITY security, 
        //                            Constants.HTTP_VER httpVer, 
        //                            Dictionary<String, String>  cookies = null,
        //                            List<String> filePathList= null, 
        //                            Dictionary<String, String> httpHeader = null,
        //                            Dictionary<String, String> paramMap = null, 
        //                            String body = null)
        //{
        //    RestParam restParam = new RestParam(url, restAction, reqType,
        //                                        resType, security, httpVer, cookies,filePathList, httpHeader,
        //                                        paramMap, body);
        //    RestHandler restHandler = null;
        //    Object content = null;
        //    try
        //    {
        //        Console.WriteLine("start Web service");

        //        var client = new RestClient(restParam.url);

        //        restHandler = RestHandler.Builder().
        //            SetRequest(restParam.restAction).
        //            SetHeader(restParam.httpHeader).
        //            SetParamMap(restParam.paramMap).
        //            SetCookies(restParam.cookies).
        //            SetBody(restParam.body, restParam.reqType).
        //            SetSecurity(restParam.security).
        //            SetFileList(restParam.restAction,restParam.fileDscList);

        //        IRestResponse response = client.Execute(restHandler.getRequest());
        //        content = response.Content; // raw content as string
        //        Console.WriteLine("status=" + response.StatusCode);
        //        Console.WriteLine("content=" + content);

        //        if (content != null)
        //          GA.AddOutput("Param1", content);

        //        Console.WriteLine("End Web service");
        //    }
        //    catch(Exception e)
        //    {
        //        Console.WriteLine("Failed to execute rest call");
        //        Console.WriteLine(e.Message);
        //    }
        //    return content;
        //}



    }


}
