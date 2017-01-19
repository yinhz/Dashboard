using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UAUtility.OAuth;

namespace UAUtility.OData
{
    public class UAODataRequestService : IUARequestService
    {
        public UAODataRequestService(IUAOAuthService uaOAuthService, string ua_ODATA_ENDPOINT)
        {
            if (uaOAuthService == null)
                throw new ArgumentNullException("uaOAuthService");

            this.UAOAuthService = uaOAuthService;

            var _token = this.UAOAuthService.GetToken();

            if (_token == null || string.IsNullOrEmpty(_token.AccessToken))
                throw new Exception("无法获得服务器授权！");

            this.UAOAuthToken = _token;

            this.UA_ODATA_ENDPOINT = ua_ODATA_ENDPOINT;
        }

        public string UA_ODATA_ENDPOINT { get; private set; }

        public string EntityQuery(string entityName, string options = "")
        {
            if (string.IsNullOrEmpty(entityName))
                throw new ArgumentNullException("entityName");

            string odataUrl = UA_ODATA_ENDPOINT + entityName;

            if (!string.IsNullOrEmpty(options))
                odataUrl = odataUrl + "?" + options;

            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(odataUrl);
                wReq.Method = "GET";
                wReq.Headers["Authorization"] = @"Bearer " + this.UAOAuthToken.AccessToken;
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException webException)
            {
                if (webException.Response!=null && webException.Response is HttpWebResponse)
                {
                    if (((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        this.UAOAuthToken = this.UAOAuthService.GetToken();

                        return this.EntityQuery(entityName, options);
                    }
                }

                // if occur other exception.
                throw;
            }
        }

        public string CallCommand(CommandModel cmdModel)
        {
            if (cmdModel == null || string.IsNullOrEmpty(cmdModel.AppName) || string.IsNullOrEmpty(cmdModel.CommandName))
                throw new ArgumentNullException("cmdModel");

            System.Net.WebRequest wReq = System.Net.WebRequest.Create(
                UA_ODATA_ENDPOINT
                + cmdModel.AppName
                + "_"
                + cmdModel.CommandName
                );

            wReq.Method = "POST";
            wReq.Headers["Authorization"] = @"Bearer " + this.UAOAuthToken.AccessToken;

            dynamic commandPara = new { command = cmdModel.Params };
            string postData =
                Newtonsoft.Json.JsonConvert.SerializeObject(commandPara);

            byte[] byteArray = this.ContentEncoding.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            wReq.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            wReq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = wReq.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.Z
            dataStream.Close();

            // Get the response instance.
            System.Net.WebResponse wResp = null;

            try
            {
                // Get the response instance.
                wResp = wReq.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    if (wex.Response is HttpWebResponse)
                    {
                        // if occur unauthorized exception, get token again.
                        if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.Unauthorized)
                        {
                            this.UAOAuthToken = this.UAOAuthService.GetToken();

                            return this.CallCommand(cmdModel);
                        }
                    }

                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            throw new Exception(reader.ReadToEnd(), wex);
                        }
                    }
                }
            }

            System.IO.Stream respStream = wResp.GetResponseStream();
            // Dim reader As StreamReader = New StreamReader(respStream)
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
            {
                return reader.ReadToEnd();
            }
        }


        public UAOAuthToken UAOAuthToken
        {
            get;
            private set;
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.UTF8; }
        }


        public IUAOAuthService UAOAuthService
        {
            get;
            private set;
        }
    }
}
