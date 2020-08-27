using CoffeeHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> GetAllWithCategoryOrderedByCategoryNameThenByName();
        Product GetById(int id);
        Task<Product> GetByIdAsync(int id);
    }
}