using CoffeeHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        IQueryable<Client> GetAllOrderedByName();
        Client GetById(int id);
        Task<Client> GetByIdAsync(int id);
    }
}