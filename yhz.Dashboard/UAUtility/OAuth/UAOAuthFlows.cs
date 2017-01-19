
namespace UAUtility.OAuth
{
    /// <summary>
    /// UA OAuth 提供的2种验证流程
    /// </summary>
    public enum UAOAuthFlows : byte
    {
        /// <summary>
        /// 简单授权模式
        /// </summary>
        ImplicitGrantFlow = 0,
        /// <summary>
        /// 资源所有者密码授权模式
        /// </summary>
        ResourceOwnerPasswordCredential = 1
    }
}
