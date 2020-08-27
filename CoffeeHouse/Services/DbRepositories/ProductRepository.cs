using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    }
}