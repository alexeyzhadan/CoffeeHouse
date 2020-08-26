using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Data;
using CoffeeHouse.Models;
using System.Collections.Generic;

namespace CoffeeHouse.Controllers
{
    public class OrderProdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderProdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Cashier)
                .Include(o => o.OrderProds)
                    .ThenInclude(op => op.Product)
                .AsNoTracking()
                .SingleAsync(o => o.Id == orderId);
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

            var order = await _context.Orders
                .AsNoTracking()
                .SingleAsync(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderProd { OrderId = (int)orderId };

            ViewData["ProductId"] = new SelectList(GetProductFullNames(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,Mark,Count")] OrderProd orderProd)
        {
            if (ModelState.IsValid)
            {
                _context.Orders
                    .Include(o => o.OrderProds)
                    .Single(o => o.Id == orderProd.OrderId)
                    .OrderProds.Add(orderProd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { orderId = orderProd.OrderId });
            }
            ViewData["ProductId"] = new SelectList(GetProductFullNames(), "Id", "Name", orderProd.ProductId);
            return View(orderProd);
        }

        public async Task<IActionResult> Edit(int? orderId, int? productId, string mark)
        {
            if (orderId == null || productId == null || mark == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderProds)
                    .ThenInclude(op => op.Product)
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.Id == orderId);
            var orderProd = order.OrderProds
                .Single(op => op.ProductId == productId 
                    && op.Mark == mark);
            if (orderProd == null)
            {
                return NotFound();
            }

            ViewData["ProductId"] = new SelectList(GetProductFullNames(), "Id", "Name", orderProd.ProductId);
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
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.OrderProds)
                        .SingleAsync(o => o.Id == oldOrderId);
                    order.OrderProds.RemoveAll(op => op.Mark == oldMark
                        && op.ProductId == oldProductId);
                    await _context.SaveChangesAsync();
                    order.OrderProds.Add(orderProd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderProdExists(oldOrderId, oldProductId, oldMark))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { orderId = orderProd.OrderId });
            }
            ViewData["ProductId"] = new SelectList(GetProductFullNames(), "Id", "Name", orderProd.ProductId);
            return View(orderProd);
        }

        public async Task<IActionResult> Delete(int? orderId, int? productId, string mark)
        {
            if (orderId == null || productId == null || mark == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderProds)
                    .ThenInclude(op => op.Product)
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.Id == orderId);
            var orderProd = order.OrderProds
                .Single(op => op.Mark == mark
                    && op.ProductId == productId);
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
            var order = await _context.Orders
                .Include(o => o.OrderProds)
                .SingleOrDefaultAsync(o => o.Id == orderId);
            order.OrderProds.RemoveAll(op => op.Mark == mark
                && op.ProductId == productId);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { orderId });
        }

        private bool OrderProdExists(int orderId, int productId, string mark)
        {
            return _context.Orders
                .Include(o => o.OrderProds)
                .AsNoTracking()
                .SingleOrDefault(o => o.Id == orderId)
                .OrderProds.Any(op => op.Mark == mark 
                    && op.ProductId == productId);
        }

        private IEnumerable<object> GetProductFullNames()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Category.Name)
                    .ThenBy(p => p.Name)
                .AsNoTracking();
            var fullNameProducts = products.Select(p => new 
            {
                Id = p.Id,
                Name = p.Name + ", " + p.Quantity
            }).AsEnumerable<object>();
            return fullNameProducts;
        }
    }
}