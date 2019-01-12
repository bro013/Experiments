using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;

namespace OmniaTransferLogDev.Security
{
    public static class KeyVault
    {
        /// <summary>
        /// Gets secret from key vault with url
        /// </summary>
        /// <returns></returns>
        public static async Task<SecretBundle> GetSecretAsync(string url)
        {
            // Pulls the connection string from Azure Key Vault
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var secret = await keyVaultClient.GetSecretAsync(url).ConfigureAwait(false);
            return secret;
        }
    }
}
