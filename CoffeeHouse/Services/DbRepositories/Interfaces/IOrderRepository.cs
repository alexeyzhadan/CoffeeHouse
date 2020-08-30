using CoffeeHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IOrderRepository : IDefaultEntityRepository<Order>
    {
        IQueryable<Order> GetAllWithCashiersAndClientsOrderedByDate();
        Order GetByIdWithAllInclusiveData(int id);
        Task<Order> GetByIdWithAllInclusiveDataAsync(int id);
        Order GetByIdWithOrderProds(int id);
        Task<Order> GetByIdWithOrderProdsAsync(int id);
        Order GetByIdWithOrderProdsAndProducts(int id);
        Task<Order> GetByIdWithOrderProdsAndProductsAsync(int id);
        Order GetByIdWithOrderProdsAsTracking(int id);
        Task<Order> GetByIdWithOrderProdsAsTrackingAsync(int id);
        Order GetByIdWithClientAndCashier(int id);
        Task<Order> GetByIdWithClientAndCashierAsync(int id);
    }
}