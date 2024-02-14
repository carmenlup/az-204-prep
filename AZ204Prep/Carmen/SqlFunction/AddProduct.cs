using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlFunction.Services;
using System.Data.SqlClient;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace SqlFunction
{
    public class AddProduct
    {
        private readonly HttpClient _client;
        private readonly IDbService _dbService;
        private readonly IConfiguration _configuration;

        public AddProduct(IHttpClientFactory httpClientFactory, IDbService service,  IConfiguration configuration)
        {
            this._client = httpClientFactory.CreateClient();
            this._dbService = service;
            _configuration = configuration;
        }

        [FunctionName("AddProduct")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Product>(requestBody);
            var connection = _dbService.GetConnection(_configuration);
            connection.Open();

            string statement = "insert into Products(Name,Quantity) values (@param2, @param3)";
            using(SqlCommand cmd = new SqlCommand(statement, connection))
            {
                cmd.Parameters.Add("@param2", System.Data.SqlDbType.NVarChar).Value = data.Name;
                cmd.Parameters.Add("@param3", System.Data.SqlDbType.Int).Value = data.Quantity;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }

            return new OkObjectResult("Product added");
        }
        
    }
}
