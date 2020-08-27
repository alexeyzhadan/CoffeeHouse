using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories
{
    public class ClientRepository : DefaultRepository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public IQueryable<Client> GetAllOrderedByName()
        {
            return _db.Set<Client>()
                .OrderBy(c => c.Name)
                .AsNoTracking();
        }
    }
}