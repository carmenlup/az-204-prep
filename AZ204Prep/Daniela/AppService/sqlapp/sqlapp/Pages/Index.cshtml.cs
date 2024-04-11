using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<Product> Products;
         private readonly IProductService _productService;

        public bool IsBeta;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public void OnGet()
        {
            IsBeta = _productService.IsBeta().Result;
            //Products = _productService.GetProducts();

           // ProductService productService = new ProductService();
            Products = _productService.GetProducts();

        }
    }
}