using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace azfunction
{
    public static class AddProduct
    {
        [FunctionName("AddProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var product = JsonConvert.DeserializeObject<Product>(requestBody);

            SqlConnection _connection = DbConnection.GetConnection();
            _connection.Open();

            var statement = "INSERT INTO Products(ProductId,ProductName,Quantity) VALUES(@param1,@param2,@param3)";
            using (var command = new SqlCommand(statement, _connection))
            {
                command.Parameters.Add("@param1", System.Data.SqlDbType.Int).Value = product.ProductID;
                command.Parameters.Add("@param1", System.Data.SqlDbType.VarChar).Value = product.ProductName;
                command.Parameters.Add("@param1", System.Data.SqlDbType.Int).Value = product.Quantity;

                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }

            return new OkObjectResult("Product added");
        }
    }
}
