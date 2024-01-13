using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Service;
using Microsoft.Identity.Web;
using Authentication.Data;
using Authentication.Areas.Identity.Data;
using Microsoft.Identity.Web.UI;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

if(!builder.Environment.IsDevelopment())
{
    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
}


builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
builder.Services.AddTransient<IProductService, ProductService>();

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

var connectionStringIdentity = builder.Configuration.GetConnectionString("AuthenticationContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthenticationContextConnection' not found.");

builder.Services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<AuthenticationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationContext>();

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");
// Add services to the container.
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();


var app = builder.Build();
    
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationContext>();
        
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
