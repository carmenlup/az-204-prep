using Azure;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Text;

namespace AzureSecurity
{
    public class EncryptionKeys
    {
        
        public async Task RunCommands()
        {
            var keyClient = new KeyClient(Credentials.keyVaultUri, Credentials.clientSecretCredentials);

            var encryptionKeyName = "key1";
            Response<KeyVaultKey> encryptionKey = await keyClient.GetKeyAsync(encryptionKeyName);
            

            var textToEncrypt = "This is a secret text.";
            var cryptoClient = new CryptographyClient(encryptionKey.Value.Id, Credentials.clientSecretCredentials);
            byte[] textToBytes = Encoding.UTF8.GetBytes(textToEncrypt);

            EncryptResult encryptedResult = await cryptoClient.EncryptAsync(EncryptionAlgorithm.RsaOaep, textToBytes);

            Console.WriteLine($"\"{textToEncrypt}\" was encrypted to -> {Convert.ToBase64String(encryptedResult.Ciphertext)}");


            DecryptResult decryptedText = await cryptoClient.DecryptAsync(EncryptionAlgorithm.RsaOaep, encryptedResult.Ciphertext);

            Console.WriteLine($"The encrypted text was decrypted to -> {Encoding.UTF8.GetString(decryptedText.Plaintext)}");

        }
    }
}
