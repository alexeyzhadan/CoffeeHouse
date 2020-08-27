using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Services.DbRepositories
{
    public class OrderProdRepository : BaseRepository<OrderProd>, IOrderProdRepository
    {
        public OrderProdRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public override bool Exists(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }
    }
}