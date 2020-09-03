using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeHouse.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _clientRepository
                .GetAllOrderedByName()
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Phone,Cashback")] Client client)
        {
            if (ModelState.IsValid)
            {
                await _clientRepository.AddAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetByIdAsync((int)id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Cashback")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_clientRepository.Exists(client))
                {
                    await _clientRepository.UpdateAsync(client);
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetByIdAsync((int)id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            await _clientRepository.RemoveAsync(client);
            return RedirectToAction(nameof(Index));
        }
    }
}
