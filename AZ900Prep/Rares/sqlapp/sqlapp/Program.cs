using Microsoft.FeatureManagement;
using sqlapp.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "";
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
