using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Internal;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Service;
using WebApp.TestAutomation.BaseClasses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
public class DatabaseConnection
{
    private IProductService _productService;
    private IConfiguration _configuration;
    public DatabaseConnection(IConfiguration configuration, IProductService product)
    {
        _configuration = configuration;
        _productService = product;
    }
    public SqlConnection GetConnection()
    {
        SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DbConnectionString"]);
        connection.Open();
        return connection;
    }

    public void CloseConnection(SqlConnection connection)
    {
        connection.Close();
    }

    public string GetQuery(string queryName)
    {
        var queries = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"Queries\Queries.json"));
        return queries[queryName];
    }

    public List<Product> ExecuteQuery()
    {
        var result = _productService.GetProduct();
        return result; 
    }

}