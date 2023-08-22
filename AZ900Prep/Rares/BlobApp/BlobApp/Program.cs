using Azure.Storage.Blobs;
using System.ComponentModel;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=az204bd17;AccountKey=M14U6hJmEbHxuBZ7XuMPpiQDz9w9fegEwD95ux2jAYht8iuySlNwwyA9+trnAP4e+ki35DzTi5Ia+AStrU4Q0w==;EndpointSuffix=core.windows.net";
string containerName = "data1";
var blobName = "journal.txt";
var filePath = "C:\\temp\\journal.txt";

BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

Console.WriteLine("Creating the container");

var container = blobServiceClient.GetBlobContainerClient(containerName);
container.CreateIfNotExists();


///*
//If you want to specify properties for the container

//await blobServiceClient.CreateBlobContainerAsync(containerName,Azure.Storage.Blobs.Models.PublicAccessType.Blob);
//*/
//Console.WriteLine("Container creation complete");

BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
await blobClient.UploadAsync(filePath, true);
