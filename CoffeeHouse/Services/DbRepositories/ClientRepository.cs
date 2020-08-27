using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Services.DbRepositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public override bool Exists(Client entity)
        {
            throw new System.NotImplementedException();
        }
    }
}