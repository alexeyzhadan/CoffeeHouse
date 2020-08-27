using CoffeeHouse.Models;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IClientRepository : IDefaultEntityRepository<Client>
    {
        IQueryable<Client> GetAllOrderedByName();
    }
}