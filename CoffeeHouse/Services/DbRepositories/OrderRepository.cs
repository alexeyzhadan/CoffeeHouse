using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Services.DbRepositories
{
    public class OrderRepository : DefaultRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}