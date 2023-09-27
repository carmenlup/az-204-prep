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

var storageConnectionString = builder.Configuration.GetConnectionString("StorageAccountConnectionString");

// Application code should start here.
string containerName = "newcontainer";
BlobServiceClient blobServiceCLient = new BlobServiceClient(storageConnectionString);

var containers = blobServiceCLient.GetBlobContainers();
Console.WriteLine(containers.Count());
var container = containers.FirstOrDefault(c => c.Name == containerName);
if (container == null)
    blobServiceCLient.CreateBlobContainer(containerName, PublicAccessType.Blob);

Console.WriteLine("Run the host");
await host.RunAsync();





