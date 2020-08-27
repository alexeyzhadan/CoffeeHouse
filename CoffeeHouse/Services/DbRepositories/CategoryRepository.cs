using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Services.DbRepositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public override bool Exists(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}