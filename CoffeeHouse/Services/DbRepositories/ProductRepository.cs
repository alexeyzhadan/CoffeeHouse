using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class ProductRepository : DefaultRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public IQueryable<Product> GetAllWithCategoryOrderedByCategoryNameThenByName()
        {
            return _db.Set<Product>()
                .Include(p => p.Category)
                .OrderBy(p => p.Category.Name)
                    .ThenBy(p => p.Name)
                .AsNoTracking();
        }

        public Product GetByIdWithCategory(int id)
        {
            return _db.Set<Product>()
                .Include(p => p.Category)
                .AsNoTracking()
                .SingleOrDefault(p => p.Id == id);
        }

        public async Task<Product> GetByIdWithCategoryAsync(int id)
        {
            return await Task.Run(() => GetByIdWithCategory(id));
        }
    }
}