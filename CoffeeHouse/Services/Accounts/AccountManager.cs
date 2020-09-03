using CoffeeHouse.Models;
using CoffeeHouse.Services.Accounts.Interfaces;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.Accounts
{
    public class AccountManager : IAccountManager
    {
        private const string CASHIER_ID = "CashierId";

        private readonly ICashierRepository _cashierRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountManager(
            ICashierRepository cashierRepository,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _cashierRepository = cashierRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public List<User> GetUsers()
        {
            var cashiers = _cashierRepository
                .GetAll()
                .ToList();
            var users = cashiers.Join(
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
                            c.Type == CASHIER_ID)
                })
                .OrderBy(m => m.IsAdmin)
                .ThenBy(m => m.FullName)
                .ToList();

            return users;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await Task.Run(() => GetUsers());
        }

        public User GetUserById(string id)
        {
            var identityUser = _userManager.FindByIdAsync(id).Result;
            var user = new User
            {
                Id = identityUser.Id,
                UserName = identityUser.UserName,
                Email = identityUser.Email,
                PhoneNumber = identityUser.PhoneNumber,
                FullName = _cashierRepository
                    .GetByUserName(identityUser.UserName)
                    .FullName,
                IsAdmin = !_userManager
                    .GetClaimsAsync(identityUser)
                    .Result
                    .Any(c =>
                        c.Type == CASHIER_ID)
            };

            return user;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await Task.Run(() => GetUserById(id));
        }

        public async Task CreateAsync(User user, string password)
        {
            var identityUser = new IdentityUser
            {
                Email = user.Email,
                EmailConfirmed = true,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = true,
                UserName = user.UserName
            };
            var result = await _userManager.CreateAsync(identityUser, password);

            if (result.Succeeded)
            {
                var cashier = new Cashier 
                {
                    FullName = user.FullName,
                    UserName = user.UserName
                };
                await _cashierRepository.AddAsync(cashier);

                if (!user.IsAdmin)
                {
                    await _userManager.AddClaimAsync(
                        identityUser, 
                        new Claim(CASHIER_ID, cashier.Id.ToString()));
                }
            }
        }

        public async Task UpdateAsync(User user)
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

                var claimsCashier = await _userManager.GetClaimsAsync(identityUser);
                var claimCashierId = claimsCashier.SingleOrDefault(c => c.Type == CASHIER_ID);

                if (claimCashierId == null && !user.IsAdmin)
                {
                    await _userManager.AddClaimAsync(
                        identityUser,
                        new Claim(CASHIER_ID, cashier.Id.ToString()));
                }
                else if (claimCashierId != null && user.IsAdmin)
                {
                    await _userManager.RemoveClaimAsync(identityUser, claimCashierId);
                }

                await _cashierRepository.UpdateAsync(cashier);
                await _userManager.UpdateAsync(identityUser);
            }
        }

        public async Task DeleteAsync(User user)
        {
            var cashier = await _cashierRepository.GetByUserNameAsTrackingAsync(user.UserName);
            var identityUser = await _userManager.FindByIdAsync(user.Id);

            await _cashierRepository.RemoveAsync(cashier);
            await _userManager.DeleteAsync(identityUser);
        }

        public async Task ChangePasswordAsync(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);
        }

        public bool Exists(string id)
        {
            return _userManager.Users.Any(u => u.Id == id);
        }

        public async Task<bool> SignInAsync(string userName, string password)
        {
            var result = await _signInManager
                .PasswordSignInAsync(userName, password, false, false);

            return result.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> UserIsAdminAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var claims = await _userManager.GetClaimsAsync(user);
            return !claims.Any(c => c.Type == CASHIER_ID);
        }
    }
}
