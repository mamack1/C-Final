using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace C_DiscApp.Controllers
{
    public class PlayController : Controller
    {
        public IActionResult Start(string name, string address, double distance)
        {
            ViewBag.CourseName = name;
            ViewBag.CourseAddress = address;
            ViewBag.CourseDistance = distance;
            return View();
        }

        [HttpPost]
        public IActionResult Start(int numberOfHoles)
        {
            ViewBag.NumberOfHoles = numberOfHoles;
            ViewBag.CurrentHole = 1;
            return View("Play");
        }

        [HttpPost]
        public IActionResult Play(int numberOfHoles, int currentHole, string action, List<int> pars, List<int> throws)
        {
            if (action == "Next")
            {
                currentHole++;
            }
            else if (action == "Previous")
            {
                currentHole--;
            }

            ViewBag.NumberOfHoles = numberOfHoles;
            ViewBag.CurrentHole = currentHole;
            ViewBag.Pars = pars;
            ViewBag.Throws = throws;
            return View();
        }
    }
}
