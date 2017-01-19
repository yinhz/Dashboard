
using UAUtility.OAuth;
namespace UAUtility.OData
{
    /// <summary>
    /// UA 服务请求接口
    /// </summary>
    public static class UARequestServiceFactory
    {
        public static IUARequestService GetService()
        {
            return new UAODataRequestService(UAOAuthFactory.GetService(), UAUtilityConfig.UA_ODATA_ENDPOINT);
        }
    }
}
