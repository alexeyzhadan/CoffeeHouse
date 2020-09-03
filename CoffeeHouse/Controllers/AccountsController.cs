using System.Threading.Tasks;
using CoffeeHouse.Services.Accounts.Interfaces;
using CoffeeHouse.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHouse.Controllers
{
    public class AccountsController : Controller
    {
        private const string ORDER_REGISTRATION = "OrderRegistration";
        private const string INDEX = "Index";
        private const string ADMIN = "Admin";

        private readonly IAccountManager _accountManager;

        public AccountsController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.SignInAsync(login.UserName, login.Password);
                if (result)
                {
                    if (await _accountManager.UserIsAdminAsync(login.UserName))
                    {
                        return RedirectToAction(INDEX, ADMIN);
                    }
                    return RedirectToAction(INDEX, ORDER_REGISTRATION);
                }
            }
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
