using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
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

        public Category GetById(int id)
        {
            return GetAll().SingleOrDefault(c => c.Id == id);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
        }

        public override bool Exists(Category entity)
        {
            return GetAll().Any(c => c.Id == entity.Id);
        }
    }
}