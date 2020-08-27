using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories
{
    public class CategoryRepository : DefaultRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public IQueryable<Category> GetAllOrderedByName()
        {
            return _db.Set<Category>()
                .OrderBy(c => c.Name)
                .AsNoTracking();
        }
    }
}