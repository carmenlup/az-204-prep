using Microsoft.FeatureManagement;
using sqlapp.Models;
using System.Data.SqlClient;
using System.Text.Json;

namespace sqlapp.Services
{

    // This service will interact with our Product data in the SQL database
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _featureManager;

        public ProductService(IConfiguration configuration, IFeatureManager featureManager)
        {
            _configuration = configuration;
            _featureManager = featureManager;
        }

        public async Task<List<Product>> GetProducts()
        {
            var functionUrl = "https://az204-function4pp.azurewebsites.net/api/GetProducts?code=ITp6b05GpmaRHvdiITZKOtZtxYNJumZggs2qKsu-U3JJAzFur-_PqQ==";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(functionUrl);
                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Product>>(content);
            }
        }

        public async Task<bool> IsBeta()
        {
            if (await _featureManager.IsEnabledAsync("beta"))
            { return true; }
            else { return false; }
        }

        private SqlConnection GetConnection()
        {

            return new SqlConnection(_configuration["SQLConnection"]);
        }
    }
}

