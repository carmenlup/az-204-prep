using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace StorageAccountApp
{
    public class BloblService
    {

        string _connectionString = "";
        string _containerName = "test-container";

        public async Task RunBlobServiceCommandsAsync()
        {
            await CreateContainerAsync(_containerName);
            await UploadFileAsync();
            await ListAllBlobsAsync();
            await DownloadFirstBlobAsync();
        }

        private async Task DownloadFirstBlobAsync()
        {
            var downloadFolder = "DownloadFolder";
            var containerClient = GetContainerClient();
            await foreach (var item in containerClient.GetBlobsAsync())
            {
                var firstBlobName = item.Name;
                var blobClient = containerClient.GetBlobClient(firstBlobName);
                string downloadPath = $"../../../{downloadFolder}/{firstBlobName}";

                await blobClient.DownloadToAsync(downloadPath);

                Console.WriteLine($"File {firstBlobName} has been downloaded. Path: {downloadPath}.");

                break;
            }

        }

        private async Task ListAllBlobsAsync()
        {
            var containerClient = GetContainerClient();
            AsyncPageable<BlobItem> blobs = containerClient.GetBlobsAsync();
            await foreach (var blob in blobs)
            {
                Console.WriteLine($"Found blob: {blob.Name}");
            }
            Console.WriteLine();
        }

        private async Task UploadFileAsync()
        {
            string blobName = $"File1-{DateTime.Now.ToString("HHmmss")}.txt";
            string filePath = "FilesToUpload/File1.txt";

            var containerClient = GetContainerClient();
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(filePath);
            Console.WriteLine($"The file {blobName} has been uploaded.");
            Console.WriteLine();
        }

        private async Task CreateContainerAsync(string containerName)
        {
            var client = GetBlobServiceClient();

            var container = client.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();    

                      
            //await client.CreateBlobContainerAsync(containerName);

            Console.WriteLine($"Container {containerName} has been created.");
            Console.WriteLine();
        }



        BlobServiceClient GetBlobServiceClient() => new BlobServiceClient(_connectionString);
        BlobContainerClient GetContainerClient() => new BlobContainerClient(_connectionString, _containerName);
   }
}
