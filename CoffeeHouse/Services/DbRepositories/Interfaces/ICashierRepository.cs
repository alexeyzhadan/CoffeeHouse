using CoffeeHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface ICashierRepository : IDefaultEntityRepository<Cashier>
    {
        IQueryable<Cashier> GetAllOrderedByFullName();
        Cashier GetByUserName(string userName);
        Task<Cashier> GetByUserNameAsync(string userName);
        Cashier GetByUserNameAsTracking(string userName);
        Task<Cashier> GetByUserNameAsTrackingAsync(string userName);
    }
}