using Azure.Identity;
using Azure.Storage.Blobs;

string tenantId = "47dfbe1e-652d-4398-a0ad-aec0321256f2";
string clientid = "dd64e704-21f6-4a55-affc-57eecb44eeb6";
string clientsecret = "b1b8Q~W5EEpo1abeqoZ8Xe9sSu8CT6xHn1qwhcHQ";
ClientSecretCredential secret = new ClientSecretCredential(tenantId, clientid, clientsecret);

string blobUri = @"https://testdaniela.blob.core.windows.net/test/AZ-900 Daniela Cretu.pdf";
string filePath = "d:\\lucru\\script.pdf";

BlobClient blob = new BlobClient(new Uri(blobUri), secret);

await blob.DownloadToAsync(filePath);

Console.WriteLine("Download blob");
