using System.Reflection;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using WebApp.Data;
using WebApp.Service;

var builder = WebApplication.CreateBuilder(args);
var envUri = Environment.GetEnvironmentVariable("VaultUri");

// ef migration is not working with Environment.GetEnvironmentVariable("VaultUri")
if (string.IsNullOrEmpty(envUri))
    envUri = builder.Configuration["KeyVault:Uri"];

var keyVaultEndpoint = new Uri(envUri);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
builder.Services.AddTransient<IProductService, ProductService>();

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

// Configuration
if (!builder.Environment.IsDevelopment())
{
    var azureAppConfigurationString = builder.Configuration["AzureAppConfiguration"];
    builder.Configuration.AddAzureAppConfiguration(options =>
        options
            .Connect(new Uri(azureAppConfigurationString!), new DefaultAzureCredential())
            .ConfigureRefresh(refreshOptions =>
                refreshOptions.Register("TestApp:Sentinel", refreshAll: true))
            .UseFeatureFlags());
}

if(builder.Environment.IsDevelopment())
{

    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(builder.Configuration["ApiConfig:ConnectionStrings"])
            .ConfigureRefresh(refreshOptions =>
                refreshOptions.Register("TestApp:Sentinel", refreshAll: true));
        options.UseFeatureFlags();
    });

}

// Add feature management to the container of services.
builder.Services.AddFeatureManagement();

//var connectionStringIdentity = builder.Configuration.GetConnectionString("AuthenticationContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthenticationContextConnection' not found.");

//builder.Services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddDefaultIdentity<AuthenticationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationContext>();

//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");
// Add services to the container.
builder.Services.AddControllersWithViews();//.AddMicrosoftIdentityUI();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationContext>();
        // Ensure the database is created
        // Should not be used in production because bypass the migrations
        //context.Database.EnsureCreated();

        // Apply any pending migrations
        context.Database.Migrate();

        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//app.UseAuthentication();
app.UseAuthorization();
//app.UseAzureAppConfiguration();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
