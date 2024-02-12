//using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using SqlFunction.Services;
using System.IO;
using System.Xml.Linq;

[assembly: FunctionsStartup(typeof(SqlFunction.Startup))]

namespace SqlFunction;

public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        FunctionsHostBuilderContext context = builder.GetContext();

        builder.ConfigurationBuilder
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        FunctionsHostBuilderContext context = builder.GetContext();
        
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IDbService>((s) =>
        {
            return new DbService();
        });

        //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
    }
}

