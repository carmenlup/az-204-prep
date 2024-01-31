using Dapper;
using MyWebApi.Models;
using MyWebApi.RepositoriesContracts;
using System.Data.SqlClient;

namespace MyWebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetAll()
        {
            using (var conn = GetConnection())
            {
                IEnumerable<Product> products = await conn.QueryAsync<Product>("select * from Products");

                return products;
            }
        }

        public async Task<Product> GetById(int id)
        {
            using (var conn = GetConnection())
            {
                Product product = await conn.QuerySingleOrDefaultAsync<Product>("select * from Products where ProductId = @pid", new { pid = id });

                return product;
            }
        }

        private SqlConnection GetConnection()
        {
            string connectionString = "";
            return new SqlConnection(connectionString);
        }

    }
}
