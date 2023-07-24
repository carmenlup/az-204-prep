using Microsoft.FeatureManagement;
using sqlapp.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "Endpoint=https://az-204-app-config.azconfig.io;Id=Pgu3;Secret=fxG4Ef8tEMUjtEE94634ZsuvTcqfEoMU4PqN+OCZ0/Q=";
builder.Host.ConfigureAppConfiguration(builder =>
{
    builder.AddAzureAppConfiguration(options =>
    options.Connect(connectionString).UseFeatureFlags());
});
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddRazorPages();
builder.Services.AddFeatureManagement();
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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
