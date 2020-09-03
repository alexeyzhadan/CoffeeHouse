using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeHouse.Models;
using CoffeeHouse.ViewModels;
using CoffeeHouse.Services.Accounts.Interfaces;

namespace CoffeeHouse.Controllers
{
    public class CashiersController : Controller
    {
        private readonly IAccountManager _accountManager;

        public CashiersController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _accountManager.GetUsersAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,UserName,PhoneNumber,Email,Password,ConfirmPassword,IsAdmin")] RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    FullName = user.FullName,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    IsAdmin = user.IsAdmin
                };
                await _accountManager.CreateAsync(newUser, user.Password);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult ChangePassword(string id)
        {
            if (id == null || !_accountManager.Exists(id))
            {
                return NotFound();
            }

            var model = new PasswordViewModel 
            {
                Id = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, [Bind("Id,Password,ConfirmPassword")] PasswordViewModel newPassword)
        {
            if (id != newPassword.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _accountManager.ChangePasswordAsync(id, newPassword.Password);
                return RedirectToAction(nameof(Index));
            }
            return View(newPassword);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || !_accountManager.Exists(id))
            {
                return NotFound();
            }

            var user = await _accountManager.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,UserName,PhoneNumber,Email,IsAdmin")] User user)
        {
            if (id != user.Id || !_accountManager.Exists(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _accountManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || !_accountManager.Exists(id))
            {
                return NotFound();
            }

            var user = await _accountManager.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            if (id == null || !_accountManager.Exists(id))
            {
                return NotFound();
            }

            var user = await _accountManager.GetUserByIdAsync(id);

            await _accountManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
