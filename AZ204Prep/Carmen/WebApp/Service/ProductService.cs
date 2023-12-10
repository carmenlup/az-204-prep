using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Data.SqlClient;
using WebApp.Data.Entities;

namespace WebApp.Service
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration Configuration;
        public ProductService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            // connection from secrets -> key vault
            var uri = new Uri(Configuration["Uri"]);
            var tenantId = Configuration["TenantId"];
            var clientId = Configuration["ClientId"];
            var clientSecret = Configuration["clientSecret"];
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var keyVaultClient = new SecretClient(uri, clientSecretCredential);
            var dbConnectionString = keyVaultClient.GetSecret("db-connection-string");
            return new SqlConnection(dbConnectionString.Value.Value);
        }

        public List<Product> GetProduct()
        {
            var dbConnection = Configuration.GetConnectionString("DbConnectionString");
            SqlConnection connection = new SqlConnection(dbConnection);// GetConnection();
            var productList = new List<Product>();
            string statement = "SELECT Id, Name, Quantity from Products";
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
