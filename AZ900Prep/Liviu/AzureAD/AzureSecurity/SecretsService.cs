using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSecurity
{
    public class SecretsService
    {
        
        public async Task RunCommands()
        {
            await Task.Yield();

            var secretClient = new SecretClient(Credentials.keyVaultUri, Credentials.clientSecretCredentials);
            string secretName = "MySqlConnection";

            var secret = await secretClient.GetSecretAsync(secretName);
            string connectionString = secret.Value.Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlCommand =  new SqlCommand("select * from Products", connection);
                SqlDataReader result = await sqlCommand.ExecuteReaderAsync();
                while (result.Read())
                {
                    Console.WriteLine($"Product: {result.GetInt32(0)} - {result.GetString(1)} - {result.GetInt32(2)}");
                }
            }
        }
    }
}
