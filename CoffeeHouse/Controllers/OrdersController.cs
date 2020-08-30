using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using CoffeeHouse.Services.CustomSelectList.Interfaces;

namespace CoffeeHouse.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICashierRepository _cashierRepository;
        private readonly ICustomSelectList _customSelectList;

        public OrdersController(
            IOrderRepository orderRepository,
            IClientRepository clientRepository,
            ICashierRepository cashierRepository,
            ICustomSelectList customSelectList)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _cashierRepository = cashierRepository;
            _customSelectList = customSelectList;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _orderRepository
                .GetAllWithCashiersAndClientsOrderedByDate()
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CashierId"] = _customSelectList.CreateListOfCashierFullNames();
            ViewData["ClientId"] = _customSelectList.CreateListOfClientNames();
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
            ViewData["CashierId"] = _customSelectList.CreateListOfCashierFullNames(order.CashierId);
            ViewData["ClientId"] = _customSelectList.CreateListOfClientNames((int)order.ClientId);
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
            ViewData["CashierId"] = _customSelectList.CreateListOfCashierFullNames(order.CashierId);
            ViewData["ClientId"] = _customSelectList.CreateListOfClientNames((int)order.ClientId);
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
            ViewData["CashierId"] = _customSelectList.CreateListOfCashierFullNames(order.CashierId);
            ViewData["ClientId"] = _customSelectList.CreateListOfClientNames((int)order.ClientId);
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
    }
}
