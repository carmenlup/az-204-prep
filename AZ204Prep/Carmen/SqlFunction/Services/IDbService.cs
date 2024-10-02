using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFunction.Services
{
    public interface IDbService
    {
        SqlConnection GetConnection(IConfiguration configuration);
    }
}
