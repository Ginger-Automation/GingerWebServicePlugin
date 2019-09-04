using Ginger.Plugin.Platform.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static Ginger.Plugin.Platform.WebService.GingerHttpRequestMessage;
using static Ginger.Plugin.Platform.WebService.RestAPIKeyBodyValues;

namespace GingerWebServicePlugin.Client
{
    class RestClient : IHTTPClient
    {
        private string proxy;
        private string proxyUrl;
        private string BodyString = null;
        HttpRequestMessage RequestMessage;

        HttpClient Client = null;
        HttpClientHandler Handler = null;
        static Dictionary<string, Cookie> SessionCokiesDic;
        public RestClient(string proxy, string proxyUrl)
        {


            this.proxy = proxy;
            this.proxyUrl = proxyUrl;
            Handler = new HttpClientHandler();
            SessionCokiesDic = new Dictionary<string, Cookie>();
            SetProxySettings();
            Client = new HttpClient(Handler);
        }



        #region ProxySetUp
        private void SetProxySettings()
        {
#warning set all proxy modes

            if (!string.IsNullOrEmpty(this.proxyUrl))
            {
                WebProxy Proxy = new WebProxy(this.proxyUrl);
                Handler.Proxy = Proxy;
            }


        }

        #endregion



        public GingerHttpResponseMessage PerformHttpOperation(GingerHttpRequestMessage GingerRequestMessage)
        {
        
            Client = new HttpClient(Handler);

            PreparBasicRequest(GingerRequestMessage);
            HttpResponseMessage Response=null;
            try
            {
                Response = Client.SendAsync(RequestMessage).Result;
            }
            catch(Exception e)
            {

            }
            finally
            {
                HandleResponseCookies(GingerRequestMessage);
            }

            GingerHttpResponseMessage GRM=new GingerHttpResponseMessage();

            GRM.StatusCode = Response.StatusCode;
            GRM.Headers = new Dictionary<string, string>();
            //keeping the same pattern as existing Ginger Webserviced Plugin
            foreach (var Header in Response.Headers)
            {
                string headerValues = string.Empty;
                foreach (string val in Header.Value.ToArray())
                    headerValues = val + ",";
                headerValues = headerValues.Remove(headerValues.Length - 1);
                GRM.Headers.Add(Header.Key.ToString(), headerValues);
            }


            byte[] data = Response.Content.ReadAsByteArrayAsync().Result;
            GRM.Resposne= Encoding.Default.GetString(data);

            return GRM;

        }
        private void HandleResponseCookies(GingerHttpRequestMessage GingerRequestMessage)
        {
      
            if (GingerRequestMessage.CookieMode != eCookieMode.None)
            {

                CookieCollection responseCookies = Handler.CookieContainer.GetCookies(Client.BaseAddress);
                foreach (Cookie RespCookie in responseCookies)
                {
                    if (SessionCokiesDic.Keys.Contains(RespCookie.Name) == false)
                    {
                        SessionCokiesDic.Add(RespCookie.Name, RespCookie);
                    }
                    else
                    {
                        SessionCokiesDic[RespCookie.Name] = RespCookie;
                    }
                }
            }
        }

