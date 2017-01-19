
namespace UAUtility.OAuth
{
    public static class UAOAuthFactory
    {
        public static IUAOAuthService GetService()
        {
            return GetService(UAOAuthFlows.ResourceOwnerPasswordCredential);
        }

        public static IUAOAuthService GetService(UAOAuthFlows flow)
        {
            switch (flow)
            {
                case UAOAuthFlows.ImplicitGrantFlow: 
                    return null;
                case UAOAuthFlows.ResourceOwnerPasswordCredential:
                    return new UAOAuthROPCService(UAUtility.UAUtilityConfig.UA_OAUTH_ROPC_POINT);
                default:
                    return new UAOAuthROPCService(UAUtility.UAUtilityConfig.UA_OAUTH_ROPC_POINT);
            }
        }
    }
}
