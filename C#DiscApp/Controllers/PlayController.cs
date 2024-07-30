using Microsoft.AspNetCore.Mvc;
using C_DiscApp.Data;
using C_DiscApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C_DiscApp.Controllers
{
    public class PlayController : Controller
    {
        private readonly GameHistoryContext _context;

        public PlayController(GameHistoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Start(string name, string address, double distance, int numberOfHoles)
        {
            ViewBag.CourseName = name;
            ViewBag.CourseAddress = address;
            ViewBag.CourseDistance = distance;
            ViewBag.NumberOfHoles = numberOfHoles; // Set the number of holes here
            ViewBag.CurrentHole = 1; // Initialize the current hole if needed
            return View("Play"); // Ensure this returns the correct view
        }

        [HttpPost]
        public IActionResult SaveGame(int numberOfHoles, string courseName, List<int> pars, List<int> throws)
        {
            if (string.IsNullOrEmpty(courseName))
            {
                ModelState.AddModelError("", "Course name is required.");
                return View("Play");
            }

            if (pars.Count != numberOfHoles || throws.Count != numberOfHoles)
            {
                ModelState.AddModelError("", "Mismatch between number of holes and provided scores.");
                return View("Play");
            }

            var gameHistory = new GameHistory
            {
                CourseName = courseName,
                NumberOfHoles = numberOfHoles,
                DatePlayed = DateTime.Now,
                TotalParThrows = pars.Sum(),
                TotalThrows = throws.Sum()
            };

            _context.GameHistories.Add(gameHistory);
            _context.SaveChanges();


            return RedirectToAction("Index", "History");
        }
    }
}
