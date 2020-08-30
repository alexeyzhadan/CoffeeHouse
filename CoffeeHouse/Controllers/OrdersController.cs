using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICashierRepository _cashierRepository;

        public OrdersController(
            IOrderRepository orderRepository,
            IClientRepository clientRepository,
            ICashierRepository cashierRepository)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _cashierRepository = cashierRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _orderRepository
                .GetAllWithCashiersAndClientsOrderedByDate()
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CashierId"] = GetSelectListOfCashiers();
            ViewData["ClientId"] = GetSelectListOfClients();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,ClientId,CashierId")] Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.AddAsync(order);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CashierId"] = GetSelectListOfCashiers(order.CashierId);
            ViewData["ClientId"] = GetSelectListOfClients((int)order.ClientId);
            return View(order);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetByIdAsync((int)id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CashierId"] = GetSelectListOfCashiers(order.CashierId);
            ViewData["ClientId"] = GetSelectListOfClients((int)order.ClientId);
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
                if (_orderRepository.Exists(order))
                {
                    await _orderRepository.UpdateAsync(order);
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CashierId"] = GetSelectListOfCashiers(order.CashierId);
            ViewData["ClientId"] = GetSelectListOfClients((int)order.ClientId);
            return View(order);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetByIdAsync((int)id);
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
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.RemoveAsync(order);
            return RedirectToAction(nameof(Index));
        }

        private SelectList GetSelectListOfCashiers()
        { 
            return new SelectList(_cashierRepository.GetAllOrderedByFullName(), "Id", "FullName");
        }

        private SelectList GetSelectListOfCashiers(int defaultItemId)
        {
            return new SelectList(_cashierRepository.GetAllOrderedByFullName(), "Id", "FullName", defaultItemId);
        }

        private SelectList GetSelectListOfClients()
        { 
            return new SelectList(_clientRepository.GetAllOrderedByName(), "Id", "Name");
        }

        private SelectList GetSelectListOfClients(int defaultItemId)
        {
            return new SelectList(_clientRepository.GetAllOrderedByName(), "Id", "Name", defaultItemId);
        }
    }
}
