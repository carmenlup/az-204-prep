//using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using SqlFunction.Services;
using System.Xml.Linq;

[assembly: FunctionsStartup(typeof(SqlFunction.Startup))]

namespace SqlFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IDbService>((s) =>
        {
            return new DbService();
        });

        //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
    }
}

