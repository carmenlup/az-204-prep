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
    /// <summary>
    /// This constructor is responsible for injecting the IConfigurationRoot object into the class.
    /// </summary>
    /// <param name="configuration"></param>
    public DatabaseConnection(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// This method is responsile for retrievieng a specific query from the Queries.json file.
    /// </summary>
    /// <returns>Returns query as a string</returns>
    private string GetQuery()
    {
        var queries =
            JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"Queries\Queries.json"));
        return queries["GetProductList"];
    }

    /// <summary>
    /// This method is responsible for retrieving a list of products from the database.
    /// </summary>
    /// <returns>Returns a list of products</returns>
    public List<Product> GetProduct()
    {
        // Get the query from the Queries.json file.
        var queryString = GetQuery();
        // Create a list of products.
        var products = new List<Product>();
        // Get the connection string from the appsettings.json file.
        var connectionString = _configuration.GetConnectionString("DbConnectionString");
        // Create a connection to the database. 
        using var connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand(queryString, connection);
        // Open the connection.
        connection.Open();
        // Execute the query.
        using SqlDataReader rdr = command.ExecuteReader();
        // Iterate through the results and add them to the list.
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