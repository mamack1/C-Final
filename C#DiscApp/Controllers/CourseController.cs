using Microsoft.AspNetCore.Mvc;

namespace C_DiscApp.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
