using System;
using System.Data.SqlClient;

namespace sqlfunction
{
    public static class DbConnection
    {
        public static SqlConnection GetConnection()
        {
            string connection_string = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_SQLConnectionString");
            return new SqlConnection(connection_string);
        }
    }
}
