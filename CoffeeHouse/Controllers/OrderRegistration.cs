using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHouse.Controllers
{
    [Authorize(Policy = "User")]
    public class OrderRegistration : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
