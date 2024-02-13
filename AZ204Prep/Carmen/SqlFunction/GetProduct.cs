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
using Microsoft.Extensions.Configuration;
using SqlFunction.Services;
using System.Net.Http;

namespace SqlFunction
{
    public class GetProduct
    {
        private readonly HttpClient _client;
        private readonly IDbService _dbService;
        private readonly IConfiguration _configuration;
      
        public GetProduct(IHttpClientFactory httpClientFactory, IDbService service, IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _dbService = service;
            _configuration = configuration;
        }

        [FunctionName("GetProducts")]
        public async Task<IActionResult> RunProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get data from the database");
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT Id,Name,Quantity from Products";
            SqlConnection _connection = _dbService.GetConnection(_configuration);

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

            return new OkObjectResult(JsonConvert.SerializeObject(_product_lst));
        }

        [FunctionName("GetProduct")]
        public async Task<IActionResult> RunProduct(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var productId = int.Parse(req.Query["id"]);
            string _statement = $"SELECT Id,Name,Quantity from Products where Id = {productId}";
            SqlConnection _connection = _dbService.GetConnection(_configuration);

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
