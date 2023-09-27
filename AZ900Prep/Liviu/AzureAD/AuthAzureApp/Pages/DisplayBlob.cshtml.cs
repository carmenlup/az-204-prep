using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;

namespace AuthAzureApp.Pages
{
    public class DisplayBlobModel : PageModel
    {
        public ITokenAcquisition TokenAcquisition { get; }
        public string? FileContent; 
        public DisplayBlobModel(ITokenAcquisition tokenAcquisition)
        {
            TokenAcquisition = tokenAcquisition;
        }


        public async Task OnGet()
        {
            string[] scopes = new string[] { "https://storage.azure.com/user_impersonation" };


            var credentials = new TokenAcquisitionTokenCredential(TokenAcquisition);
            Uri blobUri = new Uri("https://csb100320026cc855a7.blob.core.windows.net/test-container/File1-000703.txt");


            var client = new BlobClient(blobUri, credentials);
            var content = await client.DownloadContentAsync();
            FileContent = content.Value.Content.ToString();

        }
    }
}
