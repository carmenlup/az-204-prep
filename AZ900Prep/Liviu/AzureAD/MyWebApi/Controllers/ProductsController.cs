using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.RepositoriesContracts;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : Controller
    {
        public IProductRepository Repo { get; }
        public ProductsController(IProductRepository repo)
        {
            Repo = repo;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProducts()
        {
            var allProudcts = await Repo.GetAll();
            return Ok(allProudcts);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            Models.Product product = await Repo.GetById(id);
            return Ok(product);
        }

    }
}
