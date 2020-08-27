using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeHouse.Services.DbRepositories
{
    public static class RepositoriesServiceExtension
    {
        public static IServiceCollection AddDbRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICashierRepository, CashierRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderProdRepository, OrderProdRepository>();

            return services;
        }
    }
}