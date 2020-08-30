using CoffeeHouse.Data;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public class OrderProdRepository : IOrderProdRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IOrderRepository _orderRepository;

        public OrderProdRepository(
            ApplicationDbContext context,
            IOrderRepository orderRepository)
        {
            _db = context;
            _orderRepository = orderRepository;
        }

        public OrderProd GetByOrderIdAndProductIdAndMark(int orderId, int productId, string mark)
        {
            var order = _orderRepository.GetByIdWithOrderProds(orderId);
            return order?.OrderProds
                .SingleOrDefault(op => 
                    op.ProductId == productId 
                    && op.Mark == mark);
        }

        public async Task<OrderProd> GetByOrderIdAndProductIdAndMarkAsync(int orderId, int productId, string mark)
        {
            return await Task.Run(() => 
                GetByOrderIdAndProductIdAndMark(orderId, productId, mark));
        }

        public OrderProd GetByOrderIdAndProductIdAndMarkWithProduct(int orderId, int productId, string mark)
        {
            var order = _orderRepository.GetByIdWithOrderProdsAndProducts(orderId);
            return order?.OrderProds
                .SingleOrDefault(op =>
                    op.ProductId == productId
                    && op.Mark == mark);
        }

        public async Task<OrderProd> GetByOrderIdAndProductIdAndMarkWithProductAsync(int orderId, int productId, string mark)
        {
            return await Task.Run(() =>
                GetByOrderIdAndProductIdAndMarkWithProduct(orderId, productId, mark));
        }

        public void Add(OrderProd entity)
        {
            var order = _orderRepository.GetByIdWithOrderProdsAsTracking(entity.OrderId);
            if (order != null)
            {
                order.OrderProds.Add(entity);
                _db.SaveChanges();
            }
        }

        public async Task AddAsync(OrderProd entity)
        {
            await Task.Run(() => Add(entity)); 
        }

        public void Update(OrderProd entity)
        {
            Remove(entity);
            Add(entity);
        }

        public async Task UpdateAsync(OrderProd entity)
        {
            await Task.Run(() => Update(entity));
        }

        public void Update(OrderProd oldEntity, OrderProd newEntity)
        {
            Remove(oldEntity);
            Add(newEntity);
        }

        public async Task UpdateAsync(OrderProd oldEntity, OrderProd newEntity)
        {
            await Task.Run(() => Update(oldEntity, newEntity));
        }

        public void Remove(OrderProd entity)
        {
            var order = _orderRepository.GetByIdWithOrderProdsAsTracking(entity.OrderId);
            if (order != null)
            {
                order.OrderProds
                    .RemoveAll(op => 
                        op.Mark == entity.Mark 
                        && op.ProductId == entity.ProductId);
                _db.SaveChanges();
            }
        }

        public async Task RemoveAsync(OrderProd entity)
        {
            await Task.Run(() => Remove(entity));
        }

        public bool Exists(OrderProd entity)
        {
            var order = _orderRepository.GetByIdWithOrderProds(entity.OrderId);
            if (order != null)
            {
                return order.OrderProds
                    .Any(op => 
                        op.ProductId == entity.ProductId 
                        && op.Mark == entity.Mark);
            }
            return false;
        }
    }
}