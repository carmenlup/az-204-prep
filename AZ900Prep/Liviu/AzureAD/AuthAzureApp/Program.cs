using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthAzureApp.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthAzureAppContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthAzureAppContextConnection' not found.");

builder.Services.AddDbContext<AuthAzureAppContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuthAzureAppContext>();

// Add services to the container.
//var scopes = new string[] { "https://storage.azure.com/user_impersonation" };
var scopes = new string[] { "api://13ec6c37-c61a-4755-ac0a-dd650def2153/Products.Read" };
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
    .EnableTokenAcquisitionToCallDownstreamApi(scopes)
    .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
});
app.MapRazorPages();

app.Run();
