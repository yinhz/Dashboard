
using System.Text;
using UAUtility.OAuth;
namespace UAUtility.OData
{
    /// <summary>
    /// UA 服务请求接口
    /// </summary>
    public interface IUARequestService
    {
        /// <summary>
        /// http 内容编码
        /// </summary>
        Encoding ContentEncoding { get; }

        /// <summary>
        /// 请求令牌的服务
        /// </summary>
        IUAOAuthService UAOAuthService { get; }

        /// <summary>
        /// 执行实体查询
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        string EntityQuery(string entityName, string options = "");
        /// <summary>
        /// 执行 command
        /// </summary>
        /// <param name="cmdModel"></param>
        /// <returns></returns>
        string CallCommand(CommandModel cmdModel);
    }
}