        private void PreparBasicRequest(GingerHttpRequestMessage GingerRequestMessage)
        {

            Client.BaseAddress = GingerRequestMessage.URL;

            HttpMethod Method = new HttpMethod(GingerRequestMessage.Method.ToUpper());
            RequestMessage = new HttpRequestMessage(Method, Client.BaseAddress);
            SetContentType(GingerRequestMessage);
            foreach (KeyValuePair<string, string> header in GingerRequestMessage.Headers)
            {
                RequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

           

            if (GingerRequestMessage.HttpVersion == ehttpVersion.HTTPV10)
            {
                RequestMessage.Version = HttpVersion.Version10;
            }
            else if
                (GingerRequestMessage.HttpVersion == ehttpVersion.HTTPV20)
            {
                RequestMessage.Version = HttpVersion.Version20;
            }
            else
            {
                RequestMessage.Version = HttpVersion.Version11;
            }


            SetCookies(GingerRequestMessage);
            SetRequestContent(Method, GingerRequestMessage);
        }



        private void SetContentType(GingerHttpRequestMessage GingerRequestMessage)
        {
            string ContentType=string.Empty;

            Enum.TryParse(GingerRequestMessage.ContentType, out GingerRequestMessage.BodyContentType);
           
                switch (GingerRequestMessage.BodyContentType)
                {
                    case eContentType.JSon:
                        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        ContentType = "application/json";
                        break;
                    case eContentType.XwwwFormUrlEncoded:
                        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                        ContentType = "application/x-www-form-urlencoded";
                        break;
                    case eContentType.FormData:
                        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                        ContentType = "multipart/form-data"; //update to correct value
                        break;
                    case eContentType.TextPlain:
                        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                        ContentType = "text/plain; charset=utf-8";
                        break;
                    case eContentType.XML:
                        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                        ContentType = "text/xml";
                        break;
                }
            

            GingerRequestMessage.ContentType = ContentType;
        }

        private void SetCookies(GingerHttpRequestMessage GingerRequestMessage)
        {
      
            switch (GingerRequestMessage.CookieMode)
            {
                case GingerHttpRequestMessage.eCookieMode.None:
                    break;
                case GingerHttpRequestMessage.eCookieMode.New:
                    SessionCokiesDic.Clear();
                    break;
                case GingerHttpRequestMessage.eCookieMode.Session:
                    foreach (Cookie cooki in SessionCokiesDic.Values)
                    {
                        Uri domainName = GingerRequestMessage.URL;
                        Cookie ck = new Cookie();
                        ck.Name = cooki.Name;
                        ck.Value = cooki.Value;
                        if (String.IsNullOrEmpty(cooki.Domain))
                        {
                            cooki.Domain = domainName.Host;
                        }
                        RequestMessage.Headers.Add(ck.Name, ck.Value);
                        Handler.CookieContainer.Add(cooki);
                    }
                    break;
            }
        }

          private void SetRequestContent( HttpMethod RequestMethod, GingerHttpRequestMessage GingerRequestMessage)
        {
            List<KeyValuePair<string, string>> KeyValues = new List<KeyValuePair<string, string>>();

            if (RequestMethod.Method== HttpMethod.Get.Method)
            {
                if (GingerRequestMessage.BodyContentType == eContentType.XwwwFormUrlEncoded)
                {
                    string GetRequest = "?";
                    if (GingerRequestMessage.RequestKeyValues.Count > 0)
                    {
                        for (int i = 0; i < GingerRequestMessage.RequestKeyValues.Count; i++)
                        {
                            GetRequest += GingerRequestMessage.RequestKeyValues[i].Param + "=" + GingerRequestMessage.RequestKeyValues[i].Value + "&";
                        }
                    }
                    string ValuesURL = GingerRequestMessage.URL.ToString() + GetRequest.Substring(0, GetRequest.Length - 1);
                    Client.BaseAddress = new Uri(ValuesURL);

                }
                else
                {
                    Client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", GingerRequestMessage.ContentType);
                }
            }
            else
            {
                if ((GingerRequestMessage.BodyContentType != eContentType.XwwwFormUrlEncoded) && (GingerRequestMessage.BodyContentType != eContentType.FormData))
                {
                    BodyString = GingerRequestMessage.BodyString;
                }

                switch (GingerRequestMessage.BodyContentType)
                {
                    case eContentType.XwwwFormUrlEncoded:
                        if (GingerRequestMessage.RequestKeyValues.Count > 0)
                        {
                            List<KeyValuePair<string, string>> RequestKeyValues = new List<KeyValuePair<string, string>>();

                            for (int i = 0; i < GingerRequestMessage.RequestKeyValues.Count; i++)
                            {
                                KeyValues.Add(new KeyValuePair<string, string>(GingerRequestMessage.RequestKeyValues[i].Param, GingerRequestMessage.RequestKeyValues[i].Value));
                            }
                            RequestMessage.Content = new FormUrlEncodedContent(RequestKeyValues);
                        }
                        break;
                    case eContentType.FormData:
                        if (GingerRequestMessage.RequestKeyValues.Count > 0)
                        {
                            MultipartFormDataContent requestContent = new MultipartFormDataContent();
                            List<KeyValuePair<string, string>> FormDataKeyValues = new List<KeyValuePair<string, string>>();
                            for (int i = 0; i < GingerRequestMessage.RequestKeyValues.Count; i++)
                            {
                                if (GingerRequestMessage.RequestKeyValues[i].ValueType == eValueType.Text)
                                {
                                    FormDataKeyValues.Add(new KeyValuePair<string, string>(GingerRequestMessage.RequestKeyValues[i].Param, GingerRequestMessage.RequestKeyValues[i].Value));
                                    requestContent.Add(new StringContent(GingerRequestMessage.RequestKeyValues[i].Value), GingerRequestMessage.RequestKeyValues[i].Param);
                                }
                                if (GingerRequestMessage.RequestKeyValues[i].ValueType ==eValueType.File)
                                {
                               
                                    requestContent.Add(GingerRequestMessage.RequestKeyValues[i].Content, GingerRequestMessage.RequestKeyValues[i].Param,  GingerRequestMessage.RequestKeyValues[i].Filename);
                                }

                            }
                            RequestMessage.Content = requestContent;
                        }
                        break;
                    case eContentType.XML:
                        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                        if (BodyString.StartsWith(_byteOrderMarkUtf8))
                        {
                            var lastIndexOfUtf8 = _byteOrderMarkUtf8.Length - 1;
                            BodyString = BodyString.Remove(0, lastIndexOfUtf8);
                        }
                        RequestMessage.Content = new StringContent(BodyString, Encoding.UTF8, GingerRequestMessage.ContentType);
                        break;
                    default:
                        RequestMessage.Content = new StringContent(BodyString, Encoding.UTF8, GingerRequestMessage.ContentType);
                        break;
                }
            }
        }
    }
}
