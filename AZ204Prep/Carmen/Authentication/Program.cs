using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Authentication.Data;
using Authentication.Areas.Identity.Data;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthenticationContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthenticationContextConnection' not found.");

builder.Services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AuthenticationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationContext>();

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");

// Add services to the container.
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

var app = builder.Build();

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
