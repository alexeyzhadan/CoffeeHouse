using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Data;
using CoffeeHouse.Models;

namespace CoffeeHouse.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders
                .Include(o => o.Cashier)
                .Include(o => o.Client)
                .AsNoTracking()
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CashierId"] = new SelectList(_context.Cashiers.AsNoTracking(), "Id", "FullName");
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,ClientId,CashierId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CashierId"] = new SelectList(_context.Cashiers.AsNoTracking(), "Id", "FullName", order.CashierId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "Name", order.ClientId);
            return View(order);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .AsNoTracking()
                .SingleAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CashierId"] = new SelectList(_context.Cashiers.AsNoTracking(), "Id", "FullName", order.CashierId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "Name", order.ClientId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,ClientId,CashierId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CashierId"] = new SelectList(_context.Cashiers.AsNoTracking(), "Id", "FullName", order.CashierId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "Name", order.ClientId);
            return View(order);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Cashier)
                .Include(o => o.Client)
                .SingleAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .SingleAsync(o => o.Id == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.AsNoTracking().Any(e => e.Id == id);
        }
    }
}
