using Azure.Identity;

namespace AzureSecurity
{
    public static class Credentials
    {
        public static Uri keyVaultUri = new Uri("https://liviu-key-vault.vault.azure.net/");
        // get these values from your Application Object that will be used to access the Key Vault
        public const string tenantId = "";
        public const string clientId = "";
        public const string clientSecret = "";

        public static ClientSecretCredential clientSecretCredentials => new ClientSecretCredential(Credentials.tenantId, Credentials.clientId, Credentials.clientSecret);

    }
}
