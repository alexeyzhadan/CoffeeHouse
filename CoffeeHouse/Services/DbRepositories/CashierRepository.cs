using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories
{
    public class CashierRepository : DefaultRepository<Cashier>, ICashierRepository
    {
        public CashierRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public IQueryable<Cashier> GetAllOrderedByFullName()
        {
            return _db.Set<Cashier>()
                .OrderBy(c => c.FullName)
                .AsNoTracking();
        }
    }
}