using sqlapp.Models;

namespace sqlapp.Services
{
    public interface IProductService
    {
        Task<bool> IsBeta();
        List<Product> GetProducts();
    }
}