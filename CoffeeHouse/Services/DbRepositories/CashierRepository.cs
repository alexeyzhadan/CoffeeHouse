using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Services.DbRepositories
{
    public class CashierRepository : BaseRepository<Cashier>, ICashierRepository
    {
        public CashierRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public override bool Exists(Cashier entity)
        {
            throw new System.NotImplementedException();
        }
    }
}