using System.Data.SqlClient;
using WebApp.Models;

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
            return new SqlConnection(Configuration.GetConnectionString("DbConnectionString"));
        }

        public List<Product> GetProduct()
        {
            SqlConnection connection = GetConnection();
            var productList = new List<Product>();
            string statement = "SELECT ProductID, ProductName, Quantity from Products";
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
