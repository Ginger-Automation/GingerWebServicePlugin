using System;
using System.Collections.Generic;

namespace GingerWebServicePluginConsole
{
    class RestParam
    {
        public String url { get; set; }
        public REST_ACTION restAction { get; set; }
        public REQ_TYPE reqType { get; set; }
        public RES_TYPE resType { get; set; }
        public HTTP_VER httpVer { get; set; }
        public Dictionary<String, String> cookies { get; set; }
        public SECURITY security { get; set; }
        public List<FileDescription> fileDscList { get; set; }
        public Dictionary<String, String> httpHeader { get; set; }

        public List<HeaderParam> httpHeaders { get; set; }
        public Dictionary<String,String> paramMap { get; set; }
        public String body { get; set; }



 //       private List<String> actionList = new List<string>();
    
        public RestParam()
        { }

        public RestParam(String url)
        {
            this.url = url;
        }

        public RestParam(String RestURL,
                         REST_ACTION RestAction,
                         REQ_TYPE RequestType,
                         List<HeaderParam> HTTPHeaders,
                         String RequestBody)
        {
            this.url = RestURL;
            this.restAction = RestAction;
            this.reqType = RequestType;
            this.httpHeaders = HTTPHeaders;
            this.body = RequestBody;
        }

        public RestParam(String RestURL, 
                         REST_ACTION RestAction, 
                         REQ_TYPE RequestType,
                         RES_TYPE ResponseType,
                         SECURITY security, 
                         HTTP_VER HTTPVersion, 
                         Dictionary<String, String> cookies,
                         List<String> filePathList, 
                         Dictionary<String, String> HTTPHeaders,
                         Dictionary<String, String> paramMap,
                         String body)
        {
            this.url = RestURL;
            this.restAction = RestAction;
            this.reqType = RequestType;
            this.resType = ResponseType;
            this.httpVer = HTTPVersion;
            this.security = security;
            this.httpHeader = HTTPHeaders;
            this.paramMap = paramMap;
            this.body = body;

            this.httpHeader = new Dictionary<string, string>();
            if (HTTPHeaders != null)
            {
                foreach (KeyValuePair<String, String> entry in paramMap)
                    this.httpHeader.Add(entry.Key, entry.Value);
            }

            this.cookies = new Dictionary<string, string>();
            if (cookies != null){
                foreach(KeyValuePair<String, String> entry in cookies){
                    this.cookies.Add(entry.Key, entry.Value);
                }
            }

            this.paramMap = new Dictionary<string, string>();
            if (paramMap != null)
            {
                foreach (KeyValuePair<String, String> entry in paramMap)
                    this.paramMap.Add(entry.Key, entry.Value);
            }


            List<FileDescription> list = new List<FileDescription>();
            if (filePathList != null)
            {
                foreach (String entry in filePathList)
                {
                    if (entry != null)
                    {
                        FileDescription FileDsc = new FileDescription();
                        // TBD
                       // FileDsc.fileName = getFileName();
                       // FileDsc.filePath = getFilePath();
                       // FileDsc.contentType = getContentType();
                        list.Add(FileDsc);
                    }
                }
                this.fileDscList = list;
            }
        }




        /*
        public string Validate()
        {
            String result = null;
            try
            {
                if (url == null && "".Equals(url))
                    throw new Exception("Invalid URL");
                ValidateParam(action,actionList,"Invalid REST action",false);
                ValidateParam(reqType, reqTypeList, "Invalid request type", false);
                ValidateParam(resType, resTypeList, "Invalid response type", false);
                ValidateParam(cookies, cookiesList, "Invalid cookies option",true);
             }
            catch (Exception e)
            {
                result = e.Message;
            }
            if (result == null)
                result = "Success";

            return result;
         }

        public void ValidateParam(String value,List<String> list, String msg,Boolean exceptNull)
        {
            String result = msg;
            try
            {
                if (value != null)
                {
                    foreach (String s in list)
                    {
                        if (value.Equals(s))
                        {
                            result = "Success";
                            break;
                        }
                    }
                }
                else
                {
                    if (!exceptNull)
                        throw new Exception(msg);
                    else
                        result = "Success";
                }
                if (!"Success".Equals(result))
                    throw new Exception(msg);
            }
            catch (Exception e)
            {
                throw e;
            }
         }
         */



    }
}
