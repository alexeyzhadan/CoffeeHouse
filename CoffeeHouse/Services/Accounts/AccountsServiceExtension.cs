using CoffeeHouse.Services.Accounts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeHouse.Services.Accounts
{
    public static class AccountsServiceExtension
    {
        public static IServiceCollection AddAccountManager(this IServiceCollection services)
            => services.AddScoped<IAccountManager, AccountManager>();
    }
}