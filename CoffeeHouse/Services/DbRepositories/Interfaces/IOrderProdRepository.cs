using CoffeeHouse.Models;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface IOrderProdRepository : IRepository<OrderProd>
    {
        OrderProd GetByOrderIdAndProductIdAndMark(int orderId, int productId, string mark);
        Task<OrderProd> GetByOrderIdAndProductIdAndMarkAsync(int orderId, int productId, string mark);
        OrderProd GetByOrderIdAndProductIdAndMarkWithProduct(int orderId, int productId, string mark);
        Task<OrderProd> GetByOrderIdAndProductIdAndMarkWithProductAsync(int orderId, int productId, string mark);
        void Update(OrderProd oldEntity, OrderProd newEntity);
        Task UpdateAsync(OrderProd oldEntity, OrderProd newEntity);
    }
}