using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
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

        public Product GetById(int id)
        {
            return GetAll().SingleOrDefault(c => c.Id == id);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
        }

        public override bool Exists(Product entity)
        {
            return GetAll().Any(c => c.Id == entity.Id);
        }
    }
}