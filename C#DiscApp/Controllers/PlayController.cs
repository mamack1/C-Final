using Microsoft.AspNetCore.Mvc;
using C_DiscApp.Data;
using C_DiscApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace C_DiscApp.Controllers
{
    [Authorize]
    public class PlayController : Controller
    {
        private readonly DiscContext _context;
        private readonly IDiscService _discService;
        private readonly ILogger<InventoryController> _logger;
        private readonly UserManager<User> _userManager;

        public PlayController(DiscContext context, IDiscService discService, ILogger<InventoryController> logger, UserManager<User> userManager)
        {
            _context = context;
            _discService = discService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Start(string name, string address, double distance, int numberOfHoles, int rating)
        {
            ViewBag.CourseName = name;
            ViewBag.CourseAddress = address;
            ViewBag.CourseDistance = distance;
            ViewBag.NumberOfHoles = numberOfHoles; // Set the number of holes here
            ViewBag.CurrentHole = 1; // Initialize the current hole if needed
            return View("Play"); // Ensure this returns the correct view
        }

        [HttpPost]
        public async Task<IActionResult> SaveGame(int numberOfHoles, string courseName, List<int> pars, List<int> throws, int rating)
        {
            if (string.IsNullOrWhiteSpace(courseName))
            {
                ModelState.AddModelError("", "Course name is required.");
                return View("Play");
            }

            if (pars.Count != numberOfHoles || throws.Count != numberOfHoles)
            {
                ModelState.AddModelError("", "Mismatch between number of holes and provided scores.");
                return View("Play");
            }

            var userId = _userManager.GetUserId(User);
            var gameHistory = new GameHistory
            {
                CourseName = courseName,
                NumberOfHoles = numberOfHoles,
                TotalParThrows = pars.Sum(),
                TotalThrows = throws.Sum(),
                UserId = userId,
                Rating = rating,
                DatePlayed = DateTime.UtcNow
            };

            _context.GameHistories.Add(gameHistory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "History");
        }
    }
}
