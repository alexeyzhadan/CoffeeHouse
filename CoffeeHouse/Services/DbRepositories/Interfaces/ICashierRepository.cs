using CoffeeHouse.Models;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface ICashierRepository : IDefaultEntityRepository<Cashier>
    {
        IQueryable<Cashier> GetAllOrderedByFullName();
    }
}