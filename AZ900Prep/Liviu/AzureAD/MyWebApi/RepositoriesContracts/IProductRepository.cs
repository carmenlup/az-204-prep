using MyWebApi.Models;

namespace MyWebApi.RepositoriesContracts
{
    public interface IProductRepository
    {
        Task<Product> GetById(int id);
        Task<IEnumerable<Product>> GetAll();

    }
}
