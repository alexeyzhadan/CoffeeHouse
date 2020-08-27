using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class OrderProdRepository : IOrderProdRepository
    {
        public OrderProdRepository(ApplicationDbContext context)
        {
        }

        public void Add(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }

        public void Update(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(OrderProd entity)
        {
            throw new System.NotImplementedException();
        }
    }
}