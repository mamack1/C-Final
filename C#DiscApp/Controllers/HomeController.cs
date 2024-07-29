using C_DiscApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace C_DiscApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Set a session value
            HttpContext.Session.SetString("SessionKeyName", "SessionValue");

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
        public IActionResult About()
        {
            // Get the session value
            var sessionValue = HttpContext.Session.GetString("SessionKeyName");

            ViewData["Message"] = $"Session Value: {sessionValue}";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}