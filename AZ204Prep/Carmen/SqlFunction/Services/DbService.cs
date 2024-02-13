using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFunction.Services
{
    public class DbService : IDbService
    {
        public SqlConnection GetConnection(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DbConnectionString");
            return new SqlConnection(connectionString);
        }
    }
}
