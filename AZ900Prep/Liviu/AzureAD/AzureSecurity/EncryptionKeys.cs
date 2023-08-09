using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSecurity
{
    public class EncryptionKeys
    {
        // get these values from your Application Object that will be used to access the Key Vault
        string tenantId = "62d18f9f-8f85-4e0b-901a-d292a89d516e";
        string clientId = "7e511583-ee67-4e1c-8e13-8764d928a2e0";
        string clientSecret = "";

        ClientSecretCredential clientSecretCredentials => new ClientSecretCredential(tenantId, clientId, clientSecret);

        public async Task RunCommands()
        {
            var keyVaultUri = new Uri("https://liviu-key-vault.vault.azure.net/");
            var keyClient = new KeyClient(keyVaultUri, clientSecretCredentials);

            var encryptionKeyName = "key1";
            Response<KeyVaultKey> encryptionKey = await keyClient.GetKeyAsync(encryptionKeyName);
            

            var textToEncrypt = "This is a secret text.";
            var cryptoClient = new CryptographyClient(encryptionKey.Value.Id, clientSecretCredentials);
            byte[] textToBytes = Encoding.UTF8.GetBytes(textToEncrypt);

            EncryptResult encryptedResult = await cryptoClient.EncryptAsync(EncryptionAlgorithm.RsaOaep, textToBytes);

            Console.WriteLine($"\"{textToEncrypt}\" was encrypted to -> {Convert.ToBase64String(encryptedResult.Ciphertext)}");


            DecryptResult decryptedText = await cryptoClient.DecryptAsync(EncryptionAlgorithm.RsaOaep, encryptedResult.Ciphertext);

            Console.WriteLine($"The encrypted text was decrypted to -> {Encoding.UTF8.GetString(decryptedText.Plaintext)}");

        }
    }
}
