using CoffeeHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface ICashierRepository : IRepository<Cashier>
    {
        IQueryable<Cashier> GetAllOrderedByFullName();
        Cashier GetById(int id);
        Task<Cashier> GetByIdAsync(int id);
    }
}