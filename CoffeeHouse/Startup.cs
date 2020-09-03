using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CoffeeHouse.Services.DbRepositories;
using CoffeeHouse.Services.CustomSelectList;
using CoffeeHouse.Services.Accounts;
using System.Linq;

namespace CoffeeHouse
{
    public class Startup
    {
        private const string DEFAULT_CONNECTION = "DefaultConnection";
        private const string ADMIN = "Admin";
        private const string USER = "User";
        private const string CASHIER_ID = "CashierId";
        private const string HOME_ERROR = "/Home/Error";
        private const string DEFAULT = "default";
        private const string DEFAULT_PATH = "{controller=Home}/{action=Index}";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DEFAULT_CONNECTION)));

            services.AddDefaultIdentity<IdentityUser>(options => 
                { 
                    options.SignIn.RequireConfirmedAccount = false;

                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options => 
            {
                options.AddPolicy(USER, policyBuilder =>
                    policyBuilder.RequireAuthenticatedUser().RequireClaim(CASHIER_ID));

                options.AddPolicy(ADMIN, policyBuilder =>
                    policyBuilder.RequireAuthenticatedUser().RequireAssertion(context =>
                        !context.User.Claims.Any(c => c.Type == CASHIER_ID)));
            });

            services.AddDbRepositories();
            services.AddCustomSelectList();
            services.AddAccountManager();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(HOME_ERROR);
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: DEFAULT,
                    pattern: DEFAULT_PATH);
                endpoints.MapRazorPages();
            });
        }
    }
}