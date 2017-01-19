using System.Text;

namespace UAUtility.OAuth
{
    /// <summary>
    /// UA开放授权服务
    /// </summary>
    public interface IUAOAuthService
    {
        string ServiceEndpoint { get; }

        Encoding ContentEncoding { get; }

        /// <summary>
        /// 获取 Token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecrets"></param>
        /// <returns></returns>
        UAOAuthToken GetToken(string clientId = "123", string clientSecrets = "123");

        /// <summary>
        /// 获取 Token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecrets"></param>
        /// <returns></returns>
        UAOAuthToken GetToken(string roUserName, string roPassword, string clientId = "123", string clientSecrets = "123");

        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecrets"></param>
        /// <param name="oldToken"></param>
        /// <returns></returns>
        UAOAuthToken RefreshToken(UAOAuthToken oldToken,string clientId = "123", string clientSecrets = "123");
    }
}
