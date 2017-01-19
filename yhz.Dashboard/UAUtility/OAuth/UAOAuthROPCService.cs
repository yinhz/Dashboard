using System;
using System.Text;
using System.IO;
using System.Net;

namespace UAUtility.OAuth
{
    /// <summary>
    /// 密码模式的服务
    /// </summary>
    public class UAOAuthROPCService : IUAOAuthService
    {
        public UAOAuthROPCService(string serviceEndpoint)
        {
            this.ServiceEndpoint = serviceEndpoint;
        }
        public string ServiceEndpoint
        {
            get;
            private set;
        }

        public UAOAuthToken GetToken(string clientId = "123", string clientSecrets = "123")
        {
            return this.GetToken(UAUtilityConfig.UA_RO_USERNAME, UAUtilityConfig.UA_RO_PASSWORD, clientId, clientSecrets);
        }

        public UAOAuthToken RefreshToken(UAOAuthToken oldToken, string clientId = "123", string clientSecrets = "123")
        {
            // 暂时用 重新获取 token
            return this.GetToken();
        }


        public Encoding ContentEncoding
        {
            get { return Encoding.UTF8; }
        }


        public UAOAuthToken GetToken(string roUserName, string roPassword, string clientId = "123", string clientSecrets = "123")
        {
            if (string.IsNullOrEmpty(roUserName))
                throw new ArgumentNullException("roUserName");

            if (string.IsNullOrEmpty(roPassword))
                throw new ArgumentNullException("roPassword");

            System.Net.WebRequest wReq = System.Net.WebRequest.Create(this.ServiceEndpoint);

            string authInfo = clientId + ":" + clientSecrets;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

            wReq.Method = "POST";
            wReq.Headers["Authorization"] = @"Basic " + authInfo;

            string rospPostData = @"grant_type=password&username=" + roUserName + "&password=" + roPassword + "";

            byte[] byteArray = this.ContentEncoding.GetBytes(rospPostData);
            // Set the ContentType property of the WebRequest.
            wReq.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            wReq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = wReq.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.Z
            dataStream.Close();

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
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            throw new Exception(reader.ReadToEnd(), wex);
                        }
                    }
                }

                throw;
            }

            System.IO.Stream respStream = wResp.GetResponseStream();

            // Dim reader As StreamReader = New StreamReader(respStream)
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
            {
                string str = reader.ReadToEnd();

                return new UAOAuthToken(str);
            }
        }
    }
}
