// See https://aka.ms/new-console-template for more information
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

Console.WriteLine("Create a host for console app");

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

var host = builder.Build();

Console.WriteLine("Host created and User Secret config added to the builder");

var storageConnectionString = builder.Configuration["StorageAccount:ConnectionString"];

// Application code should start here.
string containerName = "newcontainer";
BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

// Create a Container
var containers = blobServiceClient.GetBlobContainers();
Console.WriteLine(containers.Count());
var container = containers.FirstOrDefault(c => c.Name == containerName);
if (container == null)
{
    blobServiceClient.CreateBlobContainer(containerName, PublicAccessType.Blob);
    Console.WriteLine($"The container {container.Name} was created"); 
}
else
    Console.WriteLine($"The container {container.Name} already exist");

//Upload a file on blob


Console.WriteLine("Run the host");
await host.RunAsync();





