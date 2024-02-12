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
        public SqlConnection GetConnection()
        {
            string connectionString = "";
            return new SqlConnection(connectionString);
        }
    }
}
