using CoffeeHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IQueryable<Category> GetAllOrderedByName();
        Category GetById(int id);
        Task<Category> GetByIdAsync(int id);
    }
}