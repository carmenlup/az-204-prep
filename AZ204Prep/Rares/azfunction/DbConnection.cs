using System.Data.SqlClient;
using System;

namespace azfunction
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
