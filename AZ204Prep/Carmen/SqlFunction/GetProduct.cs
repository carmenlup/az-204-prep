using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace SqlFunction
{
    public static class GetProduct
    {
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> RunProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get data from the database");
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT Id,Name,Quantity from Products";
            SqlConnection _connection = GetConnection();

            _connection.Open();

            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        Name = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };

                    _product_lst.Add(_product);
                }
            }
            _connection.Close();

            return new OkObjectResult(_product_lst);
        }

        private static SqlConnection GetConnection()
        {
            string connectionString = "Server=tcp:appserver73.database.windows.net,1433;Initial Catalog=WebApplicationDb;Persist Security Info=False;User ID=sqladmin;Password=MySqlServer@k20;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
            return new SqlConnection(connectionString);
        }

        [FunctionName("GetProduct")]
        public static async Task<IActionResult> RunProduct(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var productId = int.Parse(req.Query["id"]);
            string _statement = $"SELECT Id,Name,Quantity from Products where Id = {productId}";
            SqlConnection _connection = GetConnection();

            _connection.Open();
            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            var product = new Product();
            var response = new Product();
            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                _reader.Read();
                product.Id = _reader.GetInt32(0);
                product.Name = _reader.GetString(1);
                product.Quantity = _reader.GetInt32(2);
                response = product;

            }
            _connection.Close();
            return new OkObjectResult(response);
        }


    }
}
