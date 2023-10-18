// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

var host = builder.Build();

/*
 * throught visual studion key vault management can use DefaultAzureCredential for conectivity
 * For more info see documentation (Readme.md) provided under Azure.Security.KeyVault.Keys package
 * 
 * Code example to get the key from key vault service:
 * 
 * var client = new KeyClient(vaultUri: new Uri(vaultUrl), credential: new DefaultAzureCredential());
 * var keyVault = new KeyClient(Uri, client);
 */

// add your implementation here
// get configurations
var config = host.Services.GetRequiredService<IConfiguration>();

var configUri = config["Uri"];
if (configUri != null)
{
    var Uri = new Uri(configUri);
    var tenant = config["TenantId"];
    var client = config["ClientId"];
    var secret = config["ClientSecret"];

    // connect to vault
    var clientSecretCredential = new ClientSecretCredential(tenant, client, secret);

    // get the key
    var keyVaultClient = new KeyClient(Uri, clientSecretCredential);

    var key = keyVaultClient.GetKey("app-key-cd");

    var cryptoClient = new CryptographyClient(key.Value.Id, clientSecretCredential);

    byte[] textToBytes = Encoding.UTF8.GetBytes("My text for encrypt / decrypt");

    var result = cryptoClient.Encrypt(EncryptionAlgorithm.RsaOaep, textToBytes);

    Console.WriteLine($"Encrypted text is \n {Convert.ToBase64String(result.Ciphertext)}");

    var ciphertext = result.Ciphertext;

    var decryptedResult = cryptoClient.Decrypt(EncryptionAlgorithm.RsaOaep, ciphertext);

    Console.WriteLine($"My original text is \n {Encoding.UTF8.GetString(decryptedResult.Plaintext)}");
}

await host.RunAsync();