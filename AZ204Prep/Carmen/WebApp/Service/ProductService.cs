using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Data.SqlClient;
using WebApp.Data.Entities;

namespace WebApp.Service
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration Configuration;
        private string _dbConnectionString;
        public ProductService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            // connection from secrets -> key vault
            //var uriString = Configuration["KeyVault:Uri"];
            //var uri = new Uri(uriString);
            //var tenantId = Configuration["KeyVault:TenantId"];
            //var clientId = Configuration["KeyVault:ClientId"];
            //var clientSecret = Configuration["KeyVault:ClientSecret"];
            //var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            //var keyVaultClient = new SecretClient(uri, clientSecretCredential);
            //var dbConnectionString = keyVaultClient.GetSecret("connection-string");
            //_dbConnectionString = dbConnectionString.Value.Value;
            return new SqlConnection(Configuration["connection-string"]);
        }

        public List<Product> GetProduct()
        {
            var dbConnection = Configuration.GetConnectionString("DbConnectionString");
            SqlConnection connection = GetConnection(); //- use get connection for azure key vault usage
            var productList = new List<Product>();
            var statement = "SELECT Id, Name, Quantity from Products";
            connection.Open();

            SqlCommand cmd = new SqlCommand(statement, connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new Product()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Quantity = reader.GetInt32(2)
                };

                productList.Add(product);
            }

            connection.Close();

            return productList;
        }

    }
}
