using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountApp
{
    public class GrantAccessWithApplicationObjects
    {
        // 1st Create an Application Object. From AD -> App Registrations -> New Registration

        // Get them from your Application Object - overview
        string clientId = "";
        string tenantId = "";
        // Get it from Application Object -> Certificates & Secrets -> Add new Secret -> Copy the value
        string clientSecret = "";

        string blobUrl = "https://csb100320026cc855a7.blob.core.windows.net/test-container/File1-002123.txt";

        public async Task RunBlobCommandsUsingAppObjectsAuthentication()
        {
            // install Azure.Identity package
            ClientSecretCredential clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var blobClient = new BlobClient(new Uri(blobUrl), clientSecretCredential);

            string downloadPath = $"../../../DownloadFolder/{blobClient.Name}";
            await blobClient.DownloadToAsync(downloadPath);

            Console.WriteLine($"The file has been downloaded using the Application Object authentication method. Path: {downloadPath}");
            Console.WriteLine();

        }
    }
}
