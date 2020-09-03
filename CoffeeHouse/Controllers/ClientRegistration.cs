using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHouse.Controllers
{
    [Authorize(Policy = "User")]
    public class ClientRegistration : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
