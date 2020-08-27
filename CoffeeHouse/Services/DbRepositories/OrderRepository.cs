using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories
{
    public class OrderRepository : DefaultRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public IQueryable<Order> GetAllWithCashiersAndClientsOrderByDate()
        {
            return _db.Set<Order>()
                .Include(o => o.Cashier)
                .Include(o => o.Client)
                .OrderByDescending(o => o.Date)
                .AsNoTracking();
        }
    }
}