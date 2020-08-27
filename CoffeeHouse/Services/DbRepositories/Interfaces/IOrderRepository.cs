using CoffeeHouse.Models;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IOrderRepository : IDefaultEntityRepository<Order>
    {
        IQueryable<Order> GetAllWithCashiersAndClientsOrderByDate();
    }
}