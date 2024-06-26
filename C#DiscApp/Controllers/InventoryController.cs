using Microsoft.AspNetCore.Mvc;

namespace C_DiscApp.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
