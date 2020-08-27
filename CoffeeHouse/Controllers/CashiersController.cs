using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Controllers
{
    public class CashiersController : Controller
    {
        private readonly ICashierRepository _cashierRepository;

        public CashiersController(ICashierRepository cashierRepository)
        {
            _cashierRepository = cashierRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _cashierRepository
                .GetAllOrderedByFullName()
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,FullName")] Cashier cashier)
        {
            if (ModelState.IsValid)
            {
                await _cashierRepository.AddAsync(cashier);
                return RedirectToAction(nameof(Index));
            }
            return View(cashier);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _cashierRepository.GetByIdAsync((int)id);
            if (cashier == null)
            {
                return NotFound();
            }
            return View(cashier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,FullName")] Cashier cashier)
        {
            if (id != cashier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_cashierRepository.Exists(cashier))
                {
                    await _cashierRepository.UpdateAsync(cashier);
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cashier);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _cashierRepository.GetByIdAsync((int)id);
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cashier = await _cashierRepository.GetByIdAsync(id);
            if (cashier == null)
            {
                return NotFound();
            }

            await _cashierRepository.RemoveAsync(cashier);
            return RedirectToAction(nameof(Index));
        }
    }
}
