using sqlapp.Models;

namespace sqlapp.Services
{
    public interface IProductService
    {
        Task<bool> IsBeta();
        Task<List<Product>> GetProducts();
    }
}