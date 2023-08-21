using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;

namespace AuthAzureApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        public string? AccessToken;

        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public void OnGet()
        {
           // var scopes = new string[] { "https://storage.azure.com/user_impersonation" };
           //AccessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        }
    }
}