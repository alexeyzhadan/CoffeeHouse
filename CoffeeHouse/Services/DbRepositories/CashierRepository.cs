using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        public Cashier GetByUserName(string userName)
        {
            return _db.Set<Cashier>()
                .AsNoTracking()
                .SingleOrDefault(c => c.UserName == userName);
        }

        public async Task<Cashier> GetByUserNameAsync(string userName)
        {
            return await Task.Run(() => GetByUserName(userName));
        }

        public Cashier GetByUserNameAsTracking(string userName)
        {
            return _db.Set<Cashier>()
                .SingleOrDefault(c => c.UserName == userName);
        }

        public async Task<Cashier> GetByUserNameAsTrackingAsync(string userName)
        {
            return await Task.Run(() => GetByUserNameAsTracking(userName));
        }
    }
}