using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApp.TestAutomation.DbModel;

namespace WebApp.TestAutomation.Settings;

public class DatabaseConnection
{
    private readonly IConfigurationRoot _configuration;

    public DatabaseConnection(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    private string GetQuery()
    {
        var queries =
            JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"Queries\Queries.json"));
        return queries["GetProductList"];
    }

    public List<Product> GetProduct()
    {
        var queryString = GetQuery();

        var products = new List<Product>();

        var connectionString = _configuration.GetConnectionString("DbConnectionString");
        using var connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand(queryString, connection);

        connection.Open();

        using SqlDataReader rdr = command.ExecuteReader();
        while (rdr.Read())
        {
            var product = new Product
            {
                Id = Convert.ToInt32(rdr["Id"]),
                Name = rdr["Name"].ToString() ?? string.Empty,
                Quantity = Convert.ToInt32(rdr["Quantity"])
            };

            products.Add(product);
        }

        return products;
    }
}