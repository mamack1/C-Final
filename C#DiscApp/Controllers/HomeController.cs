using Microsoft.AspNetCore.Mvc;

namespace C_DiscApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            return View();
        }

        public IActionResult Course()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}

