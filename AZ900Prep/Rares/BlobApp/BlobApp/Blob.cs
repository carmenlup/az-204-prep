using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlobApp
{
    public class Blob
    {
        private const string connectionString = "";
        private const string containerName = "data1";
        private const string blobName = "journal.txt";
        private const string filePath = "C:\\temp\\journal.txt";
        private const string downloadPath = "C:\\temp1\\journal.txt";
        BlobClient? blob = null;

        public async Task HandleBlobs()
        {
            CreateBlobContainer();
            await UploadFileToBlob();
            await DownloadFileFromBlob();
            await SetBlobMetaData(blob);

            await GetBlobMetaData();
            Console.ReadLine();
        }

        async Task GetBlobMetaData()
        {
            BlobProperties blobPRoperties = await blob.GetPropertiesAsync();
            foreach (var property in blobPRoperties.Metadata)
            {
                Console.WriteLine($"The key is {property.Key} and value is {property.Value}");
            }
        }


        private void CreateBlobContainer()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            Console.WriteLine("Creating the container");

            var container = blobServiceClient.GetBlobContainerClient(containerName);
            container.CreateIfNotExists();
        }

        private async Task UploadFileToBlob()
        {

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(filePath, true);
        }

        private async Task DownloadFileFromBlob()
        {
            blob = new BlobClient(connectionString, containerName, blobName);

            await blob.DownloadToAsync(downloadPath);
            Console.WriteLine("Downloaded");
        }

        async Task SetBlobMetaData(BlobClient blob)
        {
            IDictionary<string, string> metaData = new Dictionary<string, string>
            {
                { "Department", "Finance" },
                { "Application", "A" }
            };

            await blob.SetMetadataAsync(metaData);
            Console.WriteLine("Metadata added");
        }
    }

}
