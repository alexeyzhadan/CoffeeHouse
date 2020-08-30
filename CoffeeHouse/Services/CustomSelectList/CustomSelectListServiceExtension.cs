using CoffeeHouse.Services.CustomSelectList.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeHouse.Services.CustomSelectList
{
    public static class CustomSelectListServiceExtension
    {
        public static IServiceCollection AddCustomSelectList(this IServiceCollection services)
            => services.AddScoped<ICustomSelectList, CustomSelectList>();
    }
}