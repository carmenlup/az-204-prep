using Azure.Identity;

namespace AzureSecurity
{
    public static class Credentials
    {
        public static Uri keyVaultUri = new Uri("https://liviu-key-vault.vault.azure.net/");
        // get these values from your Application Object that will be used to access the Key Vault
        public const string tenantId = "62d18f9f-8f85-4e0b-901a-d292a89d516e";
        public const string clientId = "7e511583-ee67-4e1c-8e13-8764d928a2e0";
        public const string clientSecret = "gRc8Q~XkcdZof_IPWNOPg3.b0qUk0U2yom~P7cg3";

        public static ClientSecretCredential clientSecretCredentials => new ClientSecretCredential(Credentials.tenantId, Credentials.clientId, Credentials.clientSecret);

    }
}
