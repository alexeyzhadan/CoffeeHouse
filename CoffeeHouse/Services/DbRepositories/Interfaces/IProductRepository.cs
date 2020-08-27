using CoffeeHouse.Models;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IProductRepository : IDefaultEntityRepository<Product>
    {
        IQueryable<Product> GetAllWithCategoryOrderedByCategoryNameThenByName();
    }
}