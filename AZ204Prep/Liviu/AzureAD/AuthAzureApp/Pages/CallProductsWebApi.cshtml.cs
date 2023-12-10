using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using Microsoft.Extensions.Http;
using AuthAzureApp.Models;

namespace AuthAzureApp.Pages
{
    public class CallProductsWebApi : PageModel
    {
        public ITokenAcquisition TokenAcquisition { get; }
        public IEnumerable<Product> Products; 
        public CallProductsWebApi(ITokenAcquisition tokenAcquisition)
        {
            TokenAcquisition = tokenAcquisition;
        }


        public async Task OnGet()
        {
            var scope = new string[] { "api://13ec6c37-c61a-4755-ac0a-dd650def2153/Products.Read" };
            string credentials = await TokenAcquisition.GetAccessTokenForUserAsync(scope);
           
            var httpClient  = new HttpClient();
            httpClient.BaseAddress = new Uri("https://products-api-liviu.azurewebsites.net");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials);

            Products = await httpClient.GetFromJsonAsync<IEnumerable<Product>>("/api/products");
        }
    }
}
