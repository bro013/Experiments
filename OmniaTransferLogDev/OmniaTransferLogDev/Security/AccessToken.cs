using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace OmniaTransferLogDev.Security
{
    public static class AccessToken
    {
        public static string  GetAccessToken(string servicePrincipalId, string servicePrincipalKey,
            string tenantId, string resource)
        {

            ClientCredential cred = new ClientCredential(servicePrincipalId, servicePrincipalKey);
            var authority = string.Format("https://login.windows.net/{0}", tenantId);
            var authenticationContext = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var authenticationResult = authenticationContext.AcquireTokenAsync(resource, cred).Result;
            var token = authenticationResult.AccessToken;
            return token;

        }
    }
}
