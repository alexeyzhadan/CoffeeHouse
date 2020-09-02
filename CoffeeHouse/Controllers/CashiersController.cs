using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using CoffeeHouse.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoffeeHouse.Controllers
{
    public class CashiersController : Controller
    {
        private readonly ICashierRepository _cashierRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CashiersController(
            ICashierRepository cashierRepository,
            UserManager<IdentityUser> userManager)
        {
            _cashierRepository = cashierRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cashiers = await _cashierRepository.GetAllOrderedByFullName().ToListAsync();
            var model = cashiers.Join(
                _userManager.Users,
                c => c.UserName,
                u => u.UserName,
                (c, u) => new User
                {
                    Id = u.Id,
                    UserName = c.UserName,
                    FullName = c.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    IsAdmin = !_userManager
                        .GetClaimsAsync(u)
                        .Result
                        .Any(c =>
                            c.Type == "CashierId")
                })
                .OrderBy(m => m.IsAdmin)
                .ThenBy(m => m.FullName)
                .ToList();

            return View(model);
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
                var identityUser = new IdentityUser
                {
                    Email = user.Email,
                    EmailConfirmed = true,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    UserName = user.UserName
                };
                var result = await _userManager.CreateAsync(identityUser, user.Password);

                if (result.Succeeded)
                {
                    var cashier = new Cashier { FullName = user.FullName, UserName = user.UserName };
                    await _cashierRepository.AddAsync(cashier);

                    if (!user.IsAdmin)
                    {
                        await _userManager.AddClaimAsync(identityUser, new Claim("CashierId", cashier.Id.ToString()));
                    }
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
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
                var user = await _userManager.FindByIdAsync(newPassword.Id);
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, newPassword.Password);

                return RedirectToAction(nameof(Index));
            }
            return View(newPassword);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new User 
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = _cashierRepository.GetByUserName(user.UserName).FullName,
                IsAdmin = !_userManager
                    .GetClaimsAsync(user)
                    .Result
                    .Any(c =>
                        c.Type == "CashierId")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,UserName,PhoneNumber,Email,IsAdmin")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id);
                if (identityUser != null)
                {
                    var cashier = await _cashierRepository.GetByUserNameAsTrackingAsync(identityUser.UserName);
                    cashier.UserName = user.UserName;
                    cashier.FullName = user.FullName;

                    identityUser.UserName = user.UserName;
                    identityUser.PhoneNumber = user.PhoneNumber;
                    identityUser.Email = user.Email;

                    var claimCashierId = (await _userManager.GetClaimsAsync(identityUser))
                        .SingleOrDefault(c => c.Type == "CashierId");

                    if (claimCashierId == null && !user.IsAdmin)
                    {
                        await _userManager.AddClaimAsync(identityUser, new Claim("CashierId", cashier.Id.ToString()));
                    }
                    else if (claimCashierId != null && user.IsAdmin)
                    {
                        await _userManager.RemoveClaimAsync(identityUser, claimCashierId);
                    }

                    await _cashierRepository.UpdateAsync(cashier);
                    await _userManager.UpdateAsync(identityUser);
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = _cashierRepository.GetByUserName(user.UserName).FullName,
                IsAdmin = !_userManager
                    .GetClaimsAsync(user)
                    .Result
                    .Any(c =>
                        c.Type == "CashierId")
            };

            return View(model);
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _cashierRepository.RemoveAsync(
                _cashierRepository.GetByUserNameAsTracking(user.UserName));
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
