using Microsoft.AspNetCore.Mvc;

namespace C_DiscApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
