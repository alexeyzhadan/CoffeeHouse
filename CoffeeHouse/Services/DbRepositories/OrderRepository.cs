using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class OrderRepository : DefaultRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public IQueryable<Order> GetAllWithCashiersAndClientsOrderedByDate()
        {
            return _db.Set<Order>()
                .Include(o => o.Cashier)
                .Include(o => o.Client)
                .OrderByDescending(o => o.Date)
                .AsNoTracking();
        }

        public Order GetByIdWithAllInclusiveData(int id) 
        {
            return _db.Set<Order>()
                .Include(o => o.Cashier)
                .Include(o => o.Client)
                .Include(o => o.OrderProds)
                    .ThenInclude(op => op.Product)
                .AsNoTracking()
                .SingleOrDefault(o => o.Id == id);
        }

        public async Task<Order> GetByIdWithAllInclusiveDataAsync(int id)
        {
            return await Task.Run(() => GetByIdWithAllInclusiveData(id));
        }

        public Order GetByIdWithOrderProds(int id)
        {
            return _db.Set<Order>()
                .Include(o => o.OrderProds)
                .AsNoTracking()
                .SingleOrDefault(o => o.Id == id);
        }

        public async Task<Order> GetByIdWithOrderProdsAsync(int id)
        {
            return await Task.Run(() => GetByIdWithOrderProds(id));
        }

        public Order GetByIdWithOrderProdsAndProducts(int id)
        {
            return _db.Set<Order>()
                .Include(o => o.OrderProds)
                    .ThenInclude(op => op.Product)
                .AsNoTracking()
                .SingleOrDefault(o => o.Id == id);
        }

        public async Task<Order> GetByIdWithOrderProdsAndProductsAsync(int id)
        {
            return await Task.Run(() => GetByIdWithOrderProdsAndProducts(id));
        }

        public Order GetByIdWithOrderProdsAsTracking(int id)
        {
            return _db.Set<Order>()
                .Include(o => o.OrderProds)
                .SingleOrDefault(o => o.Id == id);
        }

        public async Task<Order> GetByIdWithOrderProdsAsTrackingAsync(int id)
        {
            return await Task.Run(() => GetByIdWithOrderProdsAsTracking(id));
        }
    }
}