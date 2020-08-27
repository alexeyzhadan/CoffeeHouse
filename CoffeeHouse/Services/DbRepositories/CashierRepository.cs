using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class CashierRepository : BaseRepository<Cashier>, ICashierRepository
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

        public Cashier GetById(int id)
        {
            return GetAll().SingleOrDefault(c => c.Id == id);
        }

        public async Task<Cashier> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
        }

        public override bool Exists(Cashier entity)
        {
            return GetAll().Any(c => c.Id == entity.Id);
        }
    }
}