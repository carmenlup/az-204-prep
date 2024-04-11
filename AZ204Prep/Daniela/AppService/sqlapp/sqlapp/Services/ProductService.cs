namespace WebApp.Services;

using Microsoft.FeatureManagement;
using System.Data.SqlClient;
using WebApp.Models;

public class ProductService : IProductService
{
    private readonly IConfiguration configuration;
    private readonly IFeatureManager featureManager;

    //private static string db_source = "server-204.database.windows.net";
    //private static string db_user = "admin204";
    //private static string db_password = "";
    //private static string db_database = "db-204";


    public ProductService(IConfiguration _configuration, IFeatureManager _featureManager)
    {
        configuration = _configuration;
        featureManager = _featureManager;
    }
    public async Task<bool> IsBeta()
    {
        if (await featureManager.IsEnabledAsync("beta"))
        {
            return true;
        }

        return false;
    }
    private SqlConnection GetConnection()
    {
        var builder = new SqlConnectionStringBuilder();
        //builder.DataSource = db_source;
        //builder.UserID = db_user;
        //builder.Password = db_password;
        //builder.InitialCatalog = db_database;

        //return new SqlConnection(builder.ConnectionString);

        return new SqlConnection(configuration["SQLConnection"]);
    }

    public List<Product> GetProducts()
    {
        SqlConnection connection = GetConnection();
        List<Product> products = new List<Product>();

        string statement = "SELECT ProductID, ProductName, Quantity FROM Products";

        connection.Open();
        SqlCommand command = new SqlCommand(statement, connection);
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Product product = new Product()
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Quantity = reader.GetInt32(2)
                };

                products.Add(product);
            }
        }

        connection.Close();

        return products;
    }
}
