using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Services.DbRepositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public override bool Exists(Order entity)
        {
            throw new System.NotImplementedException();
        }
    }
}