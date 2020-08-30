using CoffeeHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IProductRepository : IDefaultEntityRepository<Product>
    {
        IQueryable<Product> GetAllWithCategoryOrderedByCategoryNameThenByName();
        Product GetByIdWithCategory(int id);
        Task<Product> GetByIdWithCategoryAsync(int id);
    }
}