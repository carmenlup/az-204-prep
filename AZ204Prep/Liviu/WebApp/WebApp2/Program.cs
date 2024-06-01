using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using WebApp2.Settings;

namespace WebApp2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Configuration
                //.AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>();

            // Retrieve the connection string for Azure App Config
            string connectionString = builder.Configuration.GetConnectionString("AppConfig");

            // Load configuration from Azure App Configuration
            //builder.Configuration.AddAzureAppConfiguration(connectionString);

            // Load configuration from Azure App Configuration
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString).UseFeatureFlags();
                options.ConfigureRefresh((c) =>
                {
                    c.Register("LiviuWebApp:StyleSettings:IsEnabled", true);
                });
                // Load all feature flags with no label
               // options.UseFeatureFlags();
            });



            builder.Services.AddRazorPages();

            // Bind configuration "TestApp:Settings" section to the Settings object
            builder.Services.Configure<StyleSettings>(builder.Configuration.GetSection("LiviuWebApp:StyleSettings"));

            builder.Services.AddAzureAppConfiguration();
            // Add feature management to the container of services.
            builder.Services.AddFeatureManagement();

            var app = builder.Build();

            app.UseAzureAppConfiguration(); 

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
