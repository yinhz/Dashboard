using Newtonsoft.Json.Linq;
using System;

namespace UAUtility.OAuth
{
    /// <summary>
    /// UA OAuth Token
    /// 
    /// Example:
    /// 
    /// HTTP/1.1 200 OK
    /// Content-Type: application/json;charset=UTF-8
    /// Cache-Control: no-store
    /// Pragma: no-cache
    /// {
    ///     "access_token":"2YotnFZFEjr1zCsicMWpAA",
    ///     "token_type":"example",
    ///     "expires_in":3600,
    ///     "refresh_token":"tGzv3JOkF0XG5Qx2TlKWIA",
    ///     "example_parameter":"example_value"
    /// }
    /// </summary>
    public class UAOAuthToken
    {
        public override string ToString()
        {
            return this.AccessToken;
        }

        public const string UA_RES_AUTH_ACCESS_TOKEN_KEY    =       "access_token";
        public const string UA_RES_TOKEN_TYPE_KEY           =       "token_type";
        public const string UA_RES_EXPIRES_IN_KEY           =       "expires_in";
        public const string UA_RES_REFRESHTOKEN_KEY         =       "refresh_token";

        public void Refresh()
        { }

        public UAOAuthToken()
        {

        }

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="responseStream">在 UA OAuth 服务申请 Token 后返回的 内容</param>
        public UAOAuthToken(string responseStream)
        {
            if (string.IsNullOrEmpty(responseStream))
                throw new ArgumentNullException("responseStream");

            JObject jo = JObject.Parse(responseStream);

            this.AccessToken = jo["access_token"].Value<string>();
            this.TokenType = jo["token_type"].Value<string>();
            this.ExpiresIn = jo["expires_in"].Value<int>();
            this.RefreshToken = jo["refresh_token"].Value<string>();
        }

        /// <summary>
        /// Access token
        /// </summary>
        public string AccessToken { get; private set; }
        /// <summary>
        /// Token type
        /// </summary>
        public string TokenType { get; private set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int ExpiresIn { get; private set; }
        /// <summary>
        /// 刷新用 token
        /// </summary>
        public string RefreshToken { get; private set; }
    }
}
