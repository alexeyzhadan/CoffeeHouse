using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using CoffeeHouse.Services.CustomSelectList.Interfaces;

namespace CoffeeHouse.Controllers
{
    public class OrderProdsController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProdRepository _orderProdRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomSelectList _customSelectList;

        public OrderProdsController(
            IOrderRepository orderRepository,
            IOrderProdRepository orderProdRepository,
            IProductRepository productRepository,
            ICustomSelectList customSelectList)
        {
            _orderRepository = orderRepository;
            _orderProdRepository = orderProdRepository;
            _productRepository = productRepository;
            _customSelectList = customSelectList;
        }

        public async Task<IActionResult> Index(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            var order = await _orderRepository
                .GetByIdWithAllInclusiveDataAsync((int)orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> Create(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetByIdAsync((int)orderId);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderProd { OrderId = (int)orderId };

            ViewData["ProductId"] = _customSelectList.CreateListOfProductFullNames();
            ViewData["Mark"] = _customSelectList.CreateListOfOrderProdMarks();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,Mark,Count")] OrderProd orderProd)
        {
            if (ModelState.IsValid)
            {
                await _orderProdRepository.AddAsync(orderProd);
                return RedirectToAction(nameof(Index), new { orderId = orderProd.OrderId });
            }
            ViewData["ProductId"] = _customSelectList.CreateListOfProductFullNames(orderProd.ProductId);
            ViewData["Mark"] = _customSelectList.CreateListOfOrderProdMarks(orderProd.Mark);
            return View(orderProd);
        }

        public async Task<IActionResult> Edit(int? orderId, int? productId, string mark)
        {
            if (orderId == null || productId == null || mark == null)
            {
                return NotFound();
            }

            var orderProd = await _orderProdRepository
                .GetByOrderIdAndProductIdAndMarkAsync((int)orderId, (int)productId, mark);
            if (orderProd == null)
            {
                return NotFound();
            }

            ViewData["ProductId"] = _customSelectList.CreateListOfProductFullNames(orderProd.ProductId);
            ViewData["Mark"] = _customSelectList.CreateListOfOrderProdMarks(orderProd.Mark);
            return View(orderProd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int oldOrderId, 
            int oldProductId, 
            string oldMark, 
            [Bind("OrderId,ProductId,Mark,Count")] OrderProd orderProd)
        {
            if (orderProd.OrderId != oldOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var oldOrderProd = new OrderProd 
                { 
                    OrderId = oldOrderId, 
                    ProductId = oldProductId, 
                    Mark = oldMark 
                };

                if (_orderProdRepository.Exists(oldOrderProd))
                {
                    await _orderProdRepository.UpdateAsync(oldOrderProd, orderProd);
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index), new { orderId = orderProd.OrderId });
            }
            ViewData["ProductId"] = _customSelectList.CreateListOfProductFullNames(orderProd.ProductId);
            ViewData["Mark"] = _customSelectList.CreateListOfOrderProdMarks(orderProd.Mark);
            return View(orderProd);
        }

        public async Task<IActionResult> Delete(int? orderId, int? productId, string mark)
        {
            if (orderId == null || productId == null || mark == null)
            {
                return NotFound();
            }

            var orderProd = await _orderProdRepository
                .GetByOrderIdAndProductIdAndMarkWithProductAsync((int)orderId, (int)productId, mark);
            if (orderProd == null)
            {
                return NotFound();
            }

            return View(orderProd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int orderId, int productId, string mark)
        {
            var orderProd = await _orderProdRepository
                .GetByOrderIdAndProductIdAndMarkAsync(orderId, productId, mark);
            if (orderProd == null)
            {
                return NotFound();
            }

            await _orderProdRepository.RemoveAsync(orderProd);
            return RedirectToAction(nameof(Index), new { orderId });
        }
    }
}