using CoffeeHouse.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.Accounts.Interfaces
{
    public interface IAccountManager
    {
        List<User> GetUsers();
        Task<List<User>> GetUsersAsync();
        User GetUserById(string id);
        Task<User> GetUserByIdAsync(string id);
        Task CreateAsync(User user, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task ChangePasswordAsync(string user, string newPassword);
        bool Exists(string id);
    }
}
