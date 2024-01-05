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
Console.WriteLine("**********************");
Console.WriteLine("* Create a container *");
Console.WriteLine("**********************");
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

Console.WriteLine("**********************");
Console.WriteLine("* Upload a blob      *");
Console.WriteLine("**********************");

var blobName = "file2.txt";
string filePath = "C:\\Personal\\Learning\\Courses\\Technical\\AZ 204\\file2.txt";
//Upload a file on blob
var blobContainerClient = new BlobContainerClient(storageConnectionString, containerName);

// Get a reference to a blob named "sample-file" in a container named "sample-container"
BlobClient blob = blobContainerClient.GetBlobClient(blobName);
await blob.UploadAsync(filePath, true);

Console.WriteLine("**********************");
Console.WriteLine("* List blobs      *");
Console.WriteLine("**********************");
// Print out all the blob names
foreach (BlobItem blobItem in blobContainerClient.GetBlobs())
{
    Console.WriteLine(blobItem.Name);
}

Console.WriteLine("Run the host");
await host.RunAsync();





