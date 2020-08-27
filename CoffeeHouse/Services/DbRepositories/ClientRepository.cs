using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
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

        public Client GetById(int id)
        {
            return GetAll().SingleOrDefault(c => c.Id == id);
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
        }

        public override bool Exists(Client entity)
        {
            return GetAll().Any(c => c.Id == entity.Id);
        }
    }
}