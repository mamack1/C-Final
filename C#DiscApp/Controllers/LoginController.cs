using Microsoft.AspNetCore.Mvc;

namespace C_DiscApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
